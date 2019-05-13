using Compression;
using Compression.Huffman;
using NUnit.Framework;

namespace UnitTesting.Huffman {
    
    [TestFixture, Category("HuffmanDecoder")]
    public class HuffmanDecoderTest {
        [Test]
        public void EncodesAndDecodes_ABABAB() {
            byte[] expected = ByteMethods.StringToByteArray("ABABAB");
            HuffmanCompressor huffman = new HuffmanCompressor();
            
            DataFile compressed = huffman.Compress(new DataFile(expected));
            byte[] actual = huffman.Decompress(compressed).GetAllBytes();
            
            Assert.AreEqual(expected, actual);
        }
        
        [Test]
        public void EncodesAndDecodes_testfile2() {
            byte[] expected = ByteMethods.StringToByteArray("fem flade flødeboller på et fladt flødebollefad");
            HuffmanCompressor huffman = new HuffmanCompressor();
            
            DataFile compressed = huffman.Compress(new DataFile(expected));
            byte[] actual = huffman.Decompress(compressed).GetAllBytes();
            
            Assert.AreEqual(expected, actual);
        }
    }
}