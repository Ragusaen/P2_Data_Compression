using System;
using System.Collections.Generic;
using Compression.ByteStructures;

namespace Compression {
    /// <summary>
    ///     This class is an abstraction over a byte-list, such that it acts like a bitstring, without
    ///     using as much memory as having an actual list of "bits".
    /// </summary>
    public class BitString {
        // The index of the next bit in the last byte of the list. This is used to insert new bits
        // at the correct location
        private int _bitIndex;

        // The list that holds all the data
        private readonly List<byte> _bytes = new List<byte>();
        public BitString() {
            // There should always be a non-full byte at the end of the list.
            _bytes.Add(0);
        }

        public int Length => _bytes.Count - (8 - _bitIndex);

        /// <summary>
        ///     This method appends any UnevenByte to the bitstring.
        /// </summary>
        /// <param name="ub">The UnevenByte to append</param>
        public void Append(UnevenByte ub) {
            // Keep splitting the UnevenByte until it is empty
            while (ub.Length != 0) // If the UnevenByte has more bits left, than there is bits left in the current byte
                if (ub.Length >= 8 - _bitIndex) {
                    // Add the next 8-_bitIndex bits to the byte to fill it
                    _bytes[_bytes.Count - 1] += (byte) ub.GetBits(8 - _bitIndex);
                    ub -= 8 - _bitIndex; // Reduce the length of the UnevenByte

                    // Since the byte is now full, set bitIndex to 0 and add an empty byte.
                    _bitIndex = 0;
                    _bytes.Add(0);
                }
                else {
                    // else if the rest of the UnevenByte cannot fit in the last byte
                    // Add the remaining part
                    _bytes[_bytes.Count - 1] += (byte) (ub.GetBits(ub.Length) << (8 - _bitIndex - ub.Length));
                    _bitIndex += ub.Length; // Update the bitIndex
                    ub = default(UnevenByte); // Some the rest of the UnevenByte was used, it must now be empty
                }
        }


        public byte[] ToArray() {
            // Remove the last byte if it is empty
            if (_bitIndex == 0)
                _bytes.RemoveAt(_bytes.Count - 1);
            return _bytes.ToArray();
        }

        public override string ToString() {
            var output = "";
            for (var i = 0; i < _bytes.Count - 1; ++i) output += Convert.ToString(_bytes[i], 2).PadLeft(8, '0');

            var b = (byte) (_bytes[_bytes.Count - 1] >> (8 - _bitIndex));
            output += Convert.ToString(b, 2).PadLeft(_bitIndex, '0');

            return output;
        }
    }
}