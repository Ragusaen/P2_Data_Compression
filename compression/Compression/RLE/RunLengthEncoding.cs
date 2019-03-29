namespace Compression.RLE {
    public class RunLengthEncoding {
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