using Compression.ByteStructures;
using NUnit.Framework;

namespace UnitTesting.ByteStructures {
    [TestFixture]
    [Category("UnevenBytes")]
    public class UnevenByteTest {
        public class GetBitsTests {
            [Test]
            public void Returns_7_from_1111101_7_get_3() {
                var expected = 7;
                var ub = new UnevenByte(0b1111101, 7);

                var actual = ub.GetBits(3);

                Assert.AreEqual(expected, actual);
            }

            [Test]
            public void Returns_5_from_10101_3_get_3() {
                var expected = 5;
                var ub = new UnevenByte(0b10101, 3);

                var actual = ub.GetBits(3);

                Assert.AreEqual(expected, actual);
            }

            [Test]
            public void Returns_12_from_111100_4_get_4() {
                var expected = 12;
                var ub = new UnevenByte(0b111100, 4);

                var actual = ub.GetBits(4);

                Assert.AreEqual(expected, actual);
            }

            [Test]
            public void IndexerTest() {
                var ub = new UnevenByte(0b00110010, 6);
                var expected = 1;

                var actual = ub[0];

                Assert.AreEqual(expected, actual);
            }
        }
    }
}