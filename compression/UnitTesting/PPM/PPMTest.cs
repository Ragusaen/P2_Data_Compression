using System;
using System.Linq;
using System.Reflection;
using Compression;
using Compression.ByteStructures;
using Compression.PPM;
using Microsoft.SqlServer.Server;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace UnitTesting.PPM{
    [TestFixture, Category("PPM")]
    public class PPMTest{
        public class EntryTest{
            [Test]
            public void LowerContextInputLengthIs0() {
                byte[] input = { };
                Entry entry = new Entry(input, 11);
                int expectedLen = 0;

                entry.NextContext();
                byte[] actual = entry.Context;

                Assert.AreEqual(expectedLen, actual.Length);
            }

            [Test]
            public void LowerContextInputLengthIs1() {
                byte[] input = {11};
                Entry entry = new Entry(input, 11);
                int expectedLen = 0;

                entry.NextContext();
                byte[] actual = entry.Context;

                Assert.AreEqual(expectedLen, actual.Length);
            }

            [Test]
            public void LowerContextInputLengthIs2() {
                byte[] input = {10, 11};
                Entry entry = new Entry(input, 11);
                byte expected = 11;

                entry.NextContext();
                byte[] actual = entry.Context;

                Assert.AreEqual(expected, actual[0]);
            }

            [Test]
            public void LowerContextInputLengthIs5() {
                byte[] input = {10, 11, 12, 13, 14};
                byte[] expected = {11, 12, 13, 14};
                Entry entry = new Entry(input, 11);

                entry.NextContext();
                byte[] actual = entry.Context;

                ByteArrayComparer byteArrayComparer = new ByteArrayComparer();
                Assert.AreEqual(0, byteArrayComparer.Compare(expected, actual));
            }
        }

        public class ContextTableTest{
            [Test]
            public void UpdateCumCountIncrementsCumulativeCount() {
                byte letter = 65;
                byte letter2 = 66;
                byte[] context = {74, 68};
                SymbolList contextSymbols = new SymbolList(context);
                int expected = 1;
                ContextTable orderX = new ContextTable();
                
                orderX.UpdateContext(context, letter);
                orderX.ContextDict[contextSymbols].CalculateCumulativeCounts();
                int first = orderX.ContextDict[contextSymbols][new Letter(letter)].CumulativeCount;
                orderX.UpdateContext(context, letter2);
                orderX.ContextDict[contextSymbols].CalculateCumulativeCounts();
                int second = orderX.ContextDict[contextSymbols][new Letter(letter2)].CumulativeCount;
                int actual = second - first;
                
                Assert.AreEqual(expected, actual);
            }

            [Test]

            public void CumCountIncrementsBy1() {
                byte[] letterArray = {61, 62};
                byte[] context = {74, 68};
                SymbolList symbolList = new SymbolList(context);
                int defaultEscaping = 1;
                int expected = 1;

                ContextTable orderX = new ContextTable(defaultEscaping);
                foreach (var t in letterArray) {
                    orderX.UpdateContext(context, t);
                }

                orderX.CalculateAllCounts();

                int actual = orderX.ContextDict[symbolList][new Letter(letterArray[1])].CumulativeCount -
                              orderX.ContextDict[symbolList][new Letter(letterArray[0])].CumulativeCount;

                Assert.AreEqual(expected, actual);
            }

            [Test]
            public void UpdateCumCountWithTwoSymbolArrays() {
                byte letter = 65; // A
                byte[] context = {74, 68};
                byte[] context2 = {68, 61};
                int expected = 4 * 10;

                ContextTable orderX = new ContextTable();
                for (byte i = 0; i < 10; i++) {
                    orderX.UpdateContext(context, (byte) (letter + i));
                }

                for (int i = 0; i < 10; i++) {
                    orderX.UpdateContext(context2, (byte) (letter + i));
                }
                
                orderX.CalculateAllCounts();

                int actual = 0;

                foreach (var t in orderX.ContextDict) {
                    actual += t.Value.TotalCount;
                }
                
                PrintTable(orderX);
                
                Assert.AreEqual(expected, actual);
            }

            [Test]
            public void SymbolListHasCorrectCountsInFirstContext() {
                byte[] letterArray = {61, 62, 63, 64, 65, 66, 67, 68, 69, 70}; // 10 elements: 'a - j'
                byte[] context = {74, 68};
                byte[] context2 = {42, 77};
                byte[] context3 = {55, 22};
                
                int expected = 20;
                
                ContextTable orderX = new ContextTable();
                for (int i = 0; i < letterArray.Length; i++) {
                    orderX.UpdateContext(context, letterArray[i]);
                }

                for (int i = 0; i < letterArray.Length; i++) {
                    orderX.UpdateContext(context2, letterArray[i]);
                }

                for (int i = 0; i < letterArray.Length; i++) {
                    orderX.UpdateContext(context3, letterArray[i]);
                }

                int actual = orderX.ContextDict.First().Value.Sum(p => p.Value.Count) + orderX.ContextDict.First().Value.EscapeInfo.Count;

                PrintTable(orderX);
                
                Assert.AreEqual(expected, actual);
            }

            [Test]
            public void SymbolListHasCorrectCountsInMiddleContext() {
                byte[] letterArray = {61, 62, 63, 64, 65, 66, 67, 68, 69, 70}; // 10 elements: 'a - j'
                byte[] context1 = {74, 68};
                byte[] context2 = {42, 77};
                byte[] context3 = {55, 22};
                
                int expected = 20;
                
                ContextTable orderX = new ContextTable();
                for (int i = 0; i < letterArray.Length; i++) {
                    orderX.UpdateContext(context1, letterArray[i]);
                }

                for (int i = 0; i < letterArray.Length; i++) {
                    orderX.UpdateContext(context2, letterArray[i]);
                }

                for (int i = 0; i < letterArray.Length; i++) {
                    orderX.UpdateContext(context3, letterArray[i]);
                }
                
                SymbolList context = new SymbolList(context2);
                
                int actual = orderX.ContextDict[context].Sum(p => p.Value.Count) + orderX.ContextDict[context].EscapeInfo.Count;

                Assert.AreEqual(expected, actual);
            }

            [Test]
            public void SymbolListHasCorrectCountsInLastContext() {
                byte[] letterArray = {61, 62, 63, 64, 65, 66, 67, 68, 69, 70}; // 10 elements: 'a - j'
                byte[] context1 = {74, 68};
                byte[] context2 = {42, 77};
                byte[] context3 = {55, 22};

                int expected = 20;

                ContextTable orderX = new ContextTable();
                for (int i = 0; i < letterArray.Length; i++) {
                    orderX.UpdateContext(context1, letterArray[i]);
                }

                for (int i = 0; i < letterArray.Length; i++) {
                    orderX.UpdateContext(context2, letterArray[i]);
                }

                for (int i = 0; i < letterArray.Length; i++) {
                    orderX.UpdateContext(context3, letterArray[i]);
                }
                
                SymbolList context = new SymbolList(context3);
                
                int actual = orderX.ContextDict[context].Sum(p => p.Value.Count) + orderX.ContextDict[context].EscapeInfo.Count;

                Assert.AreEqual(expected, actual);
            }

            [Test]
            public void FirstContextAddsToTable() {
                byte[] context = {74, 68};
                byte symbol = 103;

                ContextTable ct = new ContextTable();

                ct.UpdateContext(context, symbol);

                Assert.IsTrue(ct.ContextDict.ContainsKey(new SymbolList(context)));
            }

            [Test]
            public void SecondContextAddsToTable() {
                byte[] context = {74, 68};
                byte[] context2 = {70, 69};
                byte symbol = 103;
                ContextTable ct = new ContextTable();

                ct.UpdateContext(context, symbol);
                ct.UpdateContext(context2, symbol);

                Assert.IsTrue(ct.ContextDict.ContainsKey(new SymbolList(context2)));
            }

            [Test]
            public void NoContextAddsFirstSymbolToTable() {
                byte symbol = 103;
                ContextTable ct = new ContextTable();

                ct.UpdateContext(new byte[0], symbol);

                Assert.IsTrue(ct.ContextDict[new SymbolList(new byte[0])].ContainsKey(new Letter(symbol)));
            }

            [Test]
            public void NoContextAddsSecondSymbolToTable() {
                byte symbol = 103;
                byte symbol2 = 104;
                ContextTable ct = new ContextTable();

                ct.UpdateContext(new byte[0], symbol);
                ct.UpdateContext(new byte[0], symbol2);

                Assert.IsTrue(ct.ContextDict[new SymbolList(new byte[0])].ContainsKey(new Letter(symbol2)));
            }

            [Test]
            public void NoContextAddsFifthSymbolToTable() {
                byte symbol = 100; // d -> expected = 104
                ContextTable ct = new ContextTable(1);


                for (int i = 0; i <= 5; i++)
                    ct.UpdateContext(new byte[0], (byte) (symbol + i));

                Assert.IsTrue(ct.ContextDict[new SymbolList(new byte[0])].ContainsKey(new Letter(104)));
            }
        }

        public class TablesTest{
            [Test]
            public void InitializesCorrectNumberOfTables() {
                DataFile file = new DataFile();
                int expected = 7;
                
                PredictionByPartialMatching ppm = new PredictionByPartialMatching();
                ppm.Compress(file);
                int actual = ppm.OrderList.Count;
                
                Assert.AreEqual(expected, actual);
            }
            [Test]
            public void CorrectNumberOfSymbolsInMinusFirstOrderFromTestFile1() {
                string inputPath = TestContext.CurrentContext.TestDirectory + "../../../res/testfile1";
                DataFile file = new DataFile(inputPath);
                PredictionByPartialMatching ppm = new PredictionByPartialMatching();
                int expected = 3;

                ppm.Compress(file);
                int actual = ppm.OrderList[0].ContextDict.Sum(p => p.Value.Count);
                
                Assert.AreEqual(expected, actual);
            }
            [Test]
            public void CorrectNumberOfSymbolsInMinusFirstOrderFromTestFile2() {
                string inputPath = TestContext.CurrentContext.TestDirectory + "../../../res/testfile2";
                DataFile file = new DataFile(inputPath);
                PredictionByPartialMatching ppm = new PredictionByPartialMatching();
                int expected = 15;
                
                ppm.Compress(file);
                int actual = ppm.OrderList[0].ContextDict.Sum(p => p.Value.Count);
                
                Assert.AreEqual(expected, actual);
            }
            [Test]
            public void CorrectNumberOfSymbolsInMinusFirstOrderFromTestFile3() {
                string inputPath = TestContext.CurrentContext.TestDirectory + "../../../res/testfile3";
                DataFile file = new DataFile(inputPath);
                PredictionByPartialMatching ppm = new PredictionByPartialMatching();
                int expected = 4;
                
                ppm.Compress(file);
                int actual = ppm.OrderList[0].ContextDict.Sum(p => p.Value.Count);

                Assert.AreEqual(expected, actual);
            }
            [Test]
            public void CorrectNumberOfSymbolsInZerothOrderFromTestFile1() {
                string inputPath = TestContext.CurrentContext.TestDirectory + "../../../res/testfile1";
                DataFile file = new DataFile(inputPath);
                PredictionByPartialMatching ppm = new PredictionByPartialMatching();
                int expected = 4;
                int actual = 0;
                SymbolList context = new SymbolList();
                
                ppm.Compress(file);
                if (ppm.OrderList[1].ContextDict[context].EscapeInfo.Count > 0)
                    actual += 1;
                
                actual += ppm.OrderList[1].ContextDict[context].Count;
                
                ContextTablePrinter ctp = new ContextTablePrinter();
                ctp.ConsolePrintAll(ppm);
                
                Assert.AreEqual(expected, actual);
            }
            [Test]
            public void CorrectTotalCountInZerothOrder() {
                string inputPath = TestContext.CurrentContext.TestDirectory + "../../../res/testfile3";
                DataFile file = new DataFile(inputPath);
                int expected = 9;
                SymbolList context = new SymbolList();
                
                PredictionByPartialMatching PPM = new PredictionByPartialMatching();
                PPM.Compress(file);

                
                PPM.OrderList[1].CalculateAllCounts();

                int actual = PPM.OrderList[1].ContextDict.Values.Sum(p => p.TotalCount);
                
                Assert.AreEqual(expected, actual);
            }
            [Test]
            public void AddsFirstSymbolToMinusFirstOrder() {
                string inputPath = TestContext.CurrentContext.TestDirectory + "../../../res/testfile1";
                DataFile file = new DataFile(inputPath);
                byte expected = 97; // 'a'
                
                PredictionByPartialMatching ppm = new PredictionByPartialMatching();
                ppm.Compress(file);
                                
                Assert.IsTrue(ppm.OrderList[0].ContextDict[new SymbolList(new byte[0])].ContainsKey(new Letter(expected)));
            }
            [Test]
            public void AddsSecondSymbolToMinusFirstOrder() {
                string inputPath = TestContext.CurrentContext.TestDirectory + "../../../res/testfile1";
                DataFile file = new DataFile(inputPath);
                PredictionByPartialMatching ppm = new PredictionByPartialMatching();
                byte expected = 98; // 'b'
                
                ppm.Compress(file);
                
                Assert.IsTrue(ppm.OrderList[0].ContextDict[new SymbolList(new byte[0])].ContainsKey(new Letter(expected)));
            }
            [Test]
            public void AddsSymbolZerothOrder() {
                string inputPath = TestContext.CurrentContext.TestDirectory + "../../../res/testfile1";
                DataFile file = new DataFile(inputPath);
                PredictionByPartialMatching ppm = new PredictionByPartialMatching();
                byte expected = 97; // 'a'
                
                ppm.Compress(file);
                
                Assert.IsTrue(ppm.OrderList[1].ContextDict[new SymbolList(new byte[0])].ContainsKey(new Letter(expected)));
            }
            [Test]
            public void AddsSymbolCInSecondContextFromAbcFile() {
                string inputPath = TestContext.CurrentContext.TestDirectory + "../../../res/testfile1";
                DataFile file = new DataFile(inputPath);
                PredictionByPartialMatching ppm = new PredictionByPartialMatching();
                byte expected = 99; // c
                
                ppm.Compress(file);
                
                Assert.IsTrue(ppm.OrderList[3].ContextDict[new SymbolList(new byte[] {97, 98})].ContainsKey(new Letter(expected)));

            }
            [Test]
            public void AddsContextAbInSecondContextFromAbcFile() {
                string inputPath = TestContext.CurrentContext.TestDirectory + "../../../res/testfile1";
                DataFile file = new DataFile(inputPath);
                PredictionByPartialMatching ppm = new PredictionByPartialMatching();
                byte[] expected = {97, 98}; // ab
                
                ppm.Compress(file);
                
                Assert.IsTrue(ppm.OrderList[3].ContextDict.ContainsKey(new SymbolList(expected)));
            }
            [Test]
            public void AddsCorrectNumberOfContextsInSecondOrder() {
                string inputPath = TestContext.CurrentContext.TestDirectory + "../../../res/testfile3";
                DataFile file = new DataFile(inputPath);
                PredictionByPartialMatching ppm = new PredictionByPartialMatching();
                int expected = 4;
                
                ppm.Compress(file);
                int actual = ppm.OrderList[3].ContextDict.Count;
                
                Assert.AreEqual(expected, actual);
            }
            [Test]
            public void SecondOrderHasCorrectTotalCount() {
                string inputPath = TestContext.CurrentContext.TestDirectory + "../../../res/testfile3";
                DataFile file = new DataFile(inputPath);
                PredictionByPartialMatching ppm = new PredictionByPartialMatching();
                int expected = 9; // abc:1    d:2   <esc>:4
                SymbolList context = new SymbolList();
                
                ppm.Compress(file);
                ppm.OrderList[3].CalculateAllCounts();

                int actual = ppm.OrderList[3].ContextDict.Sum(p => p.Value.TotalCount);
                
                Assert.AreEqual(expected, actual);
            }
        }

        public class InputOrderTest{
            [Test]
            public void InputOrderIsZeroHasCorrectTotalCountInMinusFirstOrder() {
                string inputPath = TestContext.CurrentContext.TestDirectory + "../../../res/testfile2";
                DataFile file = new DataFile(inputPath);
                int expected = 15;
                int actual = 0;

                PredictionByPartialMatching ppm = new PredictionByPartialMatching();
                ppm.Compress(file);
                ppm.OrderList[0].CalculateAllCounts();
                foreach (var t in ppm.OrderList[0].ContextDict.Values) {
                    actual += t.TotalCount;
                }
                                
                PrintTable(ppm.OrderList[0]);

                Assert.AreEqual(expected, actual);
            }
            [Test]
            public void InputOrderIsZeroHasCorrectTotalCountInZerothOrder() {
                string inputPath = TestContext.CurrentContext.TestDirectory + "../../../res/testfile2";
                DataFile file = new DataFile(inputPath);
                int expected = 50 + 15; // 50 from symbols + 15 from <esc>
                int actual = 0;


                PredictionByPartialMatching ppm = new PredictionByPartialMatching(0);
                ppm.Compress(file);
                ppm.OrderList[1].CalculateAllCounts();
                foreach (var t in ppm.OrderList[1].ContextDict.Values) {
                    actual += t.TotalCount;
                }
                
                Assert.AreEqual(expected, actual);
            }
            [Test]
            public void InputOrderIsOneHasCorrectTotalCountInZerothOrder() {
                string inputPath = TestContext.CurrentContext.TestDirectory + "../../../res/testfile2";
                DataFile file = new DataFile(inputPath);
                int expected = 30 + 15;
                int actual = 0;

                PredictionByPartialMatching ppm = new PredictionByPartialMatching();
                ppm.Compress(file);
                ppm.OrderList[1].CalculateAllCounts();
                foreach (var t in ppm.OrderList[1].ContextDict.Values) {
                    actual += t.TotalCount;
                }

                Assert.AreEqual(expected, actual);
            }
            [Test]
            public void InputOrderIsOneHasCorrectTotalCountInFirstOrder() {
                string inputPath = TestContext.CurrentContext.TestDirectory + "../../../res/testfile2";
                DataFile file = new DataFile(inputPath);
                int expected = 49 + 29;

                PredictionByPartialMatching ppm = new PredictionByPartialMatching(1);
                ppm.Compress(file);
                ppm.OrderList[2].CalculateAllCounts();
                int actual = 0;
                foreach (var t in ppm.OrderList[2].ContextDict.Values) {
                    actual += t.TotalCount;
                }
                
                Assert.AreEqual(expected, actual);
            }
        }
        public static void PrintPPM(PredictionByPartialMatching t) {
            ContextTablePrinter ctp = new ContextTablePrinter();
            ctp.ConsolePrintAll(t);
        }
        public static void PrintTable(ContextTable t) {
            ContextTablePrinter ctp = new ContextTablePrinter();
            ctp.ConsolePrint(t);
        }
    }
}
