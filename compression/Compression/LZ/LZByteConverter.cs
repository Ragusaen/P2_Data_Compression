using System;
using compression.ByteStructures;
using Compression.ByteStructures;

namespace Compression.LZ {
    public class LZByteConverter : IEncodedByteConverter<EncodedLZByte> {
        public EncodedLZByte ToEncodedByte(UnevenByte unevenByte) {
            if (unevenByte.GetBits(1) == 1) {
                unevenByte.Length--;
                uint pointerData = unevenByte.GetBits(PointerByte.POINTER_SIZE);
                unevenByte.Length -= PointerByte.POINTER_SIZE;
                uint lengthData = unevenByte.GetBits(PointerByte.LENGTH_SIZE);
                return new PointerByte(pointerData + 1, lengthData + 1);
            }
            else {
                unevenByte.Length--;
                return new RawByte(unevenByte.GetBits(8));
            }
        }

        public UnevenByte ToUnevenByte(EncodedLZByte eb) {
            if (eb is PointerByte pb) {
                uint data = (1 << (int) PointerByte.POINTER_SIZE) + pb.Pointer;
                data = (data << (int) PointerByte.LENGTH_SIZE) + pb.Length;

                return new UnevenByte(data, 17);
            }
            if (eb is RawByte rb) {
                return new UnevenByte(rb.Data,9);
            }
            throw new ArgumentException("EncodedByte was not of valid type");
        }

        public uint GetUnevenByteLength(byte firstByte) {
            return ((firstByte & 0x80) != 0)
                ? 1 + PointerByte.POINTER_SIZE + PointerByte.LENGTH_SIZE
                : 1 + RawByte.RAW_SIZE;
        }
    }
}