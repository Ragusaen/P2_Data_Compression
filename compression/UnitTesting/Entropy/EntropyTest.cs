using System;
using NUnit.Framework;
using Compression;
using Compression.Entropy;

namespace UnitTesting.Entropy {
    [TestFixture, Category("Entropy")]
    public class EntropyTest {
        public class LogCalcTest {
            [Test]
            public void LogarithmTest() {
                double b;
                double a;
                a = 1;
                b = 2;

                const double expectedResult = 0;
                var cee = new Compression.Entropy.Entropy();
                var result = cee.LogCalc(a, b);

                Assert.AreEqual(expectedResult, actual: result);
            }
        }

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