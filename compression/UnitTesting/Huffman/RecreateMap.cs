using System.Collections.Generic;
using Compression.ByteStructures;
using Compression.Huffman;
using NUnit.Framework;

namespace UnitTesting.Huffman {
    [TestFixture]
    [Category("HuffmanCreateEncodingDictionaryTests")]
    internal class CreateEncodingDictionary {
        [Test]
        public void CodeDictionary_AB() {
            var input = new List<Node> {
                new LeafNode((byte) 'A', 1),
                new LeafNode((byte) 'B', 1)
            };

            var expected = new Dictionary<byte, UnevenByte> {
                {65, new UnevenByte(0b0, 1)},
                {66, new UnevenByte(0b1, 1)}
            };

            var actual = new HuffmanTree(input);

            CollectionAssert.AreEqual(expected, actual.CodeDictionary);
        }
    }
}