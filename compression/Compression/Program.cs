using System;
using Compression.BWT;
using Compression.LZ;
using Compression.RLE;

namespace Compression {
    internal class Program {
        public static void Main(string[] args) {
            
            string path = "../../res/big2.txt";
            var watch = System.Diagnostics.Stopwatch.StartNew();

            LZ77 lz77 = new LZ77();
            DataFile input_file = new DataFile(path);
            DataFile compressed_file = new DataFile();

            byte[] data = input_file.GetBytes(0, input_file.Length);

            compressed_file = lz77.Compress(input_file);

            var compression_time = watch.ElapsedMilliseconds;
            Console.WriteLine("Compressions time: " + compression_time + " ms");
            
            DataFile restoredFile = lz77.Decompress(compressed_file);
            
            compressed_file.WriteToFile("../../res/out");
            restoredFile.WriteToFile("../../res/restored");
            
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            Console.WriteLine("\nTime elapsed: " + elapsedMs + " ms");
            Console.WriteLine("\nSize before: " + input_file.Length + "\nSize after: " + compressed_file.Length + "\nCompressionratio: " + (double)compressed_file.Length/input_file.Length);

            double compressionspeed = input_file.Length / compression_time;
            double decompressionspeed = input_file.Length / (elapsedMs - compression_time);
            Console.WriteLine("\nCompression speed: " + compressionspeed + " kB/s\tDecompression speed: " + decompressionspeed + " kB/s");
           
        }
    }
}