using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Compression;
using Compression.Huffman;

namespace UnitTesting.Huffman
{
    [TestFixture, Category("HuffmanLeafList")]
    class TreeCode
    {
        [Test]
        public void TreeMapOver_AB()
        {
            byte[] input = { (byte)'A', (byte)'B' };
            string expected = "0101000001101000010"; //01A1B

            Encode ListOf_AB = new Encode();
            List<Nodes> temp_list = ListOf_AB.HuffmanNodes(input);
            Tree TreeOf_AB = new Tree();
            Nodes temp_tree = TreeOf_AB.CreateTree(temp_list);

            string actual = TreeOf_AB.TreeMap(temp_tree);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TreeMapOver_AABB()
        {
            byte[] input = { (byte)'A', (byte)'A', (byte)'B', (byte)'B' };
            string expected = "0101000001101000010"; //01A1B

            Encode ListOf_AABB = new Encode();
            List<Nodes> temp_list = ListOf_AABB.HuffmanNodes(input);
            Tree TreeOf_AABB = new Tree();
            Nodes temp_tree = TreeOf_AABB.CreateTree(temp_list);

            string actual = TreeOf_AABB.TreeMap(temp_tree);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TreeMapOver_ABC()
        {
            byte[] input = { (byte)'A', (byte)'B', (byte)'C' };
            string expected = "01010000110101000001101000010"; //01C01A1B

            Encode ListOf_ABC = new Encode();
            List<Nodes> temp_list = ListOf_ABC.HuffmanNodes(input);
            Tree TreeOf_ABC = new Tree();
            Nodes temp_tree = TreeOf_ABC.CreateTree(temp_list);

            string actual = TreeOf_ABC.TreeMap(temp_tree);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TreeMapOver_ABCC()
        {
            byte[] input = { (byte)'A', (byte)'B', (byte)'C', (byte)'C' };
            string expected = "00101000001101000010101000011"; //001A1B1C

            Encode ListOf_ABCC = new Encode();
            List<Nodes> temp_list = ListOf_ABCC.HuffmanNodes(input);
            Tree TreeOf_ABCC = new Tree();
            Nodes temp_tree = TreeOf_ABCC.CreateTree(temp_list);

            string actual = TreeOf_ABCC.TreeMap(temp_tree);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TreeMapOver_AABC()
        {
            byte[] input = { (byte)'A', (byte)'A', (byte)'B', (byte)'C' };
            string expected = "01010000010101000010101000011"; //01A01B1C

            Encode ListOf_AABC = new Encode();
            List<Nodes> temp_list = ListOf_AABC.HuffmanNodes(input);
            Tree TreeOf_AABC = new Tree();
            Nodes temp_tree = TreeOf_AABC.CreateTree(temp_list);

            string actual = TreeOf_AABC.TreeMap(temp_tree);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TreeMapOver_ABCD()
        {
            byte[] input = { (byte)'A', (byte)'B', (byte)'C', (byte)'D' };
            string expected = "001010000011010000100101000011101000100"; //001A1B01C1D

            Encode ListOf_ABC = new Encode();
            List<Nodes> temp_list = ListOf_ABC.HuffmanNodes(input);
            Tree TreeOf_ABC = new Tree();
            Nodes temp_tree = TreeOf_ABC.CreateTree(temp_list);

            string actual = TreeOf_ABC.TreeMap(temp_tree);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TreeMapOver_ABCDEFGH()
        {
            byte[] input = { (byte)'A', (byte)'B', (byte)'C', (byte)'D', (byte)'E', (byte)'F', (byte)'G', (byte)'H' };
            string expected = "0001010000011010000100101000011101000100001010001011010001100101000111101001000"; //0001A1B01C1D001E1F01G1H

            Encode ListOf_ABC = new Encode();
            List<Nodes> temp_list = ListOf_ABC.HuffmanNodes(input);
            Tree TreeOf_ABC = new Tree();
            Nodes temp_tree = TreeOf_ABC.CreateTree(temp_list);

            string actual = TreeOf_ABC.TreeMap(temp_tree);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TreeMapOver_BAABCCDD()
        {
            byte[] input = { (byte)'B', (byte)'A', (byte)'A', (byte)'B', (byte)'C', (byte)'C', (byte)'D', (byte)'D' };
            string expected = "001010000011010000100101000011101000100"; //001A1B01C1D

            Encode ListOfBAABBCA = new Encode();
            List<Nodes> temp_list = ListOfBAABBCA.HuffmanNodes(input);
            Tree TreeOf_BAABBCA = new Tree();
            Nodes temp_node = TreeOf_BAABBCA.CreateTree(temp_list);


            string actual = TreeOf_BAABBCA.TreeMap(temp_node);

            Assert.AreEqual(expected, actual);
        }
        [Test]
        public void TreeMapOver_BAABBCA()
        {
            byte[] input = { (byte)'B', (byte)'A', (byte)'A', (byte)'B', (byte)'B', (byte)'C', (byte)'A' };
            string expected = "01010000100101000011101000001"; //01B01C1A

            Encode ListOfBAABBCA = new Encode();
            List<Nodes> temp_list = ListOfBAABBCA.HuffmanNodes(input);
            Tree TreeOf_BAABBCA = new Tree();
            Nodes temp_node = TreeOf_BAABBCA.CreateTree(temp_list);

            string actual = TreeOf_BAABBCA.TreeMap(temp_node);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TreeMapOver_testfile2()
        {
            string path = "fem flade flødeboller på et fladt flødebollefad"; // brug testfile2 i stedet for
            byte[] input = ByteMethods.StringToByteArray(path);

            string expected = "0000101101111010111001011110010101011101001111110000101100100100100000001011001100101100001010110001001011011011011100000101100101101101100";
            //00001o01r1å01t1ø01d1 001f01a01b01m1p01e1l

            Encode ListOf_testfile2 = new Encode();
            List<Nodes> temp = ListOf_testfile2.HuffmanNodes(input);
            Tree TreeOf_testfile2 = new Tree();
            Nodes temp_node = TreeOf_testfile2.CreateTree(temp);

            string actual = TreeOf_testfile2.TreeMap(temp_node);

            Assert.AreEqual(expected, actual);
        }
    }
}
