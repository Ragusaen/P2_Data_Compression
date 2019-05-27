namespace Compression.ByteStructures {
    /// <summary>
    ///     A class used for efficient index of arrays, this is used as an alternative to copying arrays.
    /// </summary>
    public struct ByteArrayIndexer {
        public byte[] Array;
        public int Index;
        public int Length;

        public byte this[int index] => Array[Index + index];

        public ByteArrayIndexer(byte[] array, int index, int length) {
            Array = array;
            Index = index;
            Length = length;
        }
    }
}