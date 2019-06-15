using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Compression.Huffman;
using Compression.LZ;
using Compression.PPM;
using System.Threading;
using Microsoft.SqlServer.Server;

namespace Compression {
    internal class Program {
        public static void Main(string[] args) {
            if (args.Length < 1) {
                Console.WriteLine("Usage: <input path> [-o <output path>] [-A <algorithm (lz, ppm, huffman)>]");
                return;
            }

            DataFile input;
            string outputPath;
            Stopwatch watch = new Stopwatch();
            ICompressor compressor = new LZSS();
            var fileExtension = "lz";
            try {
                var inputPath = args[0];
                input = new DataFile(inputPath);

                if (args.Contains("-A")) {
                    var i = Array.IndexOf(args, "-A") + 1;
                    Console.WriteLine($"{args[i]}");
                    switch (args[i]) {
                        case "lz":
                            compressor = new LZSS();
                            fileExtension = "lz";
                            break;
                        case "ppm":
                            compressor = new PredictionByPartialMatching();
                            fileExtension = "ppm";
                            break;
                        case "huffman":
                            compressor = new HuffmanCompressor();
                            fileExtension = "huffman";
                            break;
                        default:
                            throw new InvalidCompressorException();
                    }
                }
                
                var compress = !args.Contains("-D");

                if (!compress) {
                    outputPath = Path.GetFullPath(inputPath);
                    outputPath = Path.ChangeExtension(outputPath, null);
                }
                else 
                    outputPath = Path.GetFullPath(inputPath) + "." + fileExtension;
                
                if (args.Contains("-o")) {
                    var i = Array.IndexOf(args, "-o") + 1;
                    outputPath = args[i];
                }

                Console.WriteLine("{0}ompressing file with algorithm {1}.", compress ? "C" : "Dec", fileExtension);
                DataFile output;
                Thread t = new Thread(PrintStatus);
                
                t.Start(compressor);
                watch.Start();
                if (compress) {
                    output = compressor.Compress(input);
                }
                else
                    output = compressor.Decompress(input);
                
                output.WriteToFile(outputPath);
                Console.WriteLine("\rPercent: 100.0%");
                Console.WriteLine($"Elapsed time: {watch.Elapsed} Ratio: {(double)output.Length / input.Length}");
                Console.WriteLine("Compression speed: " + (double) input.Length / watch.ElapsedMilliseconds + " kb/s");
                Console.WriteLine("File written to {0}", outputPath);
                t.Abort();
            }
            catch (FileNotFoundException e) {
                Console.WriteLine("File was not found: {0}", e.Message);
            }
            catch (InvalidCompressorException) {
                Console.WriteLine("The compression algorithm specified does not exist");
            }
            catch (DirectoryNotFoundException e) {
                Console.WriteLine("Directory or file does not exist: {0}", e.Message);
            }   
        }

        public static void PrintStatus(object objCompressor) {
            ICompressor compressor = (ICompressor) objCompressor;
            double status;
            bool hasBegun = false;

            while (Math.Abs((status = compressor.GetStatus()) - 1) > 0.01) {
                if (status > 0.0)
                    hasBegun = true;
                if (hasBegun && status == 0)
                    break;
                
                Console.Write("\rPercent: " + (status * 100).ToString("F1") + "%");
                Thread.Sleep(100);
            }
        }

        public class InvalidCompressorException : ArgumentException {
            public InvalidCompressorException() { }

            public InvalidCompressorException(string msg) : base(msg) { }
        }
    }
}