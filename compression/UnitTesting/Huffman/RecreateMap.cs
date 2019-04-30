using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compression;
using Compression.Huffman;
using NUnit.Framework;
using System.Collections;

namespace UnitTesting.Huffman
{
    [TestFixture, Category("CreateDictionaryFromEncodedCode")]
    class RecreateMap
    {
        [Test]
        public void test() {
            string input = "0101000001101000010";
            byte[] ByteArrayOf_AB = ByteMethods.StringToByteArray(input);

            //Dictionary<byte, BitArray> expected = new Dictionary<byte, BitArray>();
            //bool[] codeA = { true };
            bool[] codeB = { false };
            //expected.Add((byte)'A', new BitArray(codeA));
            //expected.Add((byte)'B', new BitArray(codeB));
            BitArray expected = new BitArray(codeB);

            Decode_2 temp = new Decode_2();
            //Dictionary<byte,BitArray> actual = temp.DecodeFile(ByteArrayOf_AB);
            BitArray actual = temp.DecodeFile(ByteArrayOf_AB);

            Assert.AreEqual(expected, actual);
        }

        //[Test]
        //public void ReconstructedMapOf_AB() {
        //    string input = "0101000001101000010"; //01A1B AB
        //    byte[] ByteArrayOf_AB = ByteMethods.StringToByteArray(input);

        //    Dictionary<byte, BitArray> expected = new Dictionary<byte, BitArray>();
        //    bool[] codeA = { true };
        //    bool[] codeB = { false };
        //    expected.Add((byte)'A', new BitArray(codeA));
        //    expected.Add((byte)'B', new BitArray(codeB));

        //    Decode_2 MapOf_AB = new Decode_2();

        //    Dictionary<byte, BitArray> actual = MapOf_AB.DecodeFile(ByteArrayOf_AB);

        //    CollectionAssert.AreEqual(expected, actual);
        //}
        //[Test]
        //public void ReconstructedMapOf_ABCD()
        //{
        //    string input = "00101000001101000010010100001101000100"; //001A1B01C1D
        //    byte[] ByteArrayOf_ABCD = ByteMethods.StringToByteArray(input);

        //    Dictionary<string, byte> expected = new Dictionary<string, byte>();
        //    expected.Add("00", (byte)'A');
        //    expected.Add("01", (byte)'B');
        //    expected.Add("10", (byte)'C');
        //    expected.Add("11", (byte)'D');

        //    Decode MapOf_ABCD = new Decode();

        //    Dictionary<string, byte> actual = MapOf_ABCD.DecodedFile(ByteArrayOf_ABCD);

        //    //CollectionAssert.AreEqual(expected, actual);
        //    Assert.AreEqual(expected["01"], actual["01"]);
        //}
        //[Test]
        //public void ReconstructedMapOf_ABCC()
        //{
        //    string input = "00101000001101000010101000011"; //001A1B1C
        //    byte[] ByteArrayOf_ABCC = ByteMethods.StringToByteArray(input);

        //    Dictionary<string, byte> expected = new Dictionary<string, byte>();
        //    expected.Add("00", (byte)'A');
        //    expected.Add("01", (byte)'B');
        //    expected.Add("1", (byte)'C');

        //    Decode MapOf_ABCC = new Decode();

        //    Dictionary<string, byte> actual = MapOf_ABCC.DecodedFile(ByteArrayOf_ABCC);
            
        //    CollectionAssert.AreEqual(expected, actual);
        //}
        //[Test]
        //public void ReconstructedMapOf_AABC()
        //{
        //    string input = "01010000010101000010101000011"; //001A1B1C
        //    byte[] ByteArrayOf_ABCC = ByteMethods.StringToByteArray(input);

        //    Dictionary<string, byte> expected = new Dictionary<string, byte>();
        //    expected.Add("0", (byte)'A');
        //    expected.Add("10", (byte)'B');
        //    expected.Add("11", (byte)'C');

        //    Decode MapOf_ABCC = new Decode();

        //    Dictionary<string, byte> actual = MapOf_ABCC.DecodedFile(ByteArrayOf_ABCC);

        //    CollectionAssert.AreEqual(expected, actual);
        //}

        //[Test]
        //public void ReconstructedMapOf_ABCDEFGH()
        //{
        //    string input = "00010100000110100001001010000111010001000010100010110100011001000111101001000"; //0001A1B01C1D001E1F01G1H
        //    byte[] ByteArrayOf_ABCDEFGH = ByteMethods.StringToByteArray(input);

        //    Dictionary<string, byte> expected = new Dictionary<string, byte>();
        //    expected.Add("000", (byte)'A');
        //    expected.Add("001", (byte)'B');
        //    expected.Add("010", (byte)'C');
        //    expected.Add("011", (byte)'D');
        //    expected.Add("100", (byte)'E');
        //    expected.Add("101", (byte)'F');
        //    expected.Add("110", (byte)'G');
        //    expected.Add("111", (byte)'H');

        //    Decode MapOf_ABCDEFGH = new Decode();

        //    Dictionary<string, byte> actual = MapOf_ABCDEFGH.DecodedFile(ByteArrayOf_ABCDEFGH);

        //    CollectionAssert.AreEqual(expected, actual);
        //}

        //[Test]
        //public void ReconstructedMapOf_testfile2()
        //{
        //    string input = "0000101101111010111001011110010101011101001111110000101100100100100000001011001100101100001010110001001011011011011100000101100101101101100"; //001A1B1C
        //    byte[] ByteArrayOf_ABCC = ByteMethods.StringToByteArray(input);

        //    Dictionary<string, byte> expected = new Dictionary<string, byte>();
        //    expected.Add("0000", (byte)'o');
        //    expected.Add("00010", (byte)'r');
        //    expected.Add("00011", (byte)'å');
        //    expected.Add("0010", (byte)'t');
        //    expected.Add("0011", (byte)'ø');
        //    expected.Add("010", (byte)'d');
        //    expected.Add("011", (byte)' ');
        //    expected.Add("100", (byte)'f');
        //    expected.Add("1010", (byte)'a');
        //    expected.Add("10110", (byte)'b');
        //    expected.Add("101110", (byte)'m');
        //    expected.Add("101111", (byte)'p');
        //    expected.Add("110", (byte)'e');
        //    expected.Add("111", (byte)'l');
            
        //    Decode MapOf_AB = new Decode();

        //    Dictionary<string, byte> actual = MapOf_AB.DecodedFile(ByteArrayOf_ABCC);
            
        //    Assert.AreEqual((byte)'o', actual["00001"]);
        //}

        [Test]
        public void minor_test() { //Slet
            byte expected = (byte)'A';

            byte actual = ByteMethods.BinaryStringToByte("01000001");

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void minor_test2() {
            string input = "01017";

            string expected = "0101";

            string actual = input.Remove(input.Length - 1);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void minor_test3() {
            byte[] input = { (byte)'A', (byte)'B' };

            BitArray expected = new BitArray(new byte[] {(byte)'A',(byte)'B' });

            BitArray actual = new BitArray(input);

            Assert.AreEqual(expected[0], true);
        }
        
        [Test]
        public void minor_test4() {
            byte[] input = { (byte)'A', (byte)'B', (byte)'C' };
            int i = 8; //Starter ved B

            byte expected = (byte)'B';

            BitArray bitArrayOfInput = new BitArray(input);

            byte actual = new byte();

            for(int j = i +7; j >= i; j--) {
                actual = (byte)((actual << 1) | (bitArrayOfInput[j] ? 1 : 0));
            }

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void minor_test5() { //Lav A om til B
            byte[] input = { (byte)'A' };
            int i = 0;

            byte expected = (byte)'B';

            BitArray temp = new BitArray(input);
                
            for(i = 0; i < 2; i++) {
                if(temp[i] == true) {
                    temp[i] = false;
                }
                else if(temp[i] == false) {
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
        public void minor_test6() {
            Decode_2 test = new Decode_2();
            BitArray expected = test.ConvertBinaryStringToBitArray("0001");
            bool[] code0001 = { true, true, true, false };

            BitArray actual = new BitArray(code0001);

            Assert.AreEqual(expected, actual);
        }
    }
}
