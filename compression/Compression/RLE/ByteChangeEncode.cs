using System.Collections.Generic;

namespace Compression.RLE {

    public class EncodedByteChanges {
        private List<byte> _dataStream;
        private List<byte> _changes;
        private byte _last;
        private uint _changes_index;

        public EncodedByteChanges() {
            _last = 0;
            _changes_index = 7;
            _dataStream = new List<byte>(0);
            _changes = new List<byte>(0);
            _changes.Add((byte)0);
        }

        public void AddEntry(byte b) {
            if (b != _last) {
                _dataStream.Add(b);
                _last = b;
                _changes[_changes.Count - 1] += (byte)(1 << (int)_changes_index);
            }

            --_changes_index;
            if (_changes_index > 7) {
                _changes_index = 7;
                _changes.Add((byte)0);
            }
        }
        
        public byte[] ToBytes() {
            byte[] bytes = new byte[GetLength()];
            _dataStream.CopyTo(bytes, 0);
            _changes.CopyTo(bytes, _dataStream.Count);
            return bytes;
        }

        private uint GetLength() {
            return (uint)(_dataStream.Count + _changes.Count);
        }
    }
    
    public class ByteChangeEncoder {
        
        public EncodedByteChanges EncodeBytes(byte[] input) {
            EncodedByteChanges ebc = new EncodedByteChanges();
            for (int i = 0; i < input.Length; ++i) {
                ebc.AddEntry(input[i]);
            }
            return ebc;
        }
    }
}