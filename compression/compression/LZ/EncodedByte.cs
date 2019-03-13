using System.Security.Cryptography.X509Certificates;

namespace compression.LZ{
    public struct UnevenBits{
        public uint Data;
        public uint Length;

        public UnevenBits(uint data, uint length) {
            Data = data;
            Length = length;
        }

        public byte GetBits(uint count) {
            return (byte)((Data >> (int)(Length - count)) % (1 << (int)(Length + 1)));
        }
        public static int ArrayByteCount(UnevenBits[] array) {
            int res = 0;
            for (int i = 0; i < array.Length; i++)
                res += (int)array[i].Length;
            
            return res % 8 == 0 ? res / 8 : res / 8 + 1;
        }
    }
    
    public abstract class EncodedByte{
        public abstract UnevenBits ToUnevenBits();
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
        
        public override UnevenBits ToUnevenBits() {
            uint data = (1 << (int) POINTER_SIZE) + Pointer;
            data = (data << (int) LENGTH_SIZE) + Length;

            return new UnevenBits(data, 17);
        }
    }

    public class RawByte : EncodedByte {
        public byte Data;

        public RawByte(byte data) {
            Data = data;
        }

        public override UnevenBits ToUnevenBits() {
            return new UnevenBits(Data,9);
        }
    }
}