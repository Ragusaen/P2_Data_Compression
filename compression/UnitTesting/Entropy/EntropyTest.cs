using System;
using NUnit.Framework;
using Compression;
using Compression.Entropy;

namespace UnitTesting.Entropy
{
    [TestFixture, Category("Entropy")]
    public class EntropyTest
    {
        public class logCalcTest
        {
            [Test]
            public void logarithmTest()
            {
                double a = 0;
                double b = 2;

                double expectedResult = 0;
                double result = logCalc(a, b);

                Assert.AreEqual(expectedResult, result);
            }
        }
    }
}
