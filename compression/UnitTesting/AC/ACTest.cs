using System;
using System.Collections.Generic;
using System.Linq;
using Compression;
using Compression.PPM;
using Compression.AC;
using Compression.Arithmetic;
using NUnit.Framework;

namespace UnitTesting.AC{
    [TestFixture, Category("ArithmeticCoding")]
    public class ACTest{
        [Test]
        public void SetIntervals() {
            string inputPath = TestContext.CurrentContext.TestDirectory + "../../../res/hcandersen.txt";
            Dictionary<double[], byte> testDict = new Dictionary<double[],byte>();
            
            DataFile file = new DataFile(inputPath);
            PredictionByPartialMatching ppm = new PredictionByPartialMatching();
            ArithmeticEncoding ae = new ArithmeticEncoding(file, 0, 100);
            
            ppm.Compress(file);
            ppm.EscapeToEnd();
                      
            testDict = ae.SetIntervals(ppm.OrderList);
            
            Assert.AreEqual(true, testDict.Count != 0);
        }

        [Test]
        public void CalcFreq() {
            string inputPath = TestContext.CurrentContext.TestDirectory + "../../../res/hcandersen.txt";
            DataFile file = new DataFile(inputPath);
            
            //ArithmeticEncoding ae = new ArithmeticEncoding(file, 0, 100 );
            var ae = new Compression.Arithmetic.ArithmeticEncoding(file, 0, 100 );

            Dictionary<byte,double> table = ae.CalcFreq(); 

            Assert.AreEqual(63, table.Count);
            
        }
    }
}
