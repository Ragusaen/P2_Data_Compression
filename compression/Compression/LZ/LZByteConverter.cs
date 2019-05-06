using System;
using compression.ByteStructures;
using Compression.ByteStructures;

namespace Compression.LZ {
    public class LZByteConverter : IEncodedByteConverter<EncodedLZByte> {
        public EncodedLZByte ToEncodedByte(UnevenByte unevenByte) {
            if (unevenByte.GetBits(1) == 1) {
                unevenByte -= 1;
                int pointerData = unevenByte.GetBits(PointerByte.POINTER_SIZE);
                unevenByte -= PointerByte.POINTER_SIZE;
                int lengthData = unevenByte.GetBits(PointerByte.LENGTH_SIZE);
                return new PointerByte(pointerData + 1, lengthData + 1);
            }
            else {
                unevenByte -= 1;
                return new RawByte(unevenByte.GetBits(8));
            }
        }

        public UnevenByte ToUnevenByte(EncodedLZByte eb) {
            if (eb is PointerByte pb) {
                int data = (1 << PointerByte.POINTER_SIZE) + pb.Pointer;
                data = (data << PointerByte.LENGTH_SIZE) + pb.Length;

                return new UnevenByte((uint)data, 17);
            }
            if (eb is RawByte rb) {
                return new UnevenByte(rb.Data,9);
            }
            throw new ArgumentException("EncodedByte was not of valid type");
        }

        public int GetUnevenByteLength(byte firstByte) {
            return (int)(((firstByte & 0x80) != 0)
                ? 1 + PointerByte.POINTER_SIZE + PointerByte.LENGTH_SIZE
                : 1 + RawByte.RAW_SIZE);
        }
    }
}