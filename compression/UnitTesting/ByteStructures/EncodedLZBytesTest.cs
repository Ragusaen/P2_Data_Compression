using NUnit.Framework;
using Compression.LZ;
using Compression.ByteStructures;

namespace UnitTesting.ByteStructures {
    public class EncodedLZByteTest {
        [TestFixture, Category("EncodedLZByte")]
        public class TestEncodedByte{
            [Test]
            public void PointerBytePointSpanIs4096WhenSize12() {
                uint expected = 4096;

                uint actual = PointerByte.GetPointerSpan();
            
                Assert.AreEqual(expected, actual);
            }

            [Test]
            public void ToUnevenBitsPointerByte_150_9_returns_67945() {
                uint expected = 67945;
                EncodedByte pb = new PointerByte(150,9);

                uint actual = pb.ToUnevenBits().Data;
            
                Assert.AreEqual(expected, actual);
            }
            [Test]
            public void ToUnevenBitsRawByte_189_returns_189() {
                uint expected = 189;
                RawByte pb = new RawByte(189);

                uint actual = pb.ToUnevenBits().Data;
            
                Assert.AreEqual(expected, actual);
            }
            [Test]
            public void GetBits_3_from_1000() {
                byte expected = 7;
                UnevenByte ub = new UnevenByte(1000,10);

                byte actual = ub.GetBits(3);
            
                Assert.AreEqual(expected, actual);
            }
        }
    }
}