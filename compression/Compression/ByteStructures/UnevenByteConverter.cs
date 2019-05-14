using System;
using System.Collections.Generic;
using compression;
using Compression.ByteStructures;

namespace Compression.ByteStructures {
    public class UnevenByteConverter {
        
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
        
        public int ArrayByteCount(List<UnevenByte> array) {
            int res = 0;
            for (int i = 0; i < array.Count; i++)
                res += array[i].Length;
            
            return res % 8 == 0 ? res / 8 : res / 8 + 1;
        }
    }
}