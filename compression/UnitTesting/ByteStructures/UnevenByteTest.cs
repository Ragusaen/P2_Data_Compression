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

                byte[] actual = UnevenByte.UnEvenBytesToBytes(array);
            
                Assert.AreEqual(expected, actual);
            }
            [Test]
            public void ReturnsCorrectFromUnevenBytes() {
                UnevenByte[] array = {new UnevenByte(500, 9), new UnevenByte(78642,17), new UnevenByte(5, 3)};
                byte[] expected = {250, 76, 204, 168};

                byte[] actual = UnevenByte.UnEvenBytesToBytes(array);
            
                Assert.AreEqual(expected, actual);
            }
        }
        
        public class GetBitsTests {
            [Test]
            public void Returns_7_from_1111101_7_get_3() {
                byte expected = 7;
                UnevenByte ub = new UnevenByte(0b1111101,7);

                byte actual = ub.GetBits(3);
            
                Assert.AreEqual(expected, actual);
            }
            
            [Test]
            public void Returns_5_from_10101_3_get_3() {
                byte expected = 5;
                UnevenByte ub = new UnevenByte(0b10101,3);

                byte actual = ub.GetBits(3);
            
                Assert.AreEqual(expected, actual);
            }
            
            [Test]
            public void Returns_12_from_111100_4_get_4() {
                byte expected = 12;
                UnevenByte ub = new UnevenByte(0b111100,4);

                byte actual = ub.GetBits(4);

                Assert.AreEqual(expected, actual);
            }
        }

        public class ParameterizedConstructorTests {
            [Test]
            public void Returns_a_From_a_0_8() {
                byte[] input = {97};
                uint expected = 97;
                
                UnevenByte ub = new UnevenByte(input, 0, 8);
                uint actual = ub.Data;
                
                Assert.AreEqual(expected, actual);
            }
            
            [Test]
            public void ReturnsCorrectFrom2BytesWith_0_16() {
                byte[] input = {12, 230};
                uint expected = 3302;
                
                UnevenByte ub = new UnevenByte(input, 0, 16);
                uint actual = ub.Data;
                
                Assert.AreEqual(expected, actual);
            }
            
            [Test]
            public void ReturnsCorrectFrom3BytesWith_5_15() {
                byte[] input = {0b10101010, 0b00101101, 0b10110110};
                uint expected = 0b010001011011011;
                
                UnevenByte ub = new UnevenByte(input, 5, 15);
                uint actual = ub.Data;
                
                Assert.AreEqual(expected, actual);
            }
            
            [Test]
            public void ReturnsCorrectWhenBitIndexIs_7_AndLength_9() {
                byte[] input = {0b10101011, 0b10101101};
                uint expected = 0b110101101;
                
                UnevenByte ub = new UnevenByte(input, 7, 9);
                uint actual = ub.Data;
                
                Assert.AreEqual(expected, actual);
            }
        }
    }
}