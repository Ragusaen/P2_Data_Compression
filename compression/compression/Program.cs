using System;
using compression.LZ;

namespace compression {
    internal class Program {
        public static void Main(string[] args) {
            string path = "../../res/hcandersen";
            var watch = System.Diagnostics.Stopwatch.StartNew();

            LZ77 lz77 = new LZ77();
            DataFile input_file = new DataFile(path);
            DataFile compressed_file = lz77.Compress(input_file);
            
            compressed_file.WriteToFile("../../res/out");
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            Console.WriteLine("\nTime elapsed" + elapsedMs / 1000 + "s");
            Console.WriteLine("\nStørrelse før: " + input_file.Length + "\nStørrelse efter: " + compressed_file.Length + "\nKomprimeringsratio: " + (double)compressed_file.Length/input_file.Length);
        }
    }
}