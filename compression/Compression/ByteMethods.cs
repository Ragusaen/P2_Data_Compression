namespace Compression {
    public class ByteMethods {
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