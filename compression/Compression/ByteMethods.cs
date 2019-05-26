using System;

namespace Compression {
    
    /// <summary>
    /// Collection of methods on byte arrays, primarily used by non-essential code.
    /// </summary>
    public class ByteMethods {
        public static string ByteArrayToString(byte[] b) {
            string res = "";
            for (int i = 0; i < b.Length; i++)
                res += (char) b[i];
            return res;
        }

        public static string ByteToBinaryString(byte b) {
            return Convert.ToString(b, 2).PadLeft(8, '0');
        }

        public static byte BinaryStringToByte(string s) {
            int i = Convert.ToInt32(s, 2);
            return (byte)i;
        }

        public static byte[] BinaryStringToByteArray(string s) {
            byte[] res = new byte[s.Length / 8 + 1];
            s = s.PadLeft((s.Length / 8 + 1) * 8, '0');

            for(int i = 0; i < s.Length / 8; i++) {
                res[i] = ByteMethods.BinaryStringToByte(s.Substring(8 * i, 8));
            }
            return res;
        }

        public static byte[] StringToByteArray(string s) {
            byte[] res = new byte[s.Length];
            for (int i = 0; i < s.Length; i++)
                res[i] = (byte)s[i];
            return res;
        }
        
        public static byte[] shiftArray( byte[] input, int n) {
            byte[] result = new byte[input.Length];
            
            for (int i = 0; i < input.Length; i++) {
                result[congMod(i+n, input.Length)] = input[i];
            }

            return result;
        }
        
        public static int congMod(int n, int m) {
            return (n < 0) ? m - (-n % m) : n % m;
        }
    }
}