using compression.AC;
using Compression.ByteStructures;
using NUnit.Framework;

namespace UnitTesting.AC {
    [TestFixture, Category("BinaryFractionInIntervalFinder")]
    public class BinaryFractionInIntervalFinderTest {
        [Test]
        public void Fraction_0_5() {
            double f = 0.5f;
            var bfiif = new BinaryFractionInIntervalFinder();
            UnevenByte expected = new UnevenByte(1, 1);

            UnevenByte actual = bfiif.FractionToUnevenByte(f);
            
            Assert.AreEqual(expected, actual);
        }
        
        [Test]
        public void Fraction_0_45() {
            double f = 0.45f;
            var bfiif = new BinaryFractionInIntervalFinder();
            UnevenByte expected = new UnevenByte(0b011100110, 9);

            UnevenByte actual = bfiif.FractionToUnevenByte(f);
            
            Assert.AreEqual(expected, actual);
        }
    }
}