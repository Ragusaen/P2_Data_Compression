using System;

namespace Compression.Huffman {
    public class OnlyOneUniqueByteException : ArgumentException {
        public OnlyOneUniqueByteException() {
        }

        public OnlyOneUniqueByteException(string message) : base(message) {
        }
    }
}