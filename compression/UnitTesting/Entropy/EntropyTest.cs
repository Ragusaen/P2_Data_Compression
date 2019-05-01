using System;
using NUnit.Framework;
using Compression;
using Compression.Entropy;

namespace UnitTesting.Entropy {
    [TestFixture, Category("Entropy")]
    public class EntropyTest {
        public class EntropyCalcTest {
            [Test]
            public void StringFrequency() {
                var cef = new Compression.Entropy.Entropy();
                double results = cef.CalcEntropy("fem flade fldeboller p et fladt fldebollefad");
                
                const double expectedResult = 3.2540779268269016; 
                
                Assert.AreEqual(expectedResult,results);
            }
        }
    }
}
