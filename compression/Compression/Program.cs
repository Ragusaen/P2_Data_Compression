using System;
using System.IO;
using System.Linq;
using Compression.Huffman;
using Compression.LZ;
using Compression.PPM;

namespace Compression {
    internal class Program {
        public static void Main(string[] args) {
            if (args.Length < 1) {
                Console.WriteLine("Usage: <input path> [-o <output path>] [-C <compressor (lz, ppm, huffman)>]");
                return;
            }

            DataFile input;
            string outputPath;
            ICompressor compressor = new LZSS();
            var fileExtension = "lz";
            try {
                var inputPath = args[0];
                input = new DataFile(inputPath);

                if (args.Contains("-A")) {
                    var i = Array.IndexOf(args, "-A") + 1;
                    switch (args[i]) {
                        case "lz":
                            compressor = new LZSS();
                            fileExtension = "lz";
                            break;
                        case "ppm":
                            compressor = new PredictionByPartialMatching();
                            fileExtension = "lz";
                            break;
                        case "huffman":
                            compressor = new HuffmanCompressor();
                            break;
                        default:
                            throw new InvalidCompressorException();
                    }
                }

                var compress = !args.Contains("-D");

                outputPath = Path.GetDirectoryName(inputPath) + "/compressed." + fileExtension;
                if (args.Contains("-o")) {
                    var i = Array.IndexOf(args, "-o") + 1;
                    outputPath = args[i];
                }

                Console.WriteLine("{0}ompressing file with algorithm {1}.", compress ? "C" : "Dec", fileExtension);
                DataFile output;
                if (compress)
                    output = compressor.Compress(input);
                else
                    output = compressor.Decompress(input);

                output.WriteToFile(outputPath);
                Console.WriteLine("File written to {0}", outputPath);
            }
            catch (FileNotFoundException) {
                Console.WriteLine("File was not find: {0}");
            }
            catch (InvalidCompressorException) {
                Console.WriteLine("The compression algorithm specified does not exist");
            }
            catch (DirectoryNotFoundException e) {
                Console.WriteLine("Directory or file does not exist: {0}", e.Message);
            }
        }

        public class InvalidCompressorException : ArgumentException {
            public InvalidCompressorException() {
            }

            public InvalidCompressorException(string msg) : base(msg) {
            }
        }
    }
}