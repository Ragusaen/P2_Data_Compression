using System.Collections;
using System.Collections.Generic;
using Compression;
using Compression.ByteStructures;
using Compression.Huffman;
using NUnit.Framework;


namespace UnitTesting.Huffman
{
    [TestFixture, Category("CreateDictionaryFromEncodedCode")]
    class RecreateMap
    {
        [Test]
        public void test() {

            string temp = "11000000001001111000011";

            byte[] expected = { 96, 19, 195 };

            temp.PadLeft((temp.Length / 8 + 1) * 8, '0');

            byte[] actual = ByteMethods.BinaryStringToByteArray(temp);

            byte test = ByteMethods.BinaryStringToByte(temp);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Minor_test() { //Slet
            byte expected = (byte)'A';

            byte actual = ByteMethods.BinaryStringToByte("01000001");

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Minor_test2() {
            string input = "01017";

            string expected = "0101";

            string actual = input.Remove(input.Length - 1);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Minor_test3() {
            byte[] input = { (byte)'A', (byte)'B' };

            BitArray expected = new BitArray(new byte[] { (byte)'A', (byte)'B' });

            BitArray actual = new BitArray(input);

            Assert.AreEqual(expected[0], true);
        }

        [Test]
        public void Minor_test4() {
            byte[] input = { (byte)'A', (byte)'B', (byte)'C' };
            int i = 8; //Starter ved B

            byte expected = (byte)'B';

            BitArray bitArrayOfInput = new BitArray(input);

            byte actual = new byte();

            for (int j = i + 7; j >= i; j--) {
                actual = (byte)((actual << 1) | (bitArrayOfInput[j] ? 1 : 0));
            }

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Minor_test5() { //Lav A om til B
            byte[] input = { (byte)'A' };
            int i = 0;

            byte expected = (byte)'B';

            BitArray temp = new BitArray(input);

            for (i = 0; i < 2; i++) {
                if (temp[i] == true) {
                    temp[i] = false;
                }
                else if (temp[i] == false) {
                    temp[i] = true;
                }
            }

            byte actual = new byte();

            for (int j = 7; j >= 0; j--) {
                actual = (byte)((actual << 1) | (temp[j] ? 1 : 0));
            }

            Assert.AreEqual(expected, actual);
        }



        [Test]
        public void Minor_test7() {
            List<string> test = new List<string> {
                "aa",
                new string('b',2),
                new string('a',2)
            };

            Assert.AreEqual("aa", test[2]);
        }

        [Test]
        public void Minor_test8() {
            var tempDict = new Dictionary<byte, string> {
                { 65, "1" },
                { 66, "2" },
                { 67, "3" }
            };
            List<string> tempList = new List<string> {
                "4", "2"
            };

            List<string> expected = new List<string> {
                "4", "2", "1", "2", "3", "3", "1", "2"
            };

            byte[] tempByteArray = { 65, 66, 67, 67, 65, 66 };

            for (int i = 0; i < tempByteArray.Length; i++) {
                tempList.Add(tempDict[tempByteArray[i]]);
            }

            CollectionAssert.AreEqual(expected, tempList);
        }

        [Test]
        public void Minor_test9() {
            byte[] temp = { (byte)'A', (byte)'B' };

            Dictionary<byte, UnevenByte> NodeDict = new Dictionary<byte, UnevenByte> {
                { (byte)'A', new UnevenByte(0b0,1) }, //A -> 0b0
                { (byte)'B', new UnevenByte(0b1,1) }  //B -> 0b1
            };

            List<UnevenByte> NodeList = new List<UnevenByte> {
                new UnevenByte(0b0101000001, 10),
                new UnevenByte(0b101000010, 9)
            };

            //List<UnevenByte> expected = new List<UnevenByte> {
            //    //new UnevenByte(0b1111,4),
            //    new UnevenByte(0b0101000001,10),
            //    new UnevenByte(0b101000010,9),
            //    new UnevenByte(0b0,1),
            //    new UnevenByte(0b1,1)
            //};

            //byte[] expected = { 0b01010000, 0b01101000, 0b01001000 };
            byte[] expected = { 0b11101010, 0b00001101, 0b00001001 };

            Encode test = new Encode();
            byte[] actual = test.EncodeEveryByteFromData(NodeList, NodeDict, temp);

            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void Minor_test10() {
            byte[] input = { 65, 66 };

            Encode ListOf_AB = new Encode();
            List<Nodes> temp_list = ListOf_AB.HuffmanNodes(input);
            Tree TreeOf_AB = new Tree();
            Nodes temp_tree = TreeOf_AB.CreateTree(temp_list);

            Dictionary<byte, UnevenByte> NodeDict = TreeOf_AB.SetCode(temp_tree);
            List<UnevenByte> NodeList = TreeOf_AB.TreeMap(temp_tree);

            byte[] actual = ListOf_AB.EncodeEveryByteFromData(NodeList, NodeDict, input);
            //Dictionary<byte, UnevenByte> expected = new Dictionary<byte, UnevenByte>
            //{
            //    {65, new UnevenByte(0b0,1) },
            //    {66, new UnevenByte(0b1,1) }
            //};
            //List<UnevenByte> expected = new List<UnevenByte> {
            //    new UnevenByte(0b111,3),
            //    new UnevenByte(0b0101000001,10),
            //    new UnevenByte(0b101000010,9),
            //    new UnevenByte(0b0,1),
            //    new UnevenByte(0b1,1)
            //};

            byte[] expected = { 0b11101010, 0b00001101, 0b00001001 };

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Minor_test11() {
            List<UnevenByte> temp = new List<UnevenByte> {
                new UnevenByte(0b10010, 5),
                new UnevenByte(0b111011, 6)
            };
            int i = temp[1].Length;

            Assert.AreEqual(6, i);
        }

        [Test]
        public void Minor_test12() {
            UnevenByte test = new UnevenByte(0b1111, 4);

            Assert.AreEqual(new UnevenByte(0b1111, 4), test);
        }

        [Test]
        public void Minor_test13() {
            byte[] input = { 65, 65, 66, 66, 66 };

            byte[] expected = { 0b01010000, 0b01101000, 0b01000111 };

            Encode ListOf_AABBB = new Encode();
            List<Nodes> temp_list = ListOf_AABBB.HuffmanNodes(input);
            Tree TreeOf_AABBB = new Tree();
            Nodes temp_tree = TreeOf_AABBB.CreateTree(temp_list);

            Dictionary<byte, UnevenByte> NodeDict = TreeOf_AABBB.SetCode(temp_tree);
            List<UnevenByte> NodeList = TreeOf_AABBB.TreeMap(temp_tree);

            byte[] actual = ListOf_AABBB.EncodeEveryByteFromData(NodeList, NodeDict, input);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Minor_test14()
        {
            byte[] input = { 0b10010100, 0b00011010, 0b00010010, 0b10000111, 0b01000100, 0b00011011, 0b00011011 }; //ABCDABCD

            Dictionary<UnevenByte, byte> DecodeDict = new Dictionary<UnevenByte, byte>();

            Dictionary<UnevenByte, byte> expected = new Dictionary<UnevenByte, byte>
            {
                { new UnevenByte(0b00, 2), 65 },
                { new UnevenByte(0b01, 2), 66 },
                { new UnevenByte(0b10, 2), 67 },
                { new UnevenByte(0b11, 2), 68 }
            };

            Decode_3 decode = new Decode_3();
            int i = 3;
            decode.CreateDecodeDictionary(input, DecodeDict, ref i);
            CollectionAssert.AreEqual(expected, DecodeDict);
        }

        [Test]
        public void Minor_test15() {
            byte[] input = { 0b10010100, 0b00011010, 0b00010010, 0b10000111, 0b01000100, 0b00011011 }; //ABCD

            Dictionary<UnevenByte, byte> DecodeDict = new Dictionary<UnevenByte, byte>();

            Dictionary<UnevenByte, byte> expected = new Dictionary<UnevenByte, byte>
            {
                { new UnevenByte(0b00, 2), 65 },
                { new UnevenByte(0b01, 2), 66 },
                { new UnevenByte(0b10, 2), 67 },
                { new UnevenByte(0b11, 2), 68 }
            };

            Decode_3 decode = new Decode_3();
            int i = 3;

            decode.CreateDecodeDictionary(input, DecodeDict, ref i);

            Assert.AreEqual(expected[new UnevenByte(0b00, 2)], DecodeDict[new UnevenByte(0b00, 2)]);
        }

        [Test]
        public void Minor_test16()
        {
            UnevenByte ub = new UnevenByte(0b10101010101010, 14);

            UnevenByte temp = new UnevenByte(ub.GetBits(8), 8);

            Assert.AreEqual(new UnevenByte(0b10101010, 8), temp);
        }
        [Test]
        public void Minor_test17()
        {
            Decode_3 decode = new Decode_3();

            UnevenByte ub = decode.ConvertCodeToUnevenByte("11011");

            //byte[] barr = ByteMethods.BinaryStringToByteArray("11011");


            Assert.AreEqual(new UnevenByte(0b11011,5),ub);
        }

        [Test]
        public void Minor_test18()
        {
            UnevenByte test = new UnevenByte(0b111010100, 9);

            int l = 4;

            UnevenByte temp = new UnevenByte(test.Data >> (test.Length - l) % (1 << test.Length), l);

            UnevenByte subject = new UnevenByte(test.GetBits(l), l);

            //UnevenByte subject = new UnevenByte((uint)(test.Data / (1 << (test.Length - l))), test.Length - l);

            //Assert.AreEqual(new UnevenByte(0b1110, 4), temp);

            Assert.AreEqual(0b11111, 255 >> 3);
        }
    }
}
