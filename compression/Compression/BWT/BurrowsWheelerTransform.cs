using System;
using System.Collections.Generic;
using compression.ByteStructures;

namespace Compression.BWT {
    
    /// <summary>
    /// This class performs the Burrows-Wheeler-Transform.
    /// </summary>
    public class BurrowsWheelerTransform {

        public byte[] Transform(byte[] input) {
            List<byte[]> transformMatrix = new List<byte[]>();

            for (int i = 0; i < input.Length; i++) {
                transformMatrix.Add(ByteMethods.shiftArray(input, i));
            }
            
            transformMatrix.Sort(new ByteArrayComparer());

            byte[] result = new byte[input.Length];
            for (int i = 0; i < input.Length; i++)
                result[i] = transformMatrix[i][input.Length - 1];
            
            return result;
        }

        public byte[] InverseTransform(byte[] input) {
            List<byte[]> itm = new List<byte[]>();

            for (int i = 0; i < input.Length; i++)
                itm.Add(new byte[input.Length]);

            for (int i = 0; i < itm.Count; i++) {
                for (int j = 0; j < input.Length; j++) {
                    itm[j] = ByteMethods.shiftArray(itm[j], 1);
                    itm[j][0] = input[j];
                }
                itm.Sort(new ByteArrayComparer());
            }
            
            for (int i = 0; i < itm.Count; i++)
                if (itm[i][0] == (byte)'^')
                    return itm[i];
            return null;
        }

        public static void PrintByteArrayList(IEnumerable<byte[]> l) {
            foreach (var t in l) {
                foreach (var t1 in t)
                    Console.Write(t1 + ",");

                Console.WriteLine();
            }
        }
    }
}