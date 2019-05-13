
using Compression.ByteStructures;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace compression {
    public class BitIndexer {
        private byte[] _bytes;
        private int currentIndex = 0;

        public BitIndexer(byte[] array) {
            _bytes = array;
        }

        public UnevenByte GetNext() {
            return this[currentIndex++];
        }

        public void GoToPrevious() {
            --currentIndex;
        }

        public bool AtEnd() {
            return currentIndex / 8 >= _bytes.Length;
        }

        public UnevenByte GetNextRange(int length) {
            var ub = GetRange(currentIndex, length);
            currentIndex += length;
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