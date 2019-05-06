using System.Linq;
using compression.ByteStructures;
using NUnit.Framework;
using Compression.ByteStructures;

namespace UnitTesting.ByteStructures {
    [TestFixture, Category("UnevenBytes")]
    public class UnevenByteTest {

        public class UnEvenBytesToBytesTest {
            [Test]
            public void Returns_abc_from_3_even_bytes_a_b_c() {
                UnevenByte[] array = {new UnevenByte(97, 8), new UnevenByte(98,8), new UnevenByte(99, 8)};
                byte[] expected = {97, 98, 99};
                UnevenByteConverter unevenByteConverter = new UnevenByteConverter();

                byte[] actual = unevenByteConverter.UnevenBytesToBytes(array.ToList());
            
                Assert.AreEqual(expected, actual);
            }
            [Test]
            public void ReturnsCorrectFromUnevenBytes() {
                UnevenByte[] array = {new UnevenByte(500, 9), new UnevenByte(78642,17), new UnevenByte(5, 3)};
                byte[] expected = {250, 76, 204, 168};
                UnevenByteConverter unevenByteConverter = new UnevenByteConverter();

                byte[] actual = unevenByteConverter.UnevenBytesToBytes(array.ToList());
            
                Assert.AreEqual(expected, actual);
            }
        }
        
        public class GetBitsTests {
            [Test]
            public void Returns_7_from_1111101_7_get_3() {
                int expected = 7;
                UnevenByte ub = new UnevenByte(0b1111101,7);

                int actual = ub.GetBits(3);
            
                Assert.AreEqual(expected, actual);
            }
            
            [Test]
            public void Returns_5_from_10101_3_get_3() {
                int expected = 5;
                UnevenByte ub = new UnevenByte(0b10101,3);

                int actual = ub.GetBits(3);
            
                Assert.AreEqual(expected, actual);
            }
            
            [Test]
            public void Returns_12_from_111100_4_get_4() {
                int expected = 12;
                UnevenByte ub = new UnevenByte(0b111100,4);

                int actual = ub.GetBits(4);

                Assert.AreEqual(expected, actual);
            }
        }

        public class ParameterizedConstructorTests {
            [Test]
            public void Returns_a_From_a_0_8() {
                byte[] input = {97};
                uint expected = 97;
                UnevenByteConverter unevenByteConverter = new UnevenByteConverter();

                UnevenByte ub = unevenByteConverter.CreateUnevenByteFromBytes(
                    new ArrayIndexer<byte>(input, 0, 1), 
                    8,
                    0);
                uint actual = ub.Data;
                
                Assert.AreEqual(expected, actual);
            }
            
            [Test]
            public void ReturnsCorrectFrom2BytesWith_0_16() {
                byte[] input = {12, 230};
                uint expected = 3302;
                UnevenByteConverter unevenByteConverter = new UnevenByteConverter();

                UnevenByte ub = unevenByteConverter.CreateUnevenByteFromBytes(
                    new ArrayIndexer<byte>(input, 0, 2), 
                    16,
                    0);
                uint actual = ub.Data;
                
                Assert.AreEqual(expected, actual);
            }
            
            [Test]
            public void ReturnsCorrectFrom3BytesWith_7_16() {
                byte[] input = {0b11111111, 0b00010000, 0b00010001};
                UnevenByte expected = new UnevenByte(0b10001000000010001, 17);
                UnevenByteConverter unevenByteConverter = new UnevenByteConverter();

                UnevenByte actual = unevenByteConverter.CreateUnevenByteFromBytes(
                    new ArrayIndexer<byte>(input, 0, 3), 
                    17,
                    7);
                
                Assert.AreEqual(expected, actual);
            }
            
            [Test]
            public void ReturnsCorrectFrom3BytesWith_5_15() {
                byte[] input = {0b10101010, 0b00101101, 0b10110110};
                uint expected = 0b010001011011011;
                UnevenByteConverter unevenByteConverter = new UnevenByteConverter();

                UnevenByte ub = unevenByteConverter.CreateUnevenByteFromBytes(
                    new ArrayIndexer<byte>(input, 0, 3), 
                    15,
                    5);
                uint actual = ub.Data;
                
                Assert.AreEqual(expected, actual);
            }
            
            [Test]
            public void ReturnsCorrectWhenBitIndexIs_7_AndLength_9() {
                byte[] input = {0b10101011, 0b10101101};
                uint expected = 0b110101101;
                UnevenByteConverter unevenByteConverter = new UnevenByteConverter();

                UnevenByte ub = unevenByteConverter.CreateUnevenByteFromBytes(
                    new ArrayIndexer<byte>(input, 0, 2), 
                    9,
                    7);
                uint actual = ub.Data;
                
                Assert.AreEqual(expected, actual);
            }
            
            [Test]
            public void IndexerTest() {
                UnevenByte ub = new UnevenByte(0b00110010, 6);
                int expected = 1;

                int actual = ub[0];
                    
                Assert.AreEqual(expected, actual);
            }
        }
    }
}