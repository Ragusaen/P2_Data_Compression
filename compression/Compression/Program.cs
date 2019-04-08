using System;
using Compression.BWT;
using Compression.LZ;
using Compression.RLE;
using StackExchange.Profiling;

namespace Compression {
    internal class Program {
        public static void Main(string[] args) {
            
            string path = "../../res/the_egg";
            var watch = System.Diagnostics.Stopwatch.StartNew();

            LZ77 lz77 = new LZ77();
            DataFile input_file = new DataFile(path);
            DataFile compressed_file = new DataFile();
            
            BurrowWheelerTransform bwt = new BurrowWheelerTransform();

            byte[] data = input_file.GetBytes(0, input_file.Length);

            compressed_file = lz77.Compress(input_file);
            
            compressed_file.WriteToFile("../../res/out");
            
            DataFile restoredFile = lz77.Decompress(compressed_file);
            restoredFile.WriteToFile("../../res/restored");
            
            //recreated_file.WriteToFile("../../res/rec");
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            Console.WriteLine("\nTime elapsed: " + elapsedMs + " ms");
            Console.WriteLine("\nStørrelse før: " + input_file.Length + "\nStørrelse efter: " + compressed_file.Length + "\nKomprimeringsratio: " + (double)compressed_file.Length/input_file.Length);
        }
    }
}