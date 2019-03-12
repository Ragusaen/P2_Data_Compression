namespace compression.LZ77{
    public abstract class EncodedByte {
        
    }

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
            uint result = 1;

            for (int i = 0; i < LENGTH_SIZE; i++) {
                result *= 2;
            }

            return result;
        }

        public PointerByte(uint pointer, uint length) {
            Pointer = pointer;
            Length = length;
        }
    }

    public class RawByte : EncodedByte {
        public byte Data;

        public RawByte(byte data) {
            Data = data;
        }
    }
}