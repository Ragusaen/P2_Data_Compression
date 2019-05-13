using Compression;
using Compression.Huffman;
using NUnit.Framework;

namespace UnitTesting.Huffman {
    
    [TestFixture, Category("HuffmanEncoder")]
    public class HuffmanEncoderTest {
        public class CompressionTests {
            [Test]
            public void Compresses_ABCDEFGH() {
                var huffmanEncoder = new Compression.Huffman.HuffmanCompressor();
                byte[] inputBytes = ByteMethods.StringToByteArray("ABCDEFGH");
                DataFile input =  new DataFile(inputBytes);
                //                   FLLLS|      A   |S|      B   |LS    |   C  |    S|   D      |LLS|       E  |S|      F   |LS|      G   |S    |  H   |    |A||B||C    ||D||E||    F||G||H|
                byte[] expected = {0b10001010, 0b00001101, 0b00001001, 0b01000011, 0b10100010, 0b00010100, 0b01011010, 0b00110010, 0b10001111, 0b01001000, 0b00000101, 0b00111001, 0b01110111};

                byte[] actual = huffmanEncoder.Compress(input).GetAllBytes();
                
                Assert.AreEqual(expected, actual);
            }
        }
    }
}