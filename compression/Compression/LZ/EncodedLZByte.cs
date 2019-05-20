using System;
using Compression.ByteStructures;

namespace Compression.LZ{

    public abstract class EncodedLZByte : EncodedByte { }

    public class PointerByte : EncodedLZByte, IEquatable<PointerByte> {
        public const int POINTER_SIZE = 12;
        public const int LENGTH_SIZE = 4;

        public int Pointer;
        public int Length;
        
        public PointerByte(int pointer, int length) {
            Pointer = pointer;
            Length = length;
        }

        public static int GetPointerSpan() {
            return 1 << POINTER_SIZE;
        }
        
        public static int GetLengthSpan() {
            return 1 << LENGTH_SIZE;
        }

        public override string ToString() {
            return "Pointer: (" + Pointer + ", " + Length + ")";
        }

        #region IEquatable
        public bool Equals(PointerByte other) {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Pointer == other.Pointer && Length == other.Length;
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((PointerByte) obj);
        }

        public override int GetHashCode() {
            unchecked {
                return ((int) Pointer * 397) ^ (int) Length;
            }
        }
        #endregion
    }

    public class RawByte : EncodedLZByte {
        public const uint RAW_SIZE = 8;
        
        public byte Data;

        public RawByte(byte data) {
            Data = data;
        }

        public override string ToString() {
            return "RawByte: " + (char)Data;
        }
    }
}