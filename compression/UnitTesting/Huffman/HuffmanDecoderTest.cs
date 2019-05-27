using Compression;
using Compression.Huffman;
using NUnit.Framework;

namespace UnitTesting.Huffman {
    [TestFixture]
    [Category("HuffmanDecoder")]
    public class HuffmanDecoderTest {
        [Test]
        public void Decode_AB() {
            byte[] input = {0b11101010, 0b00001101, 0b00001001};

            byte[] expected = {(byte) 'A', (byte) 'B'};

            var huffman = new HuffmanCompressor();
            var actual = huffman.Decompress(new DataFile(input)).GetAllBytes();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Decode_ABCDEFGH() {
            byte[] input = {
                0b10001010, 0b00001101, 0b00001001, 0b01000011, 0b10100010, 0b00010100, 0b01011010,
                0b00110010, 0b10001111, 0b01001000, 0b00000101, 0b00111001, 0b01110111
            };

            byte[] expected = {
                (byte) 'A', (byte) 'B', (byte) 'C', (byte) 'D',
                (byte) 'E', (byte) 'F', (byte) 'G', (byte) 'H'
            };

            var huffman = new HuffmanCompressor();
            var actual = huffman.Decompress(new DataFile(input)).GetAllBytes();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void EncodesAndDecodes_ABABAB() {
            var expected = ByteMethods.StringToByteArray("ABABAB");
            var huffman = new HuffmanCompressor();

            var compressed = huffman.Compress(new DataFile(expected));
            var actual = huffman.Decompress(compressed).GetAllBytes();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void EncodesAndDecodes_testfile2() {
            var expected = ByteMethods.StringToByteArray("fem flade flødeboller på et fladt flødebollefad");
            var huffman = new HuffmanCompressor();

            var compressed = huffman.Compress(new DataFile(expected));
            var actual = huffman.Decompress(compressed).GetAllBytes();

            Assert.AreEqual(expected, actual);
        }
    }
}