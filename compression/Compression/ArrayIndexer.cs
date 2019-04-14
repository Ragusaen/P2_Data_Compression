using System.Dynamic;

namespace compression.ByteStructures {
    public struct ArrayIndexer<T> {
        public T[] Array;
        public int Index;
        public int Length;
        
        public T this[int index] => Array[Index + index];

        public ArrayIndexer(T[] array, int index, int length) {
            Array = array;
            Index = index;
            Length = length;
        }
    }
}