using System;
using Compression.ByteStructures;
using Compression.Huffman;
using Compression.AC;
using Compression.LZ;
using Compression.PPM;

namespace Compression {
    internal class Program {
        public static void Main(string[] args) {
//            PPMCleanUpTest();
           SilesiaCompressionTests();
        }

        public static void PPMCleanUpTest() {
            
            DataFile dataFile = new DataFile("../../res/big2.txt");
            for (int limit = 1000000; limit >= 100; limit /= 10) {
                ICompressor compressor = new PredictionByPartialMatching(cleanUpLimit: limit);

                var timer = System.Diagnostics.Stopwatch.StartNew();
                DataFile compressedFile = compressor.Compress(dataFile);
                var compressionTime = timer.ElapsedMilliseconds;

                Console.WriteLine($"{limit}, {(double)compressedFile.Length/dataFile.Length}, {compressionTime}");
            }
        }

        public static void SilesiaCompressionTests() {
            ICompressor compressor = new LZSS();
            
            string[] paths = {/*
                "../../res/silesia.tar", */
                "../../res/silesia/data",
                "../../res/silesia/exe",
                "../../res/silesia/html",
                "../../res/silesia/img",
                "../../res/silesia/pdf",
                "../../res/silesia/src",
                "../../res/silesia/txt"
            };

            foreach (string path in paths) {
                DataFile dataFile = new DataFile(path);
                
                var compressionWatch = System.Diagnostics.Stopwatch.StartNew();
                var compressed = compressor.Compress(dataFile);
                var compressionTime = compressionWatch.ElapsedMilliseconds;

                long decompressionTime = 0;
                try {
                    var decompressionWatch = System.Diagnostics.Stopwatch.StartNew();
                    compressor.Decompress(compressed);
                    decompressionTime = decompressionWatch.ElapsedMilliseconds;
                    
                    Console.WriteLine($"{path}: R: {(double)compressed.Length / dataFile.Length}, C: {(double)dataFile.Length / compressionTime} kB/s, D: {(double)compressed.Length / decompressionTime}");
                }
                catch (NotImplementedException) {
                    Console.WriteLine(
                        $"{path}: R: {(double) compressed.Length / dataFile.Length}, C: {(double) dataFile.Length / compressionTime} kB/s");
                }
            }
        }

        public static void LZSpeedTests() {
            DataFile input = new DataFile("../../res/big2.txt");
            byte[] bytes = input.GetAllBytes();

            double averageratio = 1;

            for (int startIndex = 0; startIndex < 10000; startIndex++) {
                var longWatch = System.Diagnostics.Stopwatch.StartNew();
                var longHistory = new ByteArrayIndexer(bytes, startIndex, 4096);
                var longLookAhead = new ByteArrayIndexer(bytes, startIndex+4096, 16);

                CFindMatchingBytes.FindLongestMatch(longHistory, longLookAhead);
                var longTime = longWatch.ElapsedTicks;

                var shortWatch = System.Diagnostics.Stopwatch.StartNew();
                var shortHistory = new ByteArrayIndexer(bytes, startIndex+3968, 128);
                var shortLookAhead = new ByteArrayIndexer(bytes, startIndex+4096, 4);

                CFindMatchingBytes.FindLongestMatch(shortHistory, shortLookAhead);
                var shortTime = shortWatch.ElapsedTicks;

                averageratio = (averageratio * startIndex + (double)longTime / shortTime) / (startIndex + 1);
            }

            Console.WriteLine($"Average ratio: {averageratio}");
        }
    }
}