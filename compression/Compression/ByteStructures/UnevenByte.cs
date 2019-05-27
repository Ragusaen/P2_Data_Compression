using System;

namespace Compression.ByteStructures {
    /// <summary>
    ///     The UnevenByte is an abstraction over a string of bits which does not have a length of 8. An UnevenByte
    ///     can have any length. The length describes the amount of LSB of Data which are relevant. The struct has
    ///     implemented several methods to manipulate the data conveniently.
    /// </summary>
    public struct UnevenByte : IEquatable<UnevenByte> {
        public readonly uint Data;
        public readonly int Length;

        public UnevenByte(uint data, int length) {
            Data = data;
            Length = length;
        }

        /// <summary>
        ///     Get the bit at a given index, 0 is MSB.
        /// </summary>
        /// <param name="index"> Index of desired bit. </param>
        public int this[int index] => ((int) Data >> (Length - index - 1)) % 2;

        /// <summary>
        ///     Concatenates 2 UnevenBytes.
        /// </summary>
        /// <param name="a"> First UnevenByte. </param>
        /// <param name="b"> Second UnevenByte. </param>
        /// <returns> New instance of UnevenByte that is the concatenation of the 2 inputs. </returns>
        public static UnevenByte operator +(UnevenByte a, UnevenByte b) {
            return new UnevenByte((a.Data << b.Length) + b.Data, a.Length + b.Length);
        }

        /// <summary>
        ///     Removes l of the most significant bits.
        /// </summary>
        /// <param name="ub"> The UnevenByte to remove from. </param>
        /// <param name="l"> Amount of bits to remove. </param>
        /// <returns></returns>
        public static UnevenByte operator -(UnevenByte ub, int l) {
            return new UnevenByte((uint) (ub.Data % (1 << (ub.Length - l))), ub.Length - l);
        }

        public static bool operator ==(UnevenByte a, UnevenByte b) {
            return a.Equals(b);
        }

        public static bool operator !=(UnevenByte a, UnevenByte b) {
            return !a.Equals(b);
        }

        /// <summary>
        ///     Returns the complement of the UnevenByte, which means every bit in the UnevenByte is flipped.
        /// </summary>
        /// <param name="ub"> UnevenByte to flip</param>
        /// <returns></returns>
        public static UnevenByte operator !(UnevenByte ub) {
            return new UnevenByte((uint) (~ub.Data % (1 << ub.Length)), ub.Length);
        }

        // Two static values for convenience. This is fine because the UnevenByte is immutable.
        public static readonly UnevenByte One = new UnevenByte(1, 1);
        public static readonly UnevenByte Zero = new UnevenByte(0, 1);

        /// <summary>
        ///     Return an amount of bits, starting from MSB.
        /// </summary>
        /// <param name="count"> Amount of bits to get. </param>
        /// <returns> Count bits of UnevenByte starting from MSB. </returns>
        public int GetBits(int count) {
            return (int) (Data >> (Length - count)) % (1 << count);
        }

        public override string ToString() {
            var d = (uint) (Data % (1 << Length));
            var s = Convert.ToString(d, 2);
            while (s.Length < Length)
                s = "0" + s;
            return s;
        }

        public bool Equals(UnevenByte other) {
            return Data == other.Data && Length == other.Length;
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            return obj is UnevenByte other && Equals(other);
        }

        public override int GetHashCode() {
            unchecked {
                return ((int) Data * 397) ^ Length;
            }
        }
    }
}