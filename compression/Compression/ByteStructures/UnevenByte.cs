namespace Compression.ByteStructures{
    public struct UnevenByte{
        public uint Data;
        public uint Length;

        public UnevenByte(uint data, uint length) {
            Data = data;
            Length = length;
        }

        public byte GetBits(uint count) {
            return (byte)((Data >> (int)(Length - count)) % (1 << (int)(Length + 1)));
        }
        public static int ArrayByteCount(UnevenByte[] array) {
            int res = 0;
            for (int i = 0; i < array.Length; i++)
                res += (int)array[i].Length;
            
            return res % 8 == 0 ? res / 8 : res / 8 + 1;
        }
        
        public static byte[] UnEvenBytesToBytes(UnevenByte[] unevenByteArray) {
            byte[] resultArray = new byte[UnevenByte.ArrayByteCount(unevenByteArray)];
            uint resultIndex = 0;
            uint bitIndex = 0;
            
            
            // ubai = unevenBitsArrayIndex 
            for (int ubai = 0; ubai < unevenByteArray.Length; ubai++) {
                UnevenByte ub = unevenByteArray[ubai];
                
                while (ub.Length != 0) {
                    if (ub.Length >= 8 - bitIndex) {
                        resultArray[resultIndex] += ub.GetBits(8 - bitIndex);
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