using System;
using System.Web;
using compression.AC_R;
using Compression.ByteStructures;
using Compression.Huffman;
using Compression.LZ;
using Compression.PPM;
using Xceed.Wpf.Toolkit.Core.Input;

namespace Compression {
    internal class Program {
        public static void Main(string[] args) {
            
            //testPPM();
            
            ICompressor compressor = new PredictionByPartialMatching();
            
            string path = "../../res/a";
            var watch = System.Diagnostics.Stopwatch.StartNew();
            
            // Load files
            DataFile input_file = new DataFile(path);
            DataFile compressed_file;

            compressed_file = compressor.Compress(input_file);

            var compression_time = watch.ElapsedMilliseconds;
            Console.WriteLine("Compressions time: " + compression_time + " ms");
            
            //DataFile restoredFile = compressor.Decompress(compressed_file);
            
            compressed_file.WriteToFile("../../res/out");
            //restoredFile.WriteToFile("../../res/restored");
            
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            Console.WriteLine("\nTime elapsed: " + elapsedMs + " ms");
            Console.WriteLine("\nSize before: " + input_file.Length + "\nSize after: " + compressed_file.Length + "\nCompressionratio: " + (double)compressed_file.Length/input_file.Length);

            double compressionspeed = input_file.Length / compression_time;
            double decompressionspeed = input_file.Length / (elapsedMs - compression_time);
            Console.WriteLine("\nCompression speed: " + compressionspeed + " kB/s\tDecompression speed: " + decompressionspeed + " kB/s");
        }

        public static void testHuffman() {
            var huffmanEncoder = new Huffman.HuffmanCompressor();
            byte[] inputBytes = ByteMethods.StringToByteArray("ABCDEFGH");
            DataFile input =  new DataFile(inputBytes);
            //                   FLLLS|      A   |S|      B   |LS    |   C  |    S|   D      |LLS|       E  |S|      F   |LS|      G   |S    |  H   |    |A||B||C    ||D||E||    F||G||H|
            byte[] expected = {0b10001010, 0b00001101, 0b00001001, 0b01000011, 0b10100010, 0b00010100, 0b01011010, 0b00110010, 0b10001111, 0b01001000, 0b00000101, 0b00111001, 0b01110111};

            DataFile compressed = huffmanEncoder.Compress(input);
            byte[] actual = huffmanEncoder.Decompress(compressed).GetAllBytes();
        }

        public static void testArithmetic() {
            ArithmeticCoder AC = new ArithmeticCoder();
            AC.Encode(5,9, 10);
            AC.Encode(5,9, 10);
            AC.Encode(5,9, 10);
            AC.Encode(1,10, 10);
            Console.WriteLine(AC.GetEncodedBitString());
        }

        public static void testPPM() {
            var ppm = new PredictionByPartialMatching();
            DataFile input = new DataFile(ByteMethods.StringToByteArray("abcdabcd"));
            
            DataFile output = ppm.Compress(input);
            output.WriteToFile("../../res/ppmcmpr");
        }


        public static void LZSpeedTests() {
            DataFile input = new DataFile("../../res/hcandersen.txt");
            byte[] bytes = input.GetAllBytes();

            double averageratio = 1;

            for (int startIndex = 0; startIndex < 10000; startIndex++) {
                var longWatch = System.Diagnostics.Stopwatch.StartNew();
                var longHistory = new ArrayIndexer<byte>(bytes, startIndex, 4096);
                var longLookAhead = new ArrayIndexer<byte>(bytes, startIndex+4096, 16);

                CFindMatchingBytes.FindLongestMatch(longHistory, longLookAhead);
                var longTime = longWatch.ElapsedTicks;

                var shortWatch = System.Diagnostics.Stopwatch.StartNew();
                var shortHistory = new ArrayIndexer<byte>(bytes, startIndex+3968, 128);
                var shortLookAhead = new ArrayIndexer<byte>(bytes, startIndex+4096, 4);

                CFindMatchingBytes.FindLongestMatch(shortHistory, shortLookAhead);
                var shortTime = shortWatch.ElapsedTicks;

                averageratio = (averageratio * startIndex + (double)longTime / shortTime) / (startIndex + 1);
            }

            Console.WriteLine($"Average ratio: {averageratio}");
        }
    }
}