//using System.Collections.Generic;
//using Compression;
//using Compression.Huffman;
//using Compression.ByteStructures;
//using NUnit.Framework;
//
//namespace UnitTesting.Huffman
//{
//    [TestFixture, Category("HuffmanTreeList")]
//    class ListOfTree
//    {
//        [Test]
//        public void TreeOf_BAABCAB()
//        {
//            byte[] input = { (byte)'B', (byte)'A', (byte)'A', (byte)'B', (byte)'C', (byte)'A', (byte)'B' };
//            HuffmanEncoder ListOf_BAABBCCA = new HuffmanEncoder();
//            List<Node> temp = ListOf_BAABBCCA.HuffmanNodes(input);
//
//            HuffmanTree huffmanTreeOfBaabbca = new HuffmanTree();
//            Node actual = huffmanTreeOfBaabbca.CreateTree(temp);
//
//            // Hele Huffman træet
//            Assert.AreEqual((byte)'A', actual.symbol); //Har kun betydning for sortering
//            Assert.AreEqual(7, actual.count);
//            Assert.AreEqual(false, actual.isLeaf);
//            // Grenene af Huffman træet
//            Assert.AreEqual(4, actual.RightNode.count);
//            // Bladene af Huffman træet, går fra venstre til højre
//            Assert.AreEqual((byte)'B', actual.LeftNode.symbol);
//            Assert.AreEqual(3, actual.LeftNode.count);
//            Assert.AreEqual(true, actual.LeftNode.isLeaf);
//            Assert.AreEqual((byte)'C', actual.RightNode.LeftNode.symbol);
//            Assert.AreEqual(1, actual.RightNode.LeftNode.count);
//            Assert.AreEqual((byte)'A', actual.RightNode.RightNode.symbol);
//            Assert.AreEqual(3, actual.RightNode.RightNode.count);
//        }
//
//        [Test]
//        public void TreeOf_testfile3()
//        {
//            string path = "abcdabcd"; // brug testfile3
//            //DataFile file = new DataFile(path);
//            //byte[] input = file.GetBytes(0, file.Length);
//            byte[] input = ByteMethods.StringToByteArray(path);
//
//            HuffmanEncoder ListOf_testfile3 = new HuffmanEncoder();
//            List<Node> temp = ListOf_testfile3.HuffmanNodes(input);
//
//            HuffmanTree huffmanTreeOfTestfile3 = new HuffmanTree();
//            Node actual = huffmanTreeOfTestfile3.CreateTree(temp);
//
//            // Hele Huffman træet
//            //Assert.AreEqual((byte)'a', actual.Symbol); //Har kun betydning for sorting
//            Assert.AreEqual(8, actual.count);
//            // Grenene af Huffman træet
//            Assert.AreEqual((byte)'a', actual.LeftNode.symbol); //Sortering
//            Assert.AreEqual(4, actual.LeftNode.count);
//            Assert.AreEqual((byte)'c', actual.RightNode.symbol); //Sortering
//            Assert.AreEqual(4, actual.RightNode.count);
//            // Bladene af Huffman træet
//            Assert.AreEqual((byte)'a', actual.LeftNode.LeftNode.symbol);
//            Assert.AreEqual(2, actual.LeftNode.LeftNode.count);
//            Assert.AreEqual((byte)'b', actual.LeftNode.RightNode.symbol);
//            Assert.AreEqual(2, actual.LeftNode.RightNode.count);
//            Assert.AreEqual((byte)'c', actual.RightNode.LeftNode.symbol);
//            Assert.AreEqual(2, actual.RightNode.LeftNode.count);
//            Assert.AreEqual((byte)'d', actual.RightNode.RightNode.symbol);
//            Assert.AreEqual(2, actual.RightNode.RightNode.count);
//        }
//
//        [Test]
//        public void TreeOf_testfile2()
//        {
//            string path = "fem flade flødeboller på et fladt flødebollefad"; // brug testfile2 i stedet for
//            byte[] input = ByteMethods.StringToByteArray(path);
//            //int expected = 47;
//
//            HuffmanEncoder ListOf_testfile2 = new HuffmanEncoder();
//            List<Node> temp = ListOf_testfile2.HuffmanNodes(input);
//
//            HuffmanTree huffmanTreeOfTestfile2 = new HuffmanTree();
//            Node /*int*/ actual = huffmanTreeOfTestfile2.CreateTree(temp);
//
//            //Assert.AreEqual((byte)'l', actual.Symbol);
//            //Assert.AreEqual(expected, actual);
//
//            Assert.AreEqual(actual.RightNode.LeftNode.RightNode.RightNode.RightNode.RightNode.symbol, (byte)'p');
//        }
//    }
//    class DictionaryOfTree {
//        [Test]
//        public void DictionaryOf_AB() {
//            byte[] input = { (byte)'A', (byte)'B' };
//            var expected = new Dictionary<byte, UnevenByte> {
//                    { (byte)'A', new UnevenByte(0b0,1) },
//                    { (byte)'B', new UnevenByte(0b1,1) }
//                };
//
//            HuffmanEncoder ListOf_AB = new HuffmanEncoder();
//            List<Node> tempList = ListOf_AB.HuffmanNodes(input);
//            HuffmanTree huffmanTreeOfAb = new HuffmanTree();
//            Node tempTree = huffmanTreeOfAb.CreateTree(tempList);
//
//            Dictionary<byte, UnevenByte> actual = huffmanTreeOfAb.SetCode(tempTree);
//
//            CollectionAssert.AreEqual(expected, actual);
//        }
//
//        [Test]
//        public void DictionaryOf_ABC() {
//            byte[] input = { (byte)'A', (byte)'B', (byte)'C' };
//
//            var expected = new Dictionary<byte, UnevenByte> {
//                { (byte)'A', new UnevenByte(0b10,2) },
//                { (byte)'B', new UnevenByte(0b11,2) },
//                { (byte)'C', new UnevenByte(0b0,1) }
//            };
//
//            HuffmanEncoder ListOf_ABC = new HuffmanEncoder();
//            List<Node> tempList = ListOf_ABC.HuffmanNodes(input);
//            HuffmanTree huffmanTreeOfAbc = new HuffmanTree();
//            Node tempTree = huffmanTreeOfAbc.CreateTree(tempList);
//
//            Dictionary<byte, UnevenByte> actual = huffmanTreeOfAbc.SetCode(tempTree);
//
//            CollectionAssert.AreEqual(expected, actual);
//        }
//        [Test]
//        public void DictionaryOf_ABCD() {
//            byte[] input = { (byte)'A', (byte)'B', (byte)'C', (byte)'D' };
//
//            var expected = new Dictionary<byte, UnevenByte> {
//                { (byte)'A', new UnevenByte(0b00,2) },
//                { (byte)'B', new UnevenByte(0b01,2) },
//                { (byte)'C', new UnevenByte(0b10,2) },
//                { (byte)'D', new UnevenByte(0b11,2) }
//            };
//
//            HuffmanEncoder ListOf_ABCD = new HuffmanEncoder();
//            List<Node> tempList = ListOf_ABCD.HuffmanNodes(input);
//            HuffmanTree huffmanTreeOfAbcd = new HuffmanTree();
//            Node tempTree = huffmanTreeOfAbcd.CreateTree(tempList);
//
//            Dictionary<byte, UnevenByte> actual = huffmanTreeOfAbcd.SetCode(tempTree);
//
//            CollectionAssert.AreEqual(expected, actual);
//        }
//
//        [Test]
//        public void DictionaryOf_ABCDEFGH() {
//            byte[] input = { (byte)'A', (byte)'B', (byte)'C', (byte)'D', (byte)'E', (byte)'F', (byte)'G', (byte)'H' };
//
//            var expected = new Dictionary<byte, UnevenByte> {
//                { (byte)'A', new UnevenByte(0b000,3) },
//                { (byte)'B', new UnevenByte(0b001,3) },
//                { (byte)'C', new UnevenByte(0b010,3) },
//                { (byte)'D', new UnevenByte(0b011,3) },
//                { (byte)'E', new UnevenByte(0b100,3) },
//                { (byte)'F', new UnevenByte(0b101,3) },
//                { (byte)'G', new UnevenByte(0b110,3) },
//                { (byte)'H', new UnevenByte(0b111,3) }
//            };
//
//            HuffmanEncoder ListOf_ABCD = new HuffmanEncoder();
//            List<Node> tempList = ListOf_ABCD.HuffmanNodes(input);
//            HuffmanTree huffmanTreeOfAbcd = new HuffmanTree();
//            Node tempTree = huffmanTreeOfAbcd.CreateTree(tempList);
//
//            Dictionary<byte, UnevenByte> actual = huffmanTreeOfAbcd.SetCode(tempTree);
//
//            CollectionAssert.AreEqual(expected, actual);
//        }
//    }
//}
//
