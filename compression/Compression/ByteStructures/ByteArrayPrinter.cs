using System;

namespace Compression.ByteStructures {
    public class ByteArrayPrinter {
        public static void PrintBits(byte[] a) {
            foreach (byte b in a) {
                Console.Write(Convert.ToString(b, 2).PadLeft(8,'0'));
            }
        }

        public static void PrintToString(byte[] a) {
            foreach (byte b in a) {
                Console.Write("{0}", (char)b);
            }
        }
    }
}