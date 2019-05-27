using Compression.ByteStructures;
using Compression.LZ;
using NUnit.Framework;

namespace UnitTesting.ByteStructures {
    [TestFixture]
    [Category("EncodedLZByte")]
    public class TestEncodedByte {
        public class UnevenByteToEncodedByteTests {
            [Test]
            public void ReturnsPointer_17_4_From_10000000100010100() {
                var lzByteConverter = new LZByteConverter();
                var expected = new PointerByte(18, 5);
                var ub = new UnevenByte(0b10000000100010100, 17);

                var actual = lzByteConverter.ToEncodedByte(ub) as PointerByte;

                Assert.AreEqual(expected, actual);
            }

            [Test]
            public void ReturnsPointer_257_2_From_10001000000010001() {
                var lzByteConverter = new LZByteConverter();
                var expected = new PointerByte(258, 2);
                var ub = new UnevenByte(69649, 17);

                var actual = lzByteConverter.ToEncodedByte(ub) as PointerByte;

                Assert.AreEqual(expected, actual);
            }
        }

        public class ToUnevenByteTests {
            [Test]
            public void ToUnevenBytePointerByte_150_9_returns_67945() {
                var lzByteConverter = new LZByteConverter();
                uint expected = 67945;
                EncodedLZByte pb = new PointerByte(150, 9);

                var actual = lzByteConverter.ToUnevenByte(pb).Data;

                Assert.AreEqual(expected, actual);
            }

            [Test]
            public void ToUnevenByteRawByte_189_returns_189() {
                var lzByteConverter = new LZByteConverter();
                uint expected = 189;
                var pb = new RawByte(189);

                var actual = lzByteConverter.ToUnevenByte(pb).Data;

                Assert.AreEqual(expected, actual);
            }
        }

        [Test]
        public void PointerBytePointSpanIs4096WhenSize12() {
            uint expected = 4096;

            var actual = PointerByte.GetPointerSpan();

            Assert.AreEqual(expected, actual);
        }
    }
}