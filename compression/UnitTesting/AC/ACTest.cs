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
            string inputPath = TestContext.CurrentContext.TestDirectory + "../../../res/flodeboller.txt";
            
            DataFile file = new DataFile(inputPath);
            ArithmeticMath ae = new ArithmeticMath(file);

            Dictionary<byte, double> testTable = ae.CalcFreq();
            Dictionary<byte, Interval> testDict = ae.SetIntervals(testTable);

            foreach (var t in testDict) {
                Console.WriteLine("[" + t.Value.low+ ", " + t.Value.high + ")" + " for :" +  (char) t.Key);
            }
            
            Assert.AreEqual(1,testDict.Values.Last().high, 0.00001);
            Assert.AreEqual(0, testDict.Values.First().low);
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
            ArithmeticEncoding ae = new ArithmeticEncoding();
            ArithmeticMath am = new ArithmeticMath(file);

            Dictionary<byte,double> testFreqTable = am.CalcFreq();
            Dictionary<byte, Interval> testIntervalDic = ae.SetIntervals(testFreqTable);

            Dictionary<Interval, byte> testTagDict = ae.CalcTag(testIntervalDic, testFreqTable);

            foreach (var b in testTagDict) {
                Console.WriteLine("[" + b.Key.low + " , " + b.Key.high + ")" + " for byte: " + b.Value);   
            }
            
            
                //Console.WriteLine("[" + t.Value.high ", " + t.Value.high + ")" + " for :" + t.Key);
            */
        }
        
        [Test]
        public void DoubleToBinary() {
            string inputPath = TestContext.CurrentContext.TestDirectory + "../../../res/flodeboller.txt";
            DataFile file = new DataFile(inputPath);
            ArithmeticMath am = new ArithmeticMath(file);

            //var strings = am.doubleToBinary();
            
            /*foreach (var b in strings) {
                Console.WriteLine(" " + b);
            }*/
        }

    }
}
