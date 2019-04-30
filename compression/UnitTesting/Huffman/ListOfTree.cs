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
    [TestFixture, Category("HuffmanTreeList")]
    class ListOfTree
    {
        [Test]
        public void TreeOf_BAABCAB()
        {
            byte[] input = { (byte)'B', (byte)'A', (byte)'A', (byte)'B', (byte)'C', (byte)'A', (byte)'B' };
            Encode ListOf_BAABBCCA = new Encode();
            List<Nodes> temp = ListOf_BAABBCCA.HuffmanNodes(input);

            Tree TreeOf_BAABBCA = new Tree();
            Nodes actual = TreeOf_BAABBCA.CreateTree(temp);

            // Hele Huffman træet
            Assert.AreEqual((byte)'A', actual.symbol); //Har kun betydning for sortering
            Assert.AreEqual(7, actual.count);
            // Grenene af Huffman træet
            Assert.AreEqual(4, actual.rightNode.count);
            // Bladene af Huffman træet, går fra venstre til højre
            Assert.AreEqual((byte)'B', actual.leftNode.symbol);
            Assert.AreEqual(3, actual.leftNode.count);
            Assert.AreEqual((byte)'C', actual.rightNode.leftNode.symbol);
            Assert.AreEqual(1, actual.rightNode.leftNode.count);
            Assert.AreEqual((byte)'A', actual.rightNode.rightNode.symbol);
            Assert.AreEqual(3, actual.rightNode.rightNode.count);
        }

        [Test]
        public void TreeOf_testfile3()
        {
            string path = "abcdabcd"; // brug testfile3
            //DataFile file = new DataFile(path);
            //byte[] input = file.GetBytes(0, file.Length);
            byte[] input = ByteMethods.StringToByteArray(path);

            Encode ListOf_testfile3 = new Encode();
            List<Nodes> temp = ListOf_testfile3.HuffmanNodes(input);

            Tree TreeOf_testfile3 = new Tree();
            Nodes actual = TreeOf_testfile3.CreateTree(temp);

            // Hele Huffman træet
            //Assert.AreEqual((byte)'a', actual.Symbol); //Har kun betydning for sorting
            Assert.AreEqual(8, actual.count);
            // Grenene af Huffman træet
            Assert.AreEqual((byte)'a', actual.leftNode.symbol); //Sortering
            Assert.AreEqual(4, actual.leftNode.count);
            Assert.AreEqual((byte)'c', actual.rightNode.symbol); //Sortering
            Assert.AreEqual(4, actual.rightNode.count);
            // Bladene af Huffman træet
            Assert.AreEqual((byte)'a', actual.leftNode.leftNode.symbol);
            Assert.AreEqual(2, actual.leftNode.leftNode.count);
            Assert.AreEqual((byte)'b', actual.leftNode.rightNode.symbol);
            Assert.AreEqual(2, actual.leftNode.rightNode.count);
            Assert.AreEqual((byte)'c', actual.rightNode.leftNode.symbol);
            Assert.AreEqual(2, actual.rightNode.leftNode.count);
            Assert.AreEqual((byte)'d', actual.rightNode.rightNode.symbol);
            Assert.AreEqual(2, actual.rightNode.rightNode.count);
        }

        [Test]
        public void TreeOf_testfile2()
        {
            string path = "fem flade flødeboller på et fladt flødebollefad"; // brug testfile2 i stedet for
            byte[] input = ByteMethods.StringToByteArray(path);
            //int expected = 47;

            Encode ListOf_testfile2 = new Encode();
            List<Nodes> temp = ListOf_testfile2.HuffmanNodes(input);

            Tree TreeOf_testfile2 = new Tree();
            Nodes /*int*/ actual = TreeOf_testfile2.CreateTree(temp);

            //Assert.AreEqual((byte)'l', actual.Symbol);
            //Assert.AreEqual(expected, actual);

            Assert.AreEqual(actual.rightNode.leftNode.rightNode.rightNode.rightNode.rightNode.symbol, (byte)'p');
        }
    }
}
