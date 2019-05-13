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
            List<Node> input = new List<Node>
            {
                new LeafNode((byte)'A'),
                new LeafNode((byte)'B')
            };

            List<UnevenByte> expected = new List<UnevenByte> {
                new UnevenByte(0b0, 1),
                new UnevenByte(0b101000001, 9),
                new UnevenByte(0b101000010, 9)
            };

            var actual = new HuffmanTree(input);

            Assert.AreEqual(expected, actual.EncodedTreeList);
        }

        [Test]
        public void TreeMapOver_testfile2() {
            byte[] input = ByteMethods.StringToByteArray("fem flade flxdeboller py et fladt flxdebollefad"); //x = ø og y = å

            Compression.Huffman.HuffmanCompressor ListOfNodes = new Compression.Huffman.HuffmanCompressor();
            List<Node> NodeListOfInput = ListOfNodes.CreateLeafNodes(input);

            List<UnevenByte> expected = new List<UnevenByte> {
                new UnevenByte(0b0,1), new UnevenByte(0b0,1), new UnevenByte(0b0,1), new UnevenByte(0b0,1), new UnevenByte(0b0,1),
                new UnevenByte(0b101101101, 9), // m
                new UnevenByte(0b101110000, 9), // p
                new UnevenByte(0b101110100, 9), // t
                new UnevenByte(0b0, 1),
                new UnevenByte(0b101111000, 9), // x 
                new UnevenByte(0b0, 1),
                new UnevenByte(0b101110010, 9), // r
                new UnevenByte(0b101111001, 9), // y
                new UnevenByte(0b0, 1),
                new UnevenByte(0b101100100, 9), // d 
                new UnevenByte(0b100100000, 9), // (space)
                new UnevenByte(0b0, 1), new UnevenByte(0b0, 1),
                new UnevenByte(0b101100110, 9), // f
                new UnevenByte(0b101100101, 9), // e
                new UnevenByte(0b0, 1), new UnevenByte(0b0, 1),
                new UnevenByte(0b101100001, 9), // a
                new UnevenByte(0b0, 1),
                new UnevenByte(0b101100010, 9), // b
                new UnevenByte(0b101101111, 9), // o
                new UnevenByte(0b101101100, 9)  // l
            };

            var actual = new HuffmanTree(NodeListOfInput);

            Assert.AreEqual(expected, actual.EncodedTreeList);
        }
    }
}