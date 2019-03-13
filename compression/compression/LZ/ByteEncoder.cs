using System.Net;

namespace compression.LZ{
    public class ByteEncoder{
        public static byte[] EncodeBytes(UnevenBits[] unevenBitsArray) {
            byte[] resultArray = new byte[UnevenBits.ArrayByteCount(unevenBitsArray)];
            uint resultIndex = 0;
            uint bitIndex = 0;
            
            
            // ubai = unevenBitsArrayIndex 
            for (int ubai = 0; ubai < unevenBitsArray.Length; ubai++) {
                UnevenBits ub = unevenBitsArray[ubai];
                
                while (ub.Length != 0) {
                    if (ub.Length >= 8 - bitIndex) {
                        resultArray[resultIndex] += (byte) ub.GetBits(8 - bitIndex);
                        ub.Length -= 8 - bitIndex;
                        bitIndex = 0;
                        resultIndex++;
                    }
                    else {
                        resultArray[resultIndex] += (byte)(ub.GetBits(ub.Length) << (int)(8 - bitIndex - ub.Length));
                        bitIndex += ub.Length;
                        ub.Length = 0;
                    }
                }
            }

            return resultArray;
        }
    }
}