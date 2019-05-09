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
    [TestFixture, Category("HuffmanLeafList")]
    class TreeCode {
        [Test]
        public void TreeMapOver_AB() {
            byte[] input = { (byte)'A', (byte)'B' };

            byte[] expected = { 0b11101010, 0b00001101, 0b00001001 };

            Encode ListOf_AB = new Encode();
            List<Nodes> temp_list = ListOf_AB.HuffmanNodes(input);
            Tree TreeOf_AB = new Tree();
            Nodes temp_tree = TreeOf_AB.CreateTree(temp_list);

            Dictionary<byte, UnevenByte> NodeDict = TreeOf_AB.SetCode(temp_tree);
            List<UnevenByte> NodeList = TreeOf_AB.TreeMap(temp_tree);

            byte[] actual = ListOf_AB.EncodeEveryByteFromData(NodeList, NodeDict, input);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TreeMapOver_AABB() {
            byte[] input = { (byte)'A', (byte)'A', (byte)'B', (byte)'B' };

            byte[] expected = { 0b10101000, 0b00110100, 0b00100011 };

            Encode ListOf_AABB = new Encode();
            List<Nodes> temp_list = ListOf_AABB.HuffmanNodes(input);
            Tree TreeOf_AABB = new Tree();
            Nodes temp_tree = TreeOf_AABB.CreateTree(temp_list);

            Dictionary<byte, UnevenByte> NodeDict = TreeOf_AABB.SetCode(temp_tree);
            List<UnevenByte> NodeList = TreeOf_AABB.TreeMap(temp_tree);

            byte[] actual = ListOf_AABB.EncodeEveryByteFromData(NodeList, NodeDict, input);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TreeMapOver_ABC() {
            byte[] input = { (byte)'A', (byte)'B', (byte)'C' };

            byte[] expected = { 0b11111101, 0b01000011, 0b01010000, 0b01101000, 0b01010110 };

            Encode ListOf_ABC = new Encode();
            List<Nodes> temp_list = ListOf_ABC.HuffmanNodes(input);
            Tree TreeOf_ABC = new Tree();
            Nodes temp_tree = TreeOf_ABC.CreateTree(temp_list);

            Dictionary<byte, UnevenByte> NodeDict = TreeOf_ABC.SetCode(temp_tree);
            List<UnevenByte> NodeList = TreeOf_ABC.TreeMap(temp_tree);

            byte[] actual = ListOf_ABC.EncodeEveryByteFromData(NodeList, NodeDict, input);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TreeMapOver_ABCC() {
            byte[] input = { (byte)'A', (byte)'B', (byte)'C', (byte)'C' };

            byte[] expected = { 0b11111001, 0b01000001, 0b10100001, 0b01010000, 0b11000111 };

            Encode ListOf_ABCC = new Encode();
            List<Nodes> temp_list = ListOf_ABCC.HuffmanNodes(input);
            Tree TreeOf_ABCC = new Tree();
            Nodes temp_tree = TreeOf_ABCC.CreateTree(temp_list);

            Dictionary<byte, UnevenByte> NodeDict = TreeOf_ABCC.SetCode(temp_tree);
            List<UnevenByte> NodeList = TreeOf_ABCC.TreeMap(temp_tree);

            byte[] actual = ListOf_ABCC.EncodeEveryByteFromData(NodeList, NodeDict, input);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TreeMapOver_AABC() {
            byte[] input = { (byte)'A', (byte)'A', (byte)'B', (byte)'C' };

            byte[] expected = { 0b11111010, 0b10000010, 0b10100001, 0b01010000, 0b11001011 };

            Encode ListOf_AABC = new Encode();
            List<Nodes> temp_list = ListOf_AABC.HuffmanNodes(input);
            Tree TreeOf_AABC = new Tree();
            Nodes temp_tree = TreeOf_AABC.CreateTree(temp_list);

            Dictionary<byte, UnevenByte> NodeDict = TreeOf_AABC.SetCode(temp_tree);
            List<UnevenByte> NodeList = TreeOf_AABC.TreeMap(temp_tree);

            byte[] actual = ListOf_AABC.EncodeEveryByteFromData(NodeList, NodeDict, input);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TreeMapOver_ABCD() {
            byte[] input = { (byte)'A', (byte)'B', (byte)'C', (byte)'D' };

            byte[] expected = { 0b10010100, 0b00011010, 0b00010010, 0b10000111, 0b01000100, 0b00011011 };

            Encode ListOf_ABCD = new Encode();
            List<Nodes> temp_list = ListOf_ABCD.HuffmanNodes(input);
            Tree TreeOf_ABCD = new Tree();
            Nodes temp_tree = TreeOf_ABCD.CreateTree(temp_list);

            Dictionary<byte, UnevenByte> NodeDict = TreeOf_ABCD.SetCode(temp_tree);
            List<UnevenByte> NodeList = TreeOf_ABCD.TreeMap(temp_tree);

            byte[] actual = ListOf_ABCD.EncodeEveryByteFromData(NodeList, NodeDict, input);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TreeMapOver_ABCDEFGH() {
            byte[] input = { (byte)'A', (byte)'B', (byte)'C', (byte)'D', (byte)'E', (byte)'F', (byte)'G', (byte)'H' };

            byte[] expected = { 0b10001010, 0b00001101, 0b00001001, 0b01000011, 0b10100010, 0b00010100, 0b01011010,
                                0b00110010, 0b10001111, 0b01001000, 0b00000101, 0b00111001, 0b01110111};

            Encode ListOf_ABC = new Encode();
            List<Nodes> temp_list = ListOf_ABC.HuffmanNodes(input);
            Tree TreeOf_ABC = new Tree();
            Nodes temp_tree = TreeOf_ABC.CreateTree(temp_list);

            Dictionary<byte, UnevenByte> NodeDict = TreeOf_ABC.SetCode(temp_tree);
            List<UnevenByte> NodeList = TreeOf_ABC.TreeMap(temp_tree);

            byte[] actual = ListOf_ABC.EncodeEveryByteFromData(NodeList, NodeDict, input);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TreeMapOver_testfile2() {
            string path = "fem flade flødeboller på et fladt flødebollefad"; // brug testfile2 i stedet for
            byte[] input = ByteMethods.StringToByteArray(path);

            int expected = 38;

            Encode ListOf_testfile2 = new Encode();
            List<Nodes> temp = ListOf_testfile2.HuffmanNodes(input);
            Tree TreeOf_testfile2 = new Tree();
            Nodes temp_tree = TreeOf_testfile2.CreateTree(temp);

            Dictionary<byte, UnevenByte> NodeDict = TreeOf_testfile2.SetCode(temp_tree);
            List<UnevenByte> NodeList = TreeOf_testfile2.TreeMap(temp_tree);

            byte[] actual = ListOf_testfile2.EncodeEveryByteFromData(NodeList, NodeDict, input);

            Assert.AreEqual(expected, actual.Length);
        }
    }
}
