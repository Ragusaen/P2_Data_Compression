using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Compression;
using Compression.Huffman;
using Compression.ByteStructures;

namespace UnitTesting.Huffman
{
    [TestFixture, Category("HuffmanEncodedTreeList")]
    class CreateListOf_EncodedInput {
        [Test]
        public void TreeMapOver_AB() {
            List<Node> input = new List<Node> {
                new LeafNode((byte)'A', 1),
                new LeafNode((byte)'B', 1)
            };

            List<UnevenByte> expected = new List<UnevenByte> {
                new UnevenByte(0b0, 1),
                new UnevenByte(0b101000001, 9),
                new UnevenByte(0b101000010, 9)
            };

            var actual = new HuffmanTree(input);

            Assert.AreEqual(expected, actual.EncodedTreeList);
        }
    }
}