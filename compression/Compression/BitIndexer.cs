
using Compression.ByteStructures;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace compression {
    public class BitIndexer {
        private byte[] _bytes;
        private int _currentIndex = 0;

        public int Remaining {
            get { return _bytes.Length * 8 - _currentIndex; }
        }

        public BitIndexer(byte[] array) {
            _bytes = array;
        }

        public UnevenByte GetNext() {
            return this[_currentIndex++];
        }

        public void GoToPrevious() {
            --_currentIndex;
        }

        public bool AtEnd() {
            return _currentIndex / 8 >= _bytes.Length;
        }

        public UnevenByte GetNextRange(int length) {
            var ub = GetRange(_currentIndex, length);
            _currentIndex += length;
            return ub;
        }
        
        public UnevenByte this[int index] {
            get {
                byte b = _bytes[index / 8];
                int bitIndex = index % 8;
                return new UnevenByte(((uint)b >> (7 - bitIndex)) & 1, 1);
            }
        }

        public UnevenByte GetRange(int index, int length) {
            UnevenByte ub = new UnevenByte(0,0);
            
            for (int i = 0; i < length; ++i) {
                ub += this[index + i];
            }
            
            return ub;
        }
    }
}