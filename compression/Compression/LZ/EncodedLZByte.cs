using System;
using System.Reflection;
using Compression.ByteStructures;

namespace Compression.LZ{

    public abstract class EncodedLZByte : EncodedByte {
        public sealed override EncodedByte UnevenByteToEncodedByte(UnevenByte unevenByte) {
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

        public override uint GetUnevenByteLength(byte firstByte) {
            return ((firstByte & 0x80) != 0)
                ? 1 + PointerByte.POINTER_SIZE + PointerByte.LENGTH_SIZE
                : 1 + RawByte.RAW_SIZE;
        }
    }
    
    public class PointerByte : EncodedLZByte {
        public const uint POINTER_SIZE = 12;
        public const uint LENGTH_SIZE = 4;

        public uint Pointer;
        public uint Length;
        
        public PointerByte(uint pointer, uint length) {
            Pointer = pointer;
            Length = length;
        }

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
        
        public override UnevenByte ToUnevenByte() {
            uint data = (1 << (int) POINTER_SIZE) + Pointer;
            data = (data << (int) LENGTH_SIZE) + Length;

            return new UnevenByte(data, 17);
        }
    }

    public class RawByte : EncodedLZByte {
        public const uint RAW_SIZE = 8;
        
        public byte Data;

        public RawByte(byte data) {
            Data = data;
        }

        public override UnevenByte ToUnevenByte() {
            return new UnevenByte(Data,9);
        }
    }
}