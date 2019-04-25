using System;
using System.Reflection;
using Compression;
using Compression.PPM;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace UnitTesting.PPM{
    [TestFixture, Category("PPM")]
    public class PPMTest{
        public class SymbolTest{
            [Test]
            public void NewSymbolNoParamsHasTypeOfEscapeSymbol() {
                EscapeSymbol expected = new EscapeSymbol();

                SymbolData actual = new Symbol().Data;

                Assert.IsInstanceOf<EscapeSymbol>(actual);
            }
            [Test]
            public void NewSymbolWithParamsHasCorrectData() {
                Letter expected = new Letter(25);

                SymbolData actual = new Symbol(25).Data;

                Assert.IsInstanceOf<Letter>(actual);
            }
        }

        public class ContextTest{
            [Test]
            public void NewSymbolAddsToList() {
                byte letter = 65;
                byte[] context = {74, 68};
                byte expected = 65;

                Context a = new Context(context);
                a.SymbolList.Add(new Symbol()); // Sets first element as Escape Symbol
                a.Update(letter);
                byte actual = ((Letter) a.SymbolList[1].Data).Data;

                Assert.AreEqual(expected, actual);
            }

            [Test]
            public void NewSymbolCountIsOne() {
                byte letter = 65;
                byte[] context = {74, 68};
                int expected = 1;

                Context a = new Context(context);
                a.SymbolList.Add(new Symbol()); // Sets first element as Escape Symbol
                a.Update(letter);
                int actual = a.SymbolList[1].Count;

                Assert.AreEqual(expected, actual);
            }

            [Test]
            public void UpdateNoPreviousContextAddsNewContext() {
                byte letter = 65;
                byte[] context = {74, 68};
                byte[] expected = {74, 68};

                Context a = new Context(context);
                a.SymbolList.Add(new Symbol()); // Sets first element as Escape Symbol
                a.Update(letter);
                byte[] actual = a.ContextBytes;

                ByteArrayComparer byteArrayComparer = new ByteArrayComparer();
                Assert.AreEqual(0, byteArrayComparer.Compare(expected, actual));
            }

            [Test]
            public void UpdatePreviousContextUpdatesContextWithNewSymbol() {
                byte letter = 65;
                byte letter2 = 66;
                byte[] context = {74, 68};
                byte expected = 66;

                Context a = new Context(context);
                a.SymbolList.Add(new Symbol()); // Sets first element as Escape Symbol
                a.Update(letter);
                a.Update(letter2);
                byte actual = ((Letter) a.SymbolList[2].Data).Data;

                Assert.AreEqual(expected, actual);
            }

            [Test]
            public void UpdatePreviousContextIncrementsSymbolCount() {
                byte letter = 65;
                byte[] context = {74, 68};
                int expected = 2;

                Context a = new Context(context);
                a.SymbolList.Add(new Symbol()); // Sets first element as Escape Symbol
                a.Update(letter);
                a.Update(letter);
                int actual = a.SymbolList[1].Count;

                Assert.AreEqual(expected, actual);
            }
        }
        public class EntryTest {
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
            public void UpdatePreviousContextIncrementsTotalCountBy1() {
                byte letter = 65; 
                byte[] context = {74, 68};
                int defaultEscaping = 1;
                int expected = 1;
                
                ContextTable orderX = new ContextTable(defaultEscaping);
                orderX.UpdateContext(context, letter);
                orderX.CalculateTotalCount();
                int first = orderX.TotalCount;
                orderX.UpdateContext(context, letter);
                orderX.CalculateTotalCount();
                int second = orderX.TotalCount;
                int actual = second - first;
                
                Assert.AreEqual(expected, actual);
            }

            [Test]
            public void UpdateCumCountIncrementsCumulativeCount() {
                byte letter = 65; 
                byte letter2 = 61;
                byte[] context = {74, 68}; 
                int defaultEscaping = 1;
                int expected = 4 + defaultEscaping;
                
                ContextTable orderX = new ContextTable(defaultEscaping);
                orderX.UpdateContext(context, letter);
                orderX.UpdateContext(context, letter2);
                orderX.UpdateCumulativeCount();
                int actual = orderX.ContextList[0].SymbolList[2].CumulativeCount;

                Assert.AreEqual(expected, actual);
            }
            [Test]
            public void UpdateCumCountIncrementsBy1() {
                byte[] letterArray = {61, 62}; 
                byte[] context = {74, 68}; 
                int defaultEscaping = 1;
                int expected = 1;
                
                ContextTable orderX = new ContextTable(defaultEscaping);
                for (int i = 0; i < letterArray.Length; i++) {
                    orderX.UpdateContext(context, letterArray[i]);
                }
                orderX.UpdateCumulativeCount();
                int actual = orderX.ContextList[0].SymbolList[2].CumulativeCount - orderX.ContextList[0].SymbolList[1].CumulativeCount;

                Assert.AreEqual(expected, actual);
            }
            [Test]
            public void UpdateCumCountWithTwoSymbolArrays() {
                byte[] letterArray = {61, 62, 63, 64, 65, 66, 67, 68, 69, 70}; // 10 elements: 'a - j'
                byte[] context = {74, 68}; 
                byte[] context2 = {68, 61}; 
                int defaultEscaping = 1;
                int expected = (int)letterArray.Length * 4 + defaultEscaping;
                
                ContextTable orderX = new ContextTable(defaultEscaping);
                for (int i = 0; i < letterArray.Length; i++) {
                    orderX.UpdateContext(context, letterArray[i]);
                }
                for (int i = 0; i < letterArray.Length; i++) {
                    orderX.UpdateContext(context2, letterArray[i]);
                }

                orderX.UpdateCumulativeCount();
                int actual = orderX.ContextList[1].SymbolList[9].CumulativeCount;

                Assert.AreEqual(expected, actual);
            }
            [Test]
            public void SymbolListHasCorrectTotalCountInFirstContext() {
                byte[] letterArray = {61, 62, 63, 64, 65, 66, 67, 68, 69, 70}; // 10 elements: 'a - j'
                byte[] context = {74, 68}; 
                byte[] context2 = {42, 77}; 
                byte[] context3 = {55, 22};
                int defaultEscaping = 1;
                int expected = 20 + defaultEscaping;
                
                ContextTable orderX = new ContextTable(defaultEscaping);
                for (int i = 0; i < letterArray.Length; i++) {
                    orderX.UpdateContext(context, letterArray[i]);
                }
                for (int i = 0; i < letterArray.Length; i++) {
                    orderX.UpdateContext(context2, letterArray[i]);
                }
                for (int i = 0; i < letterArray.Length; i++) {
                    orderX.UpdateContext(context3, letterArray[i]);
                }
                int actual = 0;
                for (int i = 0; i < orderX.ContextList[0].SymbolList.Count; i++) {
                    actual += orderX.ContextList[0].SymbolList[i].Count;
                }
                
                Assert.AreEqual(expected, actual);
            }
            [Test]
            public void SymbolListHasCorrectTotalCountInMiddleContext() {
                byte[] letterArray = {61, 62, 63, 64, 65, 66, 67, 68, 69, 70}; // 10 elements: 'a - j'
                byte[] context = {74, 68}; 
                byte[] context2 = {42, 77}; 
                byte[] context3 = {55, 22};
                int defaultEscaping = 1;
                int expected = 20 + defaultEscaping;
                
                ContextTable orderX = new ContextTable(defaultEscaping);
                for (int i = 0; i < letterArray.Length; i++) {
                    orderX.UpdateContext(context, letterArray[i]);
                }
                for (int i = 0; i < letterArray.Length; i++) {
                    orderX.UpdateContext(context2, letterArray[i]);
                }
                for (int i = 0; i < letterArray.Length; i++) {
                    orderX.UpdateContext(context3, letterArray[i]);
                }
                int actual = 0;
                for (int i = 0; i < orderX.ContextList[0].SymbolList.Count; i++) {
                    actual += orderX.ContextList[1].SymbolList[i].Count;
                }
                
                Assert.AreEqual(expected, actual);
            }
            [Test]
            public void SymbolListHasCorrectTotalCountInLastContext() {
                byte[] letterArray = {61, 62, 63, 64, 65, 66, 67, 68, 69, 70}; // 10 elements: 'a - j'
                byte[] context = {74, 68}; 
                byte[] context2 = {42, 77}; 
                byte[] context3 = {55, 22};
                int defaultEscaping = 1;
                int expected = 20 + defaultEscaping;
                
                ContextTable orderX = new ContextTable(defaultEscaping);
                for (int i = 0; i < letterArray.Length; i++) {
                    orderX.UpdateContext(context, letterArray[i]);
                }
                for (int i = 0; i < letterArray.Length; i++) {
                    orderX.UpdateContext(context2, letterArray[i]);
                }
                for (int i = 0; i < letterArray.Length; i++) {
                    orderX.UpdateContext(context3, letterArray[i]);
                }
                int actual = 0;
                for (int i = 0; i < orderX.ContextList[0].SymbolList.Count; i++) {
                    actual += orderX.ContextList[2].SymbolList[i].Count;
                }
                
                Assert.AreEqual(expected, actual);
            }
            [Test]
            public void FirstContextAddsToTable() {
                byte[] context = {74, 68};
                byte symbol = 103;
                int defaultEscaping = 1;
                ContextTable CT = new ContextTable(defaultEscaping);

                CT.UpdateContext(context, symbol);
                byte[] actual = CT.ContextList[0].ContextBytes;
                
                ByteArrayComparer BAC = new ByteArrayComparer();
                Assert.AreEqual(0, BAC.Compare(context,actual));
            }
            [Test]
            public void SecondContextAddsToTable() {
                byte[] context = {74, 68};
                byte[] context2 = {70, 69};
                byte symbol = 103;
                ContextTable CT = new ContextTable(1);

                CT.UpdateContext(context, symbol);
                CT.UpdateContext(context2, symbol);
                byte[] actual = CT.ContextList[1].ContextBytes;
                
                ByteArrayComparer BAC = new ByteArrayComparer();
                Assert.AreEqual(0, BAC.Compare(context2,actual));
            }
            [Test]
            public void NoContextAddsFirstSymbolToTable() {
                byte symbol = 103;
                ContextTable CT = new ContextTable(1);

                CT.UpdateContext(new byte[0], symbol);
                byte actual = ((Letter)CT.ContextList[0].SymbolList[1].Data).Data;
                
                Assert.AreEqual(symbol, actual);
            }
            [Test]
            public void NoContextAddsSecondSymbolToTable() {
                byte symbol = 103;
                byte symbol2 = 104;
                ContextTable CT = new ContextTable(1);

                CT.UpdateContext(new byte[0], symbol);
                CT.UpdateContext(new byte[0], symbol2);
                byte actual = ((Letter)CT.ContextList[0].SymbolList[2].Data).Data;
                
                Assert.AreEqual(symbol2, actual);
            }
            [Test]
            public void NoContextAddsFifthSymbolToTable() {
                byte symbol = 100; // d
                byte expected = 104;
                ContextTable CT = new ContextTable(1);


                for (int i = 0; i <= 5; i++)
                    CT.UpdateContext(new byte[0], (byte)(symbol+i));    
                byte actual = ((Letter)CT.ContextList[0].SymbolList[5].Data).Data;
                Assert.AreEqual(expected, actual);
            }
        }

        public class TablesTest{
            [Test]
            public void InitializesCorrectNumberOfTables() {
                DataFile file = new DataFile();
                int expected = 7;
                
                PredictionByPartialMatching PPM = new PredictionByPartialMatching();
                PPM.Compress(file);
                int actual = PPM.OrderList.Count;
                
                Assert.AreEqual(expected, actual);
            }
            [Test]
            public void CorrectNumberOfSymbolsInMinusFirstOrderFromTestFile1() {
                string inputPath = TestContext.CurrentContext.TestDirectory + "../../../res/testfile1";
                DataFile file = new DataFile(inputPath);
                int expected = 3;
                
                PredictionByPartialMatching PPM = new PredictionByPartialMatching();
                PPM.Compress(file);
                int actual = PPM.OrderList[0].ContextList[0].SymbolList.Count;
                
                Assert.AreEqual(expected, actual);
            }
            [Test]
            public void CorrectNumberOfSymbolsInMinusFirstOrderFromTestFile2() {
                string inputPath = TestContext.CurrentContext.TestDirectory + "../../../res/testfile2";
                DataFile file = new DataFile(inputPath);
                int expected = 15;
                
                PredictionByPartialMatching PPM = new PredictionByPartialMatching();
                PPM.Compress(file);
                int actual = PPM.OrderList[0].ContextList[0].SymbolList.Count;
                
                Assert.AreEqual(expected, actual);
            }
            [Test]
            public void CorrectNumberOfSymbolsInMinusFirstOrderFromTestFile3() {
                string inputPath = TestContext.CurrentContext.TestDirectory + "../../../res/testfile3";
                DataFile file = new DataFile(inputPath);
                int expected = 4;
                
                PredictionByPartialMatching PPM = new PredictionByPartialMatching();
                PPM.Compress(file);
                int actual = PPM.OrderList[0].ContextList[0].SymbolList.Count;
                
                Assert.AreEqual(expected, actual);
            }
            [Test]
            public void CorrectNumberOfSymbolsInZerothOrderFromTestFile1() {
                string inputPath = TestContext.CurrentContext.TestDirectory + "../../../res/testfile1";
                DataFile file = new DataFile(inputPath);
                int expected = 4;
                
                PredictionByPartialMatching PPM = new PredictionByPartialMatching();
                PPM.Compress(file);
                int actual = PPM.OrderList[1].ContextList[0].SymbolList.Count;
                
                Assert.AreEqual(expected, actual);
            }
            [Test]
            public void CorrectTotalCountInZerothOrder() {
                string inputPath = TestContext.CurrentContext.TestDirectory + "../../../res/testfile3";
                DataFile file = new DataFile(inputPath);
                int expected = 9;
                
                PredictionByPartialMatching PPM = new PredictionByPartialMatching();
                PPM.Compress(file);
                PPM.OrderList[1].CalculateTotalCount();
                int actual = PPM.OrderList[1].TotalCount;
                
                Assert.AreEqual(expected, actual);
            }
            [Test]
            public void AddsFirstSymbolToMinusFirstOrder() {
                string inputPath = TestContext.CurrentContext.TestDirectory + "../../../res/testfile1";
                DataFile file = new DataFile(inputPath);
                byte expected = 97; // 'a'
                
                PredictionByPartialMatching PPM = new PredictionByPartialMatching();
                PPM.Compress(file);
                byte actual = ((Letter)PPM.OrderList[0].ContextList[0].SymbolList[0].Data).Data;
                
                Assert.AreEqual(expected, actual);
            }
            [Test]
            public void AddsSecondSymbolToMinusFirstOrder() {
                string inputPath = TestContext.CurrentContext.TestDirectory + "../../../res/testfile1";
                DataFile file = new DataFile(inputPath);
                byte expected = 98; // 'b'
                
                PredictionByPartialMatching PPM = new PredictionByPartialMatching();
                PPM.Compress(file);
                byte actual = ((Letter)PPM.OrderList[0].ContextList[0].SymbolList[1].Data).Data;
                
                Assert.AreEqual(expected, actual);
            }
            [Test]
            public void AddsSymbolZerothOrder() {
                string inputPath = TestContext.CurrentContext.TestDirectory + "../../../res/testfile1";
                DataFile file = new DataFile(inputPath);
                byte expected = 97; // 'a'
                
                PredictionByPartialMatching PPM = new PredictionByPartialMatching();
                PPM.Compress(file);
                byte actual = ((Letter)PPM.OrderList[1].ContextList[0].SymbolList[1].Data).Data;
                
                Assert.AreEqual(expected, actual);
            }
            [Test]
            public void AddsSymbolCInSecondContextFromAbcFile() {
                string inputPath = TestContext.CurrentContext.TestDirectory + "../../../res/testfile1";
                DataFile file = new DataFile(inputPath);
                byte expected = 99; // c
                
                PredictionByPartialMatching PPM = new PredictionByPartialMatching();
                PPM.Compress(file);
                byte actual = ((Letter)PPM.OrderList[3].ContextList[0].SymbolList[1].Data).Data;
                
                Assert.AreEqual(expected, actual);
            }
            [Test]
            public void AddsContextAbInSecondContextFromAbcFile() {
                string inputPath = TestContext.CurrentContext.TestDirectory + "../../../res/testfile1";
                DataFile file = new DataFile(inputPath);
                byte[] expected = {97, 98}; // ab
                
                PredictionByPartialMatching PPM = new PredictionByPartialMatching();
                PPM.Compress(file);
                byte[] actual = PPM.OrderList[3].ContextList[0].ContextBytes;
                Assert.AreEqual(expected, actual);
            }
            [Test]
            public void AddsCorrectNumberOfContextsInSecondOrder() {
                string inputPath = TestContext.CurrentContext.TestDirectory + "../../../res/testfile3";
                DataFile file = new DataFile(inputPath);
                int expected = 4;
                
                PredictionByPartialMatching PPM = new PredictionByPartialMatching();
                PPM.Compress(file);
                int actual = PPM.OrderList[3].ContextList.Count;
                
                Assert.AreEqual(expected, actual);
            }
            [Test]
            public void SecondOrderHasCorrectTotalCount() {
                string inputPath = TestContext.CurrentContext.TestDirectory + "../../../res/testfile3";
                DataFile file = new DataFile(inputPath);
                int expected = 9; // abc:1    d:2   <esc>:4
                
                PredictionByPartialMatching PPM = new PredictionByPartialMatching();
                PPM.Compress(file);
                PPM.OrderList[3].CalculateTotalCount();
                int actual = (int)PPM.OrderList[3].TotalCount;
                
                Assert.AreEqual(expected, actual);
            }
            [Test]
            public void PrintTables() {
                string inputPath = TestContext.CurrentContext.TestDirectory + "../../../res/testfile2";
                DataFile file = new DataFile(inputPath);
                
                PredictionByPartialMatching PPM = new PredictionByPartialMatching();
                PPM.Compress(file);
                
                ContextTablePrinter CTP = new ContextTablePrinter();
                CTP.ConsolePrint(PPM.OrderList[0]);
                
                Assert.AreEqual(true,true);
            }
        }

        public class InputOrderTest{
            [Test]
            public void InputOrderIsZeroHasCorrectTotalCountInMinusFirstOrder() {
                string inputPath = TestContext.CurrentContext.TestDirectory + "../../../res/testfile2";
                DataFile file = new DataFile(inputPath);
                int expected = 15;

                PredictionByPartialMatching PPM = new PredictionByPartialMatching();
                PPM.Compress(file);
                PPM.OrderList[0].CalculateTotalCount();
                int actual = (int)PPM.OrderList[0].TotalCount;

                Assert.AreEqual(expected, actual);
            }
            [Test]
            public void InputOrderIsZeroHasCorrectTotalCountInZerothOrder() {
                string inputPath = TestContext.CurrentContext.TestDirectory + "../../../res/testfile2";
                DataFile file = new DataFile(inputPath);
                int expected = 50 + 15; // 50 from symbols + 15 from <esc>

                PredictionByPartialMatching PPM = new PredictionByPartialMatching(0);
                PPM.Compress(file);
                PPM.OrderList[1].CalculateTotalCount();
                int actual = (int) PPM.OrderList[1].TotalCount;
                
                Assert.AreEqual(expected, actual);
            }
            [Test]
            public void InputOrderIsOneHasCorrectTotalCountInZerothOrder() {
                string inputPath = TestContext.CurrentContext.TestDirectory + "../../../res/testfile2";
                DataFile file = new DataFile(inputPath);
                int expected = 30 + 15;

                PredictionByPartialMatching PPM = new PredictionByPartialMatching();
                PPM.Compress(file);
                PPM.OrderList[1].CalculateTotalCount();
                int actual = (int)PPM.OrderList[1].TotalCount;

                Assert.AreEqual(expected, actual);
            }
            [Test]
            public void InputOrderIsOneHasCorrectTotalCountInFirstOrder() {
                string inputPath = TestContext.CurrentContext.TestDirectory + "../../../res/testfile2";
                DataFile file = new DataFile(inputPath);
                int expected = 49 + 29;

                PredictionByPartialMatching PPM = new PredictionByPartialMatching(1);
                PPM.Compress(file);
                PPM.OrderList[2].CalculateTotalCount();
                int actual = (int) PPM.OrderList[2].TotalCount;

                Assert.AreEqual(expected, actual);
            }
        }
    }
}
