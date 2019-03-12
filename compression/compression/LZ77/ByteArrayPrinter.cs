using System;

namespace compression.LZ77 {
    public class ByteArrayPrinter {
        public static void PrintBits(byte[] a) {
            foreach (byte b in a) {
                Console.Write(Convert.ToString(b, 2).PadLeft(8,'0'));
            }
        }
    }
}