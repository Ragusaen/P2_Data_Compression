using System.Collections.Generic;
using Compression;
using Compression.Huffman;
using Compression.ByteStructures;
using NUnit.Framework;

namespace UnitTesting.Huffman
{
    [TestFixture, Category("HuffmanTreeList")]
    class CreateRootNode_OfListOfLeafNodes {
        [Test]
        public void RootTreeTest() {
            Node Tree1 = new BranchNode(
                new LeafNode((byte)'A'),
                new LeafNode((byte)'B'));

            Node Tree2 = new BranchNode(
                new LeafNode((byte)'A'),
                new LeafNode((byte)'B'));

            Assert.AreEqual(Tree1, Tree2);
        }

        [Test]
        public void TreeOf_AB() {
            List<Node> input = new List<Node> {
                new LeafNode((byte)'A'),
                new LeafNode((byte)'B')
            };

            byte expected = (byte)'B';

            var actual = new HuffmanTree(input);

            Assert.AreEqual(expected, actual.RootNode.symbol);
        }

        [Test]
        public void TreeOf_testfile2() {
            byte[] input = ByteMethods.StringToByteArray("fem flade flødeboller på et fladt flødebollefad");

            byte expected = (byte)'l';

            var huffman = new HuffmanCompressor();
            List<Node> NodeListOfInput = huffman.CreateLeafNodes(input);

            var actual = new HuffmanTree(NodeListOfInput);

            Assert.AreEqual(expected, actual.RootNode.symbol);
        }
    }
}
