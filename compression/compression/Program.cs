using System;
using System.IO;
using System.Net;
using System.Xml.Serialization;
using compression.LZ;
using compression.NBWT;
using compression.RLE;
using StackExchange.Profiling;

namespace compression {
    internal class Program {
        public static void Main(string[] args) {
            
            string path = "../../res/hcandersen";
            var watch = System.Diagnostics.Stopwatch.StartNew();

            LZ77 lz77 = new LZ77();
            DataFile input_file = new DataFile(path);
            DataFile compressed_file = new DataFile();
            DataFile recreated_file = new DataFile();

            byte[] data = input_file.GetBytes(0, input_file.Length);

            Console.WriteLine("ARL before: " + AverageRunLength(data));
            
            var profiler = MiniProfiler.StartNew("Full Program");
            using (profiler.Step("Main Work")) {
                //compressed_file = lz77.Compress(input_file);
                byte[] output = BWT.Transform(data);
                //input_file.LoadBytes(output);
                output = ByteChangeEncoder.EncodeBytes(output).ToBytes();
                compressed_file.LoadBytes(output);

                //Console.WriteLine("ARL after: " + AverageRunLength(output));
            }
            
            compressed_file.WriteToFile("../../res/out");
            //recreated_file.WriteToFile("../../res/rec");
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            Console.WriteLine("\nTime elapsed: " + elapsedMs + " ms");
            Console.WriteLine("\nStørrelse før: " + input_file.Length + "\nStørrelse efter: " + compressed_file.Length + "\nKomprimeringsratio: " + (double)compressed_file.Length/input_file.Length);
        }

        public static double AverageRunLength(byte[] a) {
            int runs = 0;

            byte c = a[0];
            for (int i = 1; i < a.Length; i++) {
                if (a[i] != a[i - 1])
                    runs++;
            }

            return (double) a.Length / runs;
        }
    }
}