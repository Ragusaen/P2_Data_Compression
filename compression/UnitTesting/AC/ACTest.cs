using System;
using System.Collections.Generic;
using System.Linq;
using Compression;
using Compression.PPM;
using Compression.AC;
using Compression.ByteStructures;
using NUnit.Framework;

namespace UnitTesting.AC{
    [TestFixture, Category("ArithmeticCoding")]
    public class ACTest{
        [Test]
        public void SetIntervals() {
            string inputPath = TestContext.CurrentContext.TestDirectory + "../../../res/flodeboller.txt";
            
            DataFile file = new DataFile(inputPath);
            ArithmeticMath ae = new ArithmeticMath(file);

            Dictionary<byte, double> testTable = ae.CalcFreq();
            Dictionary<Interval, byte> testDict = ae.SetIntervals(testTable);

            foreach (var t in testDict) {
                Console.WriteLine("[" + t.Key.low+ ", " + t.Key.high + ")" + " for :" +  (char) t.Value);
            }
            
            //Assert.AreEqual(1,testDict.Keys.Last().low, 0.00001);
            //Assert.AreEqual(0, testDict.Keys.First().high);
            /*string inputPath = TestContext.CurrentContext.TestDirectory + "../../../res/hcandersen.txt";
            Dictionary<double[], byte> testDict = new Dictionary<double[],byte>();
            
            DataFile file = new DataFile(inputPath);
            PredictionByPartialMatching ppm = new PredictionByPartialMatching();
            ArithmeticEncoding ae = new ArithmeticEncoding(file, 0, 100);
            
            ppm.Compress(file);
            ppm.EscapeToEnd();
                      
            testDict = ae.SetIntervals(ppm.OrderList);
            
            Assert.AreEqual(true, testDict.Count != 0);*/
        }

        [Test]
        public void CalcFreq() {
            string inputPath = TestContext.CurrentContext.TestDirectory + "../../../res/hcandersen.txt";
            DataFile file = new DataFile(inputPath);
            
            //ArithmeticEncoding ae = new ArithmeticEncoding(file, 0, 100 );
            ArithmeticMath ae = new ArithmeticMath(file);

            Dictionary<byte,double> table = ae.CalcFreq();
            double result = 0; 
            foreach (var t in table) {
                result += t.Value;
                Console.WriteLine(t.Value + " " + (char) t.Key);
            }
            
            Assert.AreEqual(1, result, 0.00001);
            Assert.AreEqual(63, table.Count);
            
        }
        
        [Test]
        public void calcTag() {
            /*string inputPath = TestContext.CurrentContext.TestDirectory + "../../../res/flodeboller.txt";
            DataFile file = new DataFile(inputPath);
            ArithmeticEncoding ae = new ArithmeticEncoding(file);
            ArithmeticMath am = new ArithmeticMath(file);

            Dictionary<byte,double> testFreqTable = am.CalcFreq();
            Dictionary<Interval, byte> testIntervalDic = am.SetIntervals(testFreqTable);

            Dictionary<Interval, byte> testTagDict = am.CalcTag();

            foreach (var b in testTagDict) {
                Console.WriteLine("[" + b.Key.low + " , " + b.Key.high + ")" + " for byte: " + (char) b.Value);
            }

            //double results = testTagDict.Keys.Last().high - testTagDict.Keys.Last().low; 
            Console.WriteLine(testTagDict.Keys.Last().high + " - " + testTagDict.Keys.Last().low + " = " );
            
            
            
                //Console.WriteLine("[" + t.Value.high ", " + t.Value.high + ")" + " for :" + t.Key);
            */
        }
        
        [Test]
        public void BinaryTag() {
            string inputPath = TestContext.CurrentContext.TestDirectory + "../../../res/flodeboller.txt";
            
            DataFile file = new DataFile(inputPath);
            ArithmeticMath ae = new ArithmeticMath(file);

            Dictionary<byte, double> testTable = ae.CalcFreq();
            Dictionary<Interval, byte> testIntervalDict = ae.SetIntervals(testTable);
            Dictionary<UnevenByte, byte> testDict = ae.UniqueBinaryTag(testIntervalDict);

            foreach (var t in testDict) {
                Console.WriteLine( t.Key + " for char: " +  (char) t.Value);
            }
        }
        
        

    }
}
