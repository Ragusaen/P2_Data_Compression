using Compression.RLE;

namespace Compression.PPM{
    public class ByteToISymbol{
        public static ISymbol ConvertSingle(byte Byte) {
            return new Letter(Byte);
        }

        public static ISymbol[] ConvertArray(byte[] byteArray) {
            int len = byteArray.Length;
            
            if (len > 0) {
                ISymbol[] res = new ISymbol[len];

                for (int i = 0; i < len; i++) {
                    res[i] = new Letter(byteArray[i]);
                }
            }

            return new ISymbol[0];
        }
    }
}