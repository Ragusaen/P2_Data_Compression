using System;
using System.Collections.Generic;

namespace Compression.NBWT {
    public class BWT {

        public byte[] Transform(byte[] input) {
            List<byte[]> transformMatrix = new List<byte[]>();

            for (int i = 0; i < input.Length; i++) {
                transformMatrix.Add(shiftArray(input, i));
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
                    itm[j] = shiftArray(itm[j], 1);
                    itm[j][0] = input[j];
                }
                itm.Sort(new ByteArrayComparer());
            }
            
            for (int i = 0; i < itm.Count; i++)
                if (itm[i][0] == (byte)'^')
                    return itm[i];
            return null;
        }

        public static byte[] shiftArray( byte[] input, int n) {
            byte[] result = new byte[input.Length];
            
            for (int i = 0; i < input.Length; i++) {
                result[congMod(i+n, input.Length)] = input[i];
            }

            return result;
        }

        public static void PrintByteArrayList(List<byte[]> l) {
            for (int i = 0; i < l.Count; i++) {
                for (int j = 0; j < l[i].Length; j++)
                    Console.Write(l[i][j] + ",");
                Console.WriteLine();
            }
        }

        public static int congMod(int n, int m) {
            return (n < 0) ? m - (-n % m) : n % m;
        }

        public static string ByteArrayToString(byte[] b) {
            string res = "";
            for (int i = 0; i < b.Length; i++)
                res += (char) b[i];
            return res;
        }

        public static byte[] StringToByteArray(string s) {
            byte[] res = new byte[s.Length];
            for (int i = 0; i < s.Length; i++)
                res[i] = (byte)s[i];
            return res;
        }
        
    }
}