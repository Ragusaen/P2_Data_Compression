using System;
using System.Linq;
using PDC;

namespace Compression.ByteStructures{
    public struct UnevenByte{
        public uint Data;
        public uint Length;
        
        public UnevenByte(uint data, uint length) {
            Data = data;
            Length = length;
        }

        public UnevenByte(byte[] input, uint bitStartIndex, uint bitLength) {
            Length = bitLength;
            Data = (uint)(input[0] % (1 << (int)(8 - bitStartIndex)));

            if (input.Length > 1) {
                for (int i = 1; i < input.Length - 1; ++i) {
                    Data <<= 8;
                    Data += input[i];
                }

                uint bitsInLastByte = (bitLength - (8 - bitStartIndex)) % 8;
                if (bitsInLastByte == 0)
                    bitsInLastByte = 8;

                Data <<= (int) bitsInLastByte;
                Data += (uint) (input.Last() >> (int) (8 - bitsInLastByte));
            }
        }

        public byte GetBits(uint count) {
            return (byte)((Data >> (int)(Length - count)) % (1 << (int)Length));
        }
        
        public static int ArrayByteCount(UnevenByte[] array) {
            int res = 0;
            for (int i = 0; i < array.Length; i++)
                res += (int)array[i].Length;
            
            return res % 8 == 0 ? res / 8 : res / 8 + 1;
        }
        
        public static byte[] UnEvenBytesToBytes(UnevenByte[] unevenBytes) {
            byte[] resultArray = new byte[UnevenByte.ArrayByteCount(unevenBytes)];
            uint resultIndex = 0;
            uint bitIndex = 0;
            
            
            // ubai = unevenBitsArrayIndex 
            for (int ubai = 0; ubai < unevenBytes.Length; ubai++) {
                UnevenByte ub = unevenBytes[ubai];
                
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

        public override string ToString() {
            uint d = (uint) (Data % (1 << (int) (Length)));
            string s = Convert.ToString(d, 2);
            while (s.Length < Length)
                s = "0" + s;
            return s;
        }
    }
}