using System;
using compression.LZ;

namespace compression {
    internal class Program {
        public static void Main(string[] args) {

            string path = "../../res/random_data";
            
            LZ77 lz77 = new LZ77();
            DataFile compressed_file = lz77.Compress(new DataFile(path));
            
            compressed_file.WriteToFile("../../res/out");
        }
    }
}