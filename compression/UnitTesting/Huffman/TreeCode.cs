//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using NUnit.Framework;
//using Compression;
//using Compression.Huffman;
//using Compression.ByteStructures;
//
//namespace UnitTesting.Huffman
//{
//    [TestFixture, Category("HuffmanLeafList")]
//    class TreeCode {
//        [Test]
//        public void TreeMapOver_AB() {
//            byte[] input = { (byte)'A', (byte)'B' };
//
//            byte[] expected = { 0b11101010, 0b00001101, 0b00001001 };
//
//            HuffmanEncoder ListOf_AB = new HuffmanEncoder();
//            List<Node> temp_list = ListOf_AB.HuffmanNodes(input);
//            HuffmanTree huffmanTreeOfAb = new HuffmanTree();
//            Node temp_tree = huffmanTreeOfAb.CreateTree(temp_list);
//
//            Dictionary<byte, UnevenByte> NodeDict = huffmanTreeOfAb.SetCode(temp_tree);
//            List<UnevenByte> NodeList = huffmanTreeOfAb.TreeMap(temp_tree);
//
//            byte[] actual = ListOf_AB.EncodeEveryByteFromData(NodeList, NodeDict, input);
//
//            Assert.AreEqual(expected, actual);
//        }
//
//        [Test]
//        public void TreeMapOver_AABB() {
//            byte[] input = { (byte)'A', (byte)'A', (byte)'B', (byte)'B' };
//
//            byte[] expected = { 0b10101000, 0b00110100, 0b00100011 };
//
//            HuffmanEncoder ListOf_AABB = new HuffmanEncoder();
//            List<Node> temp_list = ListOf_AABB.HuffmanNodes(input);
//            HuffmanTree huffmanTreeOfAabb = new HuffmanTree();
//            Node temp_tree = huffmanTreeOfAabb.CreateTree(temp_list);
//
//            Dictionary<byte, UnevenByte> NodeDict = huffmanTreeOfAabb.SetCode(temp_tree);
//            List<UnevenByte> NodeList = huffmanTreeOfAabb.TreeMap(temp_tree);
//
//            byte[] actual = ListOf_AABB.EncodeEveryByteFromData(NodeList, NodeDict, input);
//
//            Assert.AreEqual(expected, actual);
//        }
//
//        [Test]
//        public void TreeMapOver_ABC() {
//            byte[] input = { (byte)'A', (byte)'B', (byte)'C' };
//
//            byte[] expected = { 0b11111101, 0b01000011, 0b01010000, 0b01101000, 0b01010110 };
//
//            HuffmanEncoder ListOf_ABC = new HuffmanEncoder();
//            List<Node> temp_list = ListOf_ABC.HuffmanNodes(input);
//            HuffmanTree huffmanTreeOfAbc = new HuffmanTree();
//            Node temp_tree = huffmanTreeOfAbc.CreateTree(temp_list);
//
//            Dictionary<byte, UnevenByte> NodeDict = huffmanTreeOfAbc.SetCode(temp_tree);
//            List<UnevenByte> NodeList = huffmanTreeOfAbc.TreeMap(temp_tree);
//
//            byte[] actual = ListOf_ABC.EncodeEveryByteFromData(NodeList, NodeDict, input);
//
//            Assert.AreEqual(expected, actual);
//        }
//
//        [Test]
//        public void TreeMapOver_ABCC() {
//            byte[] input = { (byte)'A', (byte)'B', (byte)'C', (byte)'C' };
//
//            byte[] expected = { 0b11111001, 0b01000001, 0b10100001, 0b01010000, 0b11000111 };
//
//            HuffmanEncoder ListOf_ABCC = new HuffmanEncoder();
//            List<Node> temp_list = ListOf_ABCC.HuffmanNodes(input);
//            HuffmanTree huffmanTreeOfAbcc = new HuffmanTree();
//            Node temp_tree = huffmanTreeOfAbcc.CreateTree(temp_list);
//
//            Dictionary<byte, UnevenByte> NodeDict = huffmanTreeOfAbcc.SetCode(temp_tree);
//            List<UnevenByte> NodeList = huffmanTreeOfAbcc.TreeMap(temp_tree);
//
//            byte[] actual = ListOf_ABCC.EncodeEveryByteFromData(NodeList, NodeDict, input);
//
//            Assert.AreEqual(expected, actual);
//        }
//
//        [Test]
//        public void TreeMapOver_AABC() {
//            byte[] input = { (byte)'A', (byte)'A', (byte)'B', (byte)'C' };
//
//            byte[] expected = { 0b11111010, 0b10000010, 0b10100001, 0b01010000, 0b11001011 };
//
//            HuffmanEncoder ListOf_AABC = new HuffmanEncoder();
//            List<Node> temp_list = ListOf_AABC.HuffmanNodes(input);
//            HuffmanTree huffmanTreeOfAabc = new HuffmanTree();
//            Node temp_tree = huffmanTreeOfAabc.CreateTree(temp_list);
//
//            Dictionary<byte, UnevenByte> NodeDict = huffmanTreeOfAabc.SetCode(temp_tree);
//            List<UnevenByte> NodeList = huffmanTreeOfAabc.TreeMap(temp_tree);
//
//            byte[] actual = ListOf_AABC.EncodeEveryByteFromData(NodeList, NodeDict, input);
//
//            Assert.AreEqual(expected, actual);
//        }
//
//        [Test]
//        public void TreeMapOver_ABCD() {
//            byte[] input = { (byte)'A', (byte)'B', (byte)'C', (byte)'D' };
//
//            byte[] expected = { 0b10010100, 0b00011010, 0b00010010, 0b10000111, 0b01000100, 0b00011011 };
//
//            HuffmanEncoder ListOf_ABCD = new HuffmanEncoder();
//            List<Node> temp_list = ListOf_ABCD.HuffmanNodes(input);
//            HuffmanTree huffmanTreeOfAbcd = new HuffmanTree();
//            Node temp_tree = huffmanTreeOfAbcd.CreateTree(temp_list);
//
//            Dictionary<byte, UnevenByte> NodeDict = huffmanTreeOfAbcd.SetCode(temp_tree);
//            List<UnevenByte> NodeList = huffmanTreeOfAbcd.TreeMap(temp_tree);
//
//            byte[] actual = ListOf_ABCD.EncodeEveryByteFromData(NodeList, NodeDict, input);
//
//            Assert.AreEqual(expected, actual);
//        }
//
//        [Test]
//        public void TreeMapOver_ABCDEFGH() {
//            byte[] input = { (byte)'A', (byte)'B', (byte)'C', (byte)'D', (byte)'E', (byte)'F', (byte)'G', (byte)'H' };
//
//            byte[] expected = { 0b10001010, 0b00001101, 0b00001001, 0b01000011, 0b10100010, 0b00010100, 0b01011010,
//                                0b00110010, 0b10001111, 0b01001000, 0b00000101, 0b00111001, 0b01110111};
//
//            HuffmanEncoder ListOf_ABC = new HuffmanEncoder();
//            List<Node> temp_list = ListOf_ABC.HuffmanNodes(input);
//            HuffmanTree huffmanTreeOfAbc = new HuffmanTree();
//            Node temp_tree = huffmanTreeOfAbc.CreateTree(temp_list);
//
//            Dictionary<byte, UnevenByte> NodeDict = huffmanTreeOfAbc.SetCode(temp_tree);
//            List<UnevenByte> NodeList = huffmanTreeOfAbc.TreeMap(temp_tree);
//
//            byte[] actual = ListOf_ABC.EncodeEveryByteFromData(NodeList, NodeDict, input);
//
//            Assert.AreEqual(expected, actual);
//        }
//
//        [Test]
//        public void TreeMapOver_testfile2() {
//            string path = "fem flade flødeboller på et fladt flødebollefad"; // brug testfile2 i stedet for
//            byte[] input = ByteMethods.StringToByteArray(path);
//
//            int expected = 38;
//
//            HuffmanEncoder ListOf_testfile2 = new HuffmanEncoder();
//            List<Node> temp = ListOf_testfile2.HuffmanNodes(input);
//            HuffmanTree huffmanTreeOfTestfile2 = new HuffmanTree();
//            Node temp_tree = huffmanTreeOfTestfile2.CreateTree(temp);
//
//            Dictionary<byte, UnevenByte> NodeDict = huffmanTreeOfTestfile2.SetCode(temp_tree);
//            List<UnevenByte> NodeList = huffmanTreeOfTestfile2.TreeMap(temp_tree);
//
//            byte[] actual = ListOf_testfile2.EncodeEveryByteFromData(NodeList, NodeDict, input);
//
//            Assert.AreEqual(expected, actual.Length);
//        }
//    }
//}
