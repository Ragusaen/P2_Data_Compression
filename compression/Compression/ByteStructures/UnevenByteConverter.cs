using System;
using System.Collections.Generic;
using Compression.ByteStructures;

namespace Compression.ByteStructures {
    public class UnevenByteConverter {

        public UnevenByte CreateUnevenByteFromBytes(ArrayIndexer<byte> array, int ubLength, int bitIndex) {
            // Calculate the amount of relevant bytes
            array.Length = (bitIndex + ubLength) / 8 + ((bitIndex + ubLength) % 8 == 0 ? 0 : 1);
            int remainingBits = ubLength;

            UnevenByte ub = default(UnevenByte);

            for (int i = 0; remainingBits > 0; ++i) {
                int bitsToAdd = remainingBits > 8 - bitIndex? 8 - bitIndex: remainingBits;
                remainingBits -= bitsToAdd;
                uint toAppend = (uint) ((array[i] % (1 << ( 8 - bitIndex))) >> (8 - bitsToAdd - bitIndex));
                ub += new UnevenByte(toAppend, bitsToAdd);
                bitIndex = (bitIndex + bitsToAdd) % 8;
            }
           
            return ub;
        }
        
        public byte[] UnevenBytesToBytes(List<UnevenByte> unevenBytes) {
            byte[] resultArray = new byte[ArrayByteCount(unevenBytes)];
            int resultIndex = 0;
            int bitIndex = 0;
            
            // ubai = unevenBytesArrayIndex 
            for (int ubai = 0; ubai < unevenBytes.Count; ubai++) {
                UnevenByte ub = unevenBytes[ubai];
                
                while (ub.Length != 0) {
                    if (ub.Length >= 8 - bitIndex) {
                        resultArray[resultIndex] += (byte) ub.GetBits(8 - bitIndex);
                        ub -= 8 - bitIndex;
                        bitIndex = 0;
                        resultIndex++;
                    } else {
                        resultArray[resultIndex] += (byte)(ub.GetBits(ub.Length) << (8 - bitIndex - ub.Length));
                        bitIndex += ub.Length;
                        ub -= ub.Length;
                    }
                }
            }

            return resultArray;
        }
        
        private int ArrayByteCount(List<UnevenByte> array) {
            int res = 0;
            for (int i = 0; i < array.Count; i++)
                res += array[i].Length;
            
            return res % 8 == 0 ? res / 8 : res / 8 + 1;
        }
    }
}