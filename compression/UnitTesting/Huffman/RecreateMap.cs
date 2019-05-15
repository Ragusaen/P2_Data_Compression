using System.Collections;
using System.Collections.Generic;
using Compression;
using Compression.ByteStructures;
using Compression.Huffman;
using NUnit.Framework;


namespace UnitTesting.Huffman
{
    [TestFixture, Category("HuffmanCreateEncodingDictionaryTests")]
    class CreateEncodingDictionary {
        [Test]
        public void CodeDictionary_AB() {
            List<Node> input = new List<Node> {
                new LeafNode((byte)'A'),
                new LeafNode((byte)'B')
            };

            var expected = new Dictionary<byte, UnevenByte> {
                { 65, new UnevenByte(0b0, 1) },
                { 66, new UnevenByte(0b1, 1) }
            };

            HuffmanTree actual = new HuffmanTree(input);

            CollectionAssert.AreEqual(expected, actual.CodeDictionary);
        }

        [Test]
        public void CodeDictionary_testfile2() {
            byte[] input = ByteMethods.StringToByteArray("fem flade flxdeboller py et fladt flxdebollefad"); //x = ø og y = å

            var huffman = new HuffmanCompressor();
            List<Node> NodeListOfInput = huffman.CreateLeafNodes(input);

            var expected = new Dictionary<byte, UnevenByte> {
                { (byte)'m', new UnevenByte(0b00000, 5) },
                { (byte)'p', new UnevenByte(0b00001, 5) },
                { (byte)'t', new UnevenByte(0b0001, 4) },
                { (byte)'x', new UnevenByte(0b0010, 4) },
                { (byte)'r', new UnevenByte(0b00110, 5) },
                { (byte)'y', new UnevenByte(0b00111, 5) },
                { (byte)'d', new UnevenByte(0b010, 3) },
                { (byte)' ', new UnevenByte(0b011, 3) },
                { (byte)'f', new UnevenByte(0b100, 3) },
                { (byte)'e', new UnevenByte(0b101, 3) },
                { (byte)'a', new UnevenByte(0b1100, 4) },
                { (byte)'b', new UnevenByte(0b11010, 5) },
                { (byte)'o', new UnevenByte(0b11011, 5) },
                { (byte)'l', new UnevenByte(0b111,3) }
            };

            HuffmanTree actual = new HuffmanTree(NodeListOfInput);

            CollectionAssert.AreEqual(expected, actual.CodeDictionary);
        }
    }
}