using NUnit.Framework;
using Compression.ByteStructures;

namespace UnitTesting.ByteStructures {
    public class UnevenByteTest {
        [Test]
        public void ByteEncoderConvert3_UnevenBits_abc() {
            UnevenByte[] array = {new UnevenByte(97, 8), new UnevenByte(98,8), new UnevenByte(99, 8)};
            byte[] expected = {97, 98, 99};

            byte[] actual = UnevenByte.UnEvenBytesToBytes(array);
            
            Assert.AreEqual(expected, actual);
        }
        [Test]
        public void ByteEncoderConvertLen_9_17_3() {
            UnevenByte[] array = {new UnevenByte(500, 9), new UnevenByte(78642,17), new UnevenByte(5, 3)};
            byte[] expected = {250, 76, 204, 168};

            byte[] actual = UnevenByte.UnEvenBytesToBytes(array);
            
            Assert.AreEqual(expected, actual);
        }
    }
}