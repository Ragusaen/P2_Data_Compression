using System;
using System.Collections.Generic;
using Compression.ByteStructures;

namespace Compression {
    public class BitString {
        private List<byte> _bytes = new List<byte>();
        private int _bitIndex = 0;

        public BitString() {
            _bytes.Add(0);
        }

        public int Length {
            get { return _bytes.Count - (8 - _bitIndex); }
        }

        public void Append(UnevenByte ub) {
            while (ub.Length != 0) {
                if (ub.Length >= 8 - _bitIndex) {
                    _bytes[_bytes.Count - 1] += (byte) ub.GetBits(8 - _bitIndex);
                    ub -= 8 - _bitIndex;
                    _bitIndex = 0;
                    _bytes.Add(0);
                } else {
                    _bytes[_bytes.Count - 1] += (byte)(ub.GetBits(ub.Length) << (8 - _bitIndex - ub.Length));
                    _bitIndex += ub.Length;
                    ub -= ub.Length;
                }
            }
        }
        
        public byte[] ToArray() {
            if (_bitIndex == 0)
                _bytes.RemoveAt(_bytes.Count - 1);
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