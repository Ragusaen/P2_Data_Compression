using System;
using Compression.ByteStructures;
using Compression.Huffman;
using Compression.AC_R;
using Compression.LZ;
using Compression.PPM;

namespace Compression {
    internal class Program {
        public static void Main(string[] args) {
            ICompressor compressor = new HuffmanCompressor();
            
            string[] paths = {
                "../../res/silesia.tar",
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

                var decompressionWatch = System.Diagnostics.Stopwatch.StartNew();
                compressor.Decompress(compressed);
                var decompressionTime = decompressionWatch.ElapsedMilliseconds;

                Console.WriteLine($"{path}: R: {(double)compressed.Length / dataFile.Length}, C: {(double)dataFile.Length / compressionTime} kB/s, D: {(double)compressed.Length / decompressionTime}");
            }
            
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