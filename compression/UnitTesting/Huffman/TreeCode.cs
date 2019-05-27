using System.Collections.Generic;
using Compression.ByteStructures;
using Compression.Huffman;
using NUnit.Framework;

namespace UnitTesting.Huffman {
    [TestFixture]
    [Category("HuffmanEncodedTreeList")]
    internal class CreateListOf_EncodedInput {
        [Test]
        public void TreeMapOver_AB() {
            var input = new List<Node> {
                new LeafNode((byte) 'A', 1),
                new LeafNode((byte) 'B', 1)
            };

            var expected = new List<UnevenByte> {
                new UnevenByte(0b0, 1),
                new UnevenByte(0b101000001, 9),
                new UnevenByte(0b101000010, 9)
            };

            var actual = new HuffmanTree(input);

            Assert.AreEqual(expected, actual.EncodedTreeList);
        }
    }
}