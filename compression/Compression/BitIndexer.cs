using Compression.ByteStructures;

namespace Compression {
    /// <summary>
    ///     This class is an abstraction over reading a byte array bit by bit.
    /// </summary>
    public class BitIndexer {
        // The array the holds the bytes
        private readonly byte[] _bytes;
        // The current index in bits
        private int _currentIndex;

        public int Remaining => _bytes.Length * 8 - _currentIndex;

        public BitIndexer(byte[] array) {
            _bytes = array;
        }

        public UnevenByte this[int index] {
            get {
                var b = _bytes[index / 8];
                var bitIndex = index % 8;
                return new UnevenByte(((uint) b >> (7 - bitIndex)) & 1, 1);
            }
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

        public UnevenByte GetRange(int index, int length) {
            var ub = new UnevenByte(0, 0);

            for (var i = 0; i < length; ++i) ub += this[index + i];

            return ub;
        }
    }
}