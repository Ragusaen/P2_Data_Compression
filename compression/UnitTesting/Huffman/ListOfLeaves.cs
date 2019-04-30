using NUnit.Framework;
using Compression;
using Compression.Huffman;
using System;
using System.Collections.Generic;

namespace UnitTesting.Huffman
{
    [TestFixture, Category("HuffmanLeafList")]
    public class ListOfLeaves {
        [Test]
        public void ListOver_BAABBCA()
        {
            byte[] input = { (byte)'B', (byte)'A', (byte)'A', (byte)'B', (byte)'C', (byte)'A', (byte)'B' };
            byte[] expected_symbol = { (byte)'C', (byte)'A', (byte)'B' };
            int[] expected_count = { 1, 3, 3 };

            Encode ListOfBAABBCA = new Encode();

            List<Nodes> actual = ListOfBAABBCA.HuffmanNodes(input);

            byte[] actual_symbol = new byte[3];
            int[] actual_count = new int[3];

            for( int i = 0; i < 3; i++) {
                actual_symbol[i] = actual[i].symbol;
                actual_count[i] = actual[i].count;
            }
      
            Assert.AreEqual(expected_symbol, actual_symbol);
            Assert.AreEqual(expected_count, actual_count);
        }

        [Test]
        public void ListOver_testfile3()
        {
            string path = TestContext.CurrentContext.TestDirectory + "../../../res/testfile3";
            DataFile file = new DataFile(path);
            byte[] input = file.GetBytes(0, file.Length);

            byte[] expected_symbol = { (byte)'a', (byte)'b', (byte)'c', (byte)'d' };
            int[] expected_count = { 2, 2, 2, 2 };

            Encode ListOf_testfile3 = new Encode();

            List<Nodes> actual = ListOf_testfile3.HuffmanNodes(input);

            byte[] actual_symbol = new byte[4];
            int[] actual_count = new int[4];

            for (int i = 0; i < 4; i++)
            {
                actual_symbol[i] = actual[i].symbol;
                actual_count[i] = actual[i].count;
            }

            Assert.AreEqual(expected_symbol, actual_symbol);
            Assert.AreEqual(expected_count, actual_count);
        }

        [Test]
        public void ListOver_testfile2()
        {
            //string path = TestContext.CurrentContext.TestDirectory + "../../../res/testfile2";
            //DataFile file = new DataFile(path);
            //byte[] input = file.GetBytes(0, file.Length);

            byte[] input = { (byte)'f', (byte)'e', (byte)'m', (byte)' ', (byte)'f', (byte)'l', (byte)'a', (byte)'d', (byte)'e', (byte)' ', (byte)'f', (byte)'l', (byte)'ø', (byte)'d', (byte)'e', (byte)'b', (byte)'o', (byte)'l', (byte)'l', (byte)'e', (byte)'r', (byte)' ', (byte)'p', (byte)'å', (byte)' ', (byte)'e', (byte)'t', (byte)' ', (byte)'f', (byte)'l', (byte)'a', (byte)'d', (byte)'t', (byte)' ', (byte)'f', (byte)'l', (byte)'ø', (byte)'d', (byte)'e', (byte)'b', (byte)'o', (byte)'l', (byte)'l', (byte)'e', (byte)'f', (byte)'a', (byte)'d' };

            byte[] expected_symbol = { (byte)'m', (byte)'p', (byte)'r', (byte)'å', (byte)'b', (byte)'o', (byte)'t',
                                       (byte)'ø', (byte)'a', (byte)'d', (byte)' ', (byte)'f', (byte)'e', (byte)'l' };
            int[] expected_count = { 1, 1, 1, 1, 2, 2, 2, 2, 3, 5, 6, 6, 7, 8 };

            Encode ListOf_testfile2 = new Encode();

            List<Nodes> actual = ListOf_testfile2.HuffmanNodes(input);

            byte[] actual_symbol = new byte[14];
            int[] actual_count = new int[14];

            for (int i = 0; i < 14; i++)
            {
                actual_symbol[i] = actual[i].symbol;
                actual_count[i] = actual[i].count;
            }

            Assert.AreEqual(expected_symbol, actual_symbol);
            Assert.AreEqual(expected_count, actual_count);
        }
    }
}
