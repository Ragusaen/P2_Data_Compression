using System;
using Compression.ByteStructures;

namespace Compression.LZ {
    /// <summary>
    /// This class converts UnevenBytes to EncodedLZBytes and vice versa.
    /// </summary>
    public class LZByteConverter {
        /// <summary>
        /// Converts an UnevenByte to EncodedLZByte based on the control bit-
        /// </summary>
        /// <param name="unevenByte"> UnevenByte to convert. </param>
        public EncodedLZByte ToEncodedByte(UnevenByte unevenByte) {
            // Check if control bit is 1
            if (unevenByte[0] == 1) {
                unevenByte -= 1;
                int pointerData = unevenByte.GetBits(PointerByte.POINTER_SIZE);
                unevenByte -= PointerByte.POINTER_SIZE;
                int lengthData = unevenByte.GetBits(PointerByte.LENGTH_SIZE);
                return new PointerByte(pointerData + 1, lengthData + 1);
            }
            else {
                unevenByte -= 1;
                return new RawByte((byte)unevenByte.Data);
            }
        }

        public UnevenByte ToUnevenByte(EncodedLZByte eb) {
            if (eb is PointerByte pb) {
                int data = (1 << PointerByte.POINTER_SIZE) + pb.Pointer;
                data = (data << PointerByte.LENGTH_SIZE) + pb.Length;

                return new UnevenByte((uint)data, PointerByte.POINTER_SIZE + PointerByte.LENGTH_SIZE + 1);
            } else {
                return new UnevenByte(((RawByte)eb).Data, RawByte.RAW_SIZE + 1);
            }
        }

        public int GetUnevenByteLength(UnevenByte controlBit) {
            return (int)(controlBit == UnevenByte.One
                ? 1 + PointerByte.POINTER_SIZE + PointerByte.LENGTH_SIZE
                : 1 + RawByte.RAW_SIZE);
        }
    }
}