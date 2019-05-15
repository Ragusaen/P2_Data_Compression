using System;
using System.Collections.Generic;
using System.Linq;
using Compression.ByteStructures;

namespace compression {
    public class BitString {
        private List<byte> _bytes = new List<byte>();
        private int _bitIndex = 0;

        public BitString() {
            _bytes.Add(0);
        }

        public void Append(UnevenByte ub) {
            if (ub.Length != 1) {
                throw new ArgumentException("You can only add UnevenByte of length 1 to bitstring");
            }
            
            if (ub == UnevenByte.One) {
                _bytes[_bytes.Count - 1] += (byte) (1 << (7 - _bitIndex));
            }

            IncrementBitIndex();
        }

        private void IncrementBitIndex() {
            _bitIndex++;
            if (_bitIndex >= 8) {
                _bitIndex = 0;
                _bytes.Add(0);
            }
        }

        public byte[] ToArray() {
            return _bytes.ToArray();
        }

        public override string ToString() {
            string output = "";
            for (int i = 0; i < _bytes.Count - 1; ++i) {
                output += Convert.ToString(_bytes[i], 2).PadLeft(8, '0');
            }

            byte b = (byte) (_bytes[_bytes.Count - 1] >> (8 - _bitIndex));
            output += Convert.ToString(b, 2).PadLeft(_bitIndex, '0');

            return output;
        }
    }
}