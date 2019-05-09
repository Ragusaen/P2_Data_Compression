using System.Collections.Generic;
using Compression;
using Compression.Huffman;
using Compression.ByteStructures;
using NUnit.Framework;

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
            Assert.AreEqual(false, actual.isLeaf);
            // Grenene af Huffman træet
            Assert.AreEqual(4, actual.rightNode.count);
            // Bladene af Huffman træet, går fra venstre til højre
            Assert.AreEqual((byte)'B', actual.leftNode.symbol);
            Assert.AreEqual(3, actual.leftNode.count);
            Assert.AreEqual(true, actual.leftNode.isLeaf);
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
    class DictionaryOfTree {
        [Test]
        public void DictionaryOf_AB() {
            byte[] input = { (byte)'A', (byte)'B' };
            var expected = new Dictionary<byte, UnevenByte> {
                    { (byte)'A', new UnevenByte(0b0,1) },
                    { (byte)'B', new UnevenByte(0b1,1) }
                };

            Encode ListOf_AB = new Encode();
            List<Nodes> tempList = ListOf_AB.HuffmanNodes(input);
            Tree TreeOf_AB = new Tree();
            Nodes tempTree = TreeOf_AB.CreateTree(tempList);

            Dictionary<byte, UnevenByte> actual = TreeOf_AB.SetCode(tempTree);

            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void DictionaryOf_ABC() {
            byte[] input = { (byte)'A', (byte)'B', (byte)'C' };

            var expected = new Dictionary<byte, UnevenByte> {
                { (byte)'A', new UnevenByte(0b10,2) },
                { (byte)'B', new UnevenByte(0b11,2) },
                { (byte)'C', new UnevenByte(0b0,1) }
            };

            Encode ListOf_ABC = new Encode();
            List<Nodes> tempList = ListOf_ABC.HuffmanNodes(input);
            Tree TreeOf_ABC = new Tree();
            Nodes tempTree = TreeOf_ABC.CreateTree(tempList);

            Dictionary<byte, UnevenByte> actual = TreeOf_ABC.SetCode(tempTree);

            CollectionAssert.AreEqual(expected, actual);
        }
        [Test]
        public void DictionaryOf_ABCD() {
            byte[] input = { (byte)'A', (byte)'B', (byte)'C', (byte)'D' };

            var expected = new Dictionary<byte, UnevenByte> {
                { (byte)'A', new UnevenByte(0b00,2) },
                { (byte)'B', new UnevenByte(0b01,2) },
                { (byte)'C', new UnevenByte(0b10,2) },
                { (byte)'D', new UnevenByte(0b11,2) }
            };

            Encode ListOf_ABCD = new Encode();
            List<Nodes> tempList = ListOf_ABCD.HuffmanNodes(input);
            Tree TreeOf_ABCD = new Tree();
            Nodes tempTree = TreeOf_ABCD.CreateTree(tempList);

            Dictionary<byte, UnevenByte> actual = TreeOf_ABCD.SetCode(tempTree);

            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void DictionaryOf_ABCDEFGH() {
            byte[] input = { (byte)'A', (byte)'B', (byte)'C', (byte)'D', (byte)'E', (byte)'F', (byte)'G', (byte)'H' };

            var expected = new Dictionary<byte, UnevenByte> {
                { (byte)'A', new UnevenByte(0b000,3) },
                { (byte)'B', new UnevenByte(0b001,3) },
                { (byte)'C', new UnevenByte(0b010,3) },
                { (byte)'D', new UnevenByte(0b011,3) },
                { (byte)'E', new UnevenByte(0b100,3) },
                { (byte)'F', new UnevenByte(0b101,3) },
                { (byte)'G', new UnevenByte(0b110,3) },
                { (byte)'H', new UnevenByte(0b111,3) }
            };

            Encode ListOf_ABCD = new Encode();
            List<Nodes> tempList = ListOf_ABCD.HuffmanNodes(input);
            Tree TreeOf_ABCD = new Tree();
            Nodes tempTree = TreeOf_ABCD.CreateTree(tempList);

            Dictionary<byte, UnevenByte> actual = TreeOf_ABCD.SetCode(tempTree);

            CollectionAssert.AreEqual(expected, actual);
        }
    }
}

