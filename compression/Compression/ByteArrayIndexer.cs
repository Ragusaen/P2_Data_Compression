namespace Compression.ByteStructures {
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