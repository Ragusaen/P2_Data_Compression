using System.Collections.Generic;
using Compression;
using Compression.Huffman;
using Compression.ByteStructures;
using NUnit.Framework;

namespace UnitTesting.Huffman
{
    [TestFixture, Category("HuffmanLeafList")]
    public class CreateListOf_NodeLeaves {
        [Test]
        public void LeafNodeTest() {
            Node Node_1 = new LeafNode(65);
            Node Node_2 = new LeafNode(65);

            Assert.AreEqual(Node_1, Node_2);
        }

        [Test]
        public void ListOver_ABCD() {
            byte[] input = { (byte)'A', (byte)'B', (byte)'C', (byte)'D' };

            List<Node> expected = new List<Node> {
                new LeafNode((byte)'A', 1),
                new LeafNode((byte)'B', 1),
                new LeafNode((byte)'C', 1),
                new LeafNode((byte)'D', 1)
            };

            HuffmanEncoder ListOfNodes = new HuffmanEncoder();
            List<Node> actual = ListOfNodes.CreateLeafNodes(input);

            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void ListOver_ABCDBCDBDB() {
            byte[] input = ByteMethods.StringToByteArray("ABCDBCDBDB");

            List<Node> expected = new List<Node> {
                new LeafNode((byte)'A', 1),
                new LeafNode((byte)'C', 2),
                new LeafNode((byte)'D', 3),
                new LeafNode((byte)'B', 4)
            };

            HuffmanEncoder ListOfNodes = new HuffmanEncoder();
            List<Node> actual = ListOfNodes.CreateLeafNodes(input);

            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void ListOver_testfile2() {
            byte[] input = ByteMethods.StringToByteArray("fem flade flxdeboller py et fladt flxdebollefad");

            List<Node> expected = new List<Node> {
                new LeafNode((byte)'m', 1),
                new LeafNode((byte)'p', 1),
                new LeafNode((byte)'r', 1),
                new LeafNode((byte)'y', 1),
                new LeafNode((byte)'b', 2),
                new LeafNode((byte)'o', 2),
                new LeafNode((byte)'t', 2),
                new LeafNode((byte)'x', 2),
                new LeafNode((byte)'a', 3),
                new LeafNode((byte)'d', 5),
                new LeafNode((byte)' ', 6),
                new LeafNode((byte)'f', 6),
                new LeafNode((byte)'e', 7),
                new LeafNode((byte)'l', 8)
            };

            HuffmanEncoder ListOfNodes = new HuffmanEncoder();
            List<Node> actual = ListOfNodes.CreateLeafNodes(input);

            Assert.AreEqual(expected, actual);
        }
    }
}