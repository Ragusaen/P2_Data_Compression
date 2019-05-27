using System;

namespace Compression.LZ {
    /// <summary>
    ///     Simple class to contain both pointer- and raw encodings.
    /// </summary>
    public abstract class EncodedLZByte {
    }

    /// <summary>
    ///     The encoded byte that describes a pointer backwards in the file.
    /// </summary>
    public class PointerByte : EncodedLZByte, IEquatable<PointerByte> {
        public const int POINTER_SIZE = 12;
        public const int LENGTH_SIZE = 4;
        public int Length;

        public int Pointer;

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
            if (obj.GetType() != GetType()) return false;
            return Equals((PointerByte) obj);
        }

        public override int GetHashCode() {
            unchecked {
                return (Pointer * 397) ^ Length;
            }
        }

        #endregion
    }

    /// <summary>
    ///     The encoded byte that describes a raw data point.
    /// </summary>
    public class RawByte : EncodedLZByte {
        public const int RAW_SIZE = 8;

        public byte Data;

        public RawByte(byte data) {
            Data = data;
        }

        public override string ToString() {
            return "RawByte: " + (char) Data;
        }
    }
}