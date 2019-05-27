using Compression;
using Compression.Huffman;
using NUnit.Framework;

namespace UnitTesting.Huffman {
    [TestFixture]
    [Category("HuffmanEncoder")]
    public class HuffmanEncoderTest {
        public class CompressionTests {
            [Test]
            public void Compresses_AB() {
                byte[] input = {(byte) 'A', (byte) 'B'};
                var data = new DataFile(input);
                //                   FFFLS|     A    |S|     B    |AB
                byte[] expected = {0b11101010, 0b00001101, 0b00001001};

                var huffman = new HuffmanCompressor();
                var actual = huffman.Compress(data).GetAllBytes();

                Assert.AreEqual(expected, actual);
            }

            [Test]
            public void Compresses_ABCDEFGH() {
                var input = ByteMethods.StringToByteArray("ABCDEFGH");
                var data = new DataFile(input);
                //                   FLLLS|      A   |S|      B   |LS    |   C  |    S|   D      |LLS|       E  |S|      F   |LS|      G   |S    |  H   |    |A||B||C    ||D||E||    F||G||H|
                byte[] expected = {
                    0b10001010, 0b00001101, 0b00001001, 0b01000011, 0b10100010, 0b00010100, 0b01011010, 0b00110010,
                    0b10001111, 0b01001000, 0b00000101, 0b00111001, 0b01110111
                };

                var huffman = new HuffmanCompressor();
                var actual = huffman.Compress(data).GetAllBytes();

                Assert.AreEqual(expected, actual);
            }

            [Test]
            public void SuccessfulOnlyOneUniqueByteExpection() {
                Assert.Throws<OnlyOneUniqueByteException>(_ExceptionTestBody_AA);
            }

            private void _ExceptionTestBody_AA() {
                byte[] input = {(byte) 'A', (byte) 'A'};
                var data = new DataFile(input);

                var huffman = new HuffmanCompressor();
                huffman.Compress(data).GetAllBytes();
            }

            [Test]
            public void FailedOnlyOneUniqueByteException() {
                Assert.DoesNotThrow(_ExceptionTestBody_AB);
            }

            private void _ExceptionTestBody_AB() {
                byte[] input = {(byte) 'A', (byte) 'B'};
                var data = new DataFile(input);

                var huffman = new HuffmanCompressor();
                huffman.Compress(data).GetAllBytes();
            }
        }
    }
}