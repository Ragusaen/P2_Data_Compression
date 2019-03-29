using Compression.ByteStructures;

namespace Compression.LZ{
    public class PointerByte : EncodedByte {
        public const uint POINTER_SIZE = 12;
        public const uint LENGTH_SIZE = 4;

        public uint Pointer;
        public uint Length;

        public static uint GetPointerSpan() {
            uint result = 1;

            for (int i = 0; i < POINTER_SIZE; i++) {
                result *= 2;
            }

            return result;
        }
        
        public static uint GetLengthSpan() {
            return 1 << (int)POINTER_SIZE;
        }

        public PointerByte(uint pointer, uint length) {
            Pointer = pointer;
            Length = length;
        }
        
        public override UnevenByte ToUnevenBits() {
            uint data = (1 << (int) POINTER_SIZE) + Pointer;
            data = (data << (int) LENGTH_SIZE) + Length;

            return new UnevenByte(data, 17);
        }
    }

    public class RawByte : EncodedByte {
        public byte Data;

        public RawByte(byte data) {
            Data = data;
        }

        public override UnevenByte ToUnevenBits() {
            return new UnevenByte(Data,9);
        }
    }
}