using NUnit.Framework;
using Compression;

namespace UnitTesting.Entropy {
    [TestFixture, Category("Entropy")]
    public class EntropyTest {
        public class EntropyCalcTest {
            [Test]
            public void FileEntropy() {
                string inputPath = TestContext.CurrentContext.TestDirectory + "../../../res/flodeboller.txt";
                DataFile file = new DataFile(inputPath);
                var cef = new Compression.Entropy.Entropy();
                double results = cef.CalcEntropy(file);
                
                const double expectedResult = 3.44; 
                
                Assert.AreEqual(expectedResult,results, 0.01);
            }
        }
    }
}
