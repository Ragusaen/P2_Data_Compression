using System;
using System.CodeDom;
using System.Reflection;
using NUnit.Framework;
using Compression.LZ;
using Compression.ByteStructures;

namespace UnitTesting.ByteStructures {
    [TestFixture, Category("EncodedLZByte")]
    public class TestEncodedByte{
        [Test]
        public void PointerBytePointSpanIs4096WhenSize12() {
            uint expected = 4096;

            uint actual = PointerByte.GetPointerSpan();
            
            Assert.AreEqual(expected, actual);
        }

        public class UnevenByteToEncodedByteTests {
            [Test]
            public void ReturnsPointer_17_4_From_10000000100010100() {
                PointerByte expected = new PointerByte(18, 5);
                UnevenByte ub = new UnevenByte(0b10000000100010100, 17);
                
                PointerByte actual = expected.UnevenByteToEncodedByte(ub) as PointerByte;
                
                Assert.AreEqual(expected.Length, actual.Length);
            }
        }

        public class ToUnevenByteTests {
            [Test]
            public void ToUnevenBytePointerByte_150_9_returns_67945() {
                uint expected = 67945;
                EncodedByte pb = new PointerByte(150,9);

                uint actual = pb.ToUnevenByte().Data;
            
                Assert.AreEqual(expected, actual);
            }
            
            [Test]
            public void ToUnevenByteRawByte_189_returns_189() {
                uint expected = 189;
                RawByte pb = new RawByte(189);

                uint actual = pb.ToUnevenByte().Data;
            
                Assert.AreEqual(expected, actual);
            }
        }

        public class GetUnevenByteLengthTests {
            [Test]
            public void Returns_17_FromByte_10000000() {
                byte b = 0b10000000;
                uint expected = 17;
    
                uint actual = (new PointerByte(0,0)).GetUnevenByteLength(b);
                
                Assert.AreEqual(expected, actual);
            }
            
            [Test]
            public void Returns_17_FromByte_10010111() {
                byte b = 0b10010111;
                uint expected = 17;
    
                uint actual = (new PointerByte(0,0)).GetUnevenByteLength(b);
                
                Assert.AreEqual(expected, actual);
            }
            
            [Test]
            public void Returns_9_FromByte_01110011() {
                byte b = 0b01110011;
                uint expected = 9;
    
                uint actual = (new PointerByte(0,0)).GetUnevenByteLength(b);
                
                Assert.AreEqual(expected, actual);
            }
            
            [Test]
            public void Returns_9_FromByte_01101110() {
                byte b = 0b01101110;
                uint expected = 9;
    
                uint actual = (new PointerByte(0,0)).GetUnevenByteLength(b);
                
                Assert.AreEqual(expected, actual);
            }
        }
    }
}