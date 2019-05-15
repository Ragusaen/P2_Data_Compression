using System.Linq;
using Compression;
using Compression.PPM;
using NUnit.Framework;

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
                int expected = 1;
                ContextTable orderX = new ContextTable();
                
                orderX.UpdateContext(context, letter);
                orderX.ContextDict[context].CalculateCumulativeCounts();
                int first = orderX.ContextDict[context][letter].CumulativeCount;
                orderX.UpdateContext(context, letter2);
                orderX.ContextDict[context].CalculateCumulativeCounts();
                int second = orderX.ContextDict[context][letter2].CumulativeCount;
                int actual = second - first;
                
                Assert.AreEqual(expected, actual);
            }

            [Test]

            public void CumCountIncrementsBy1() {
                byte[] letterArray = {61, 62};
                byte[] context = {74, 68};
                int expected = 1;

                ContextTable orderX = new ContextTable();
                foreach (var t in letterArray) {
                    orderX.UpdateContext(context, t);
                }

                orderX.CalculateAllCounts();

                int actual = orderX.ContextDict[context][letterArray[1]].CumulativeCount -
                              orderX.ContextDict[context][letterArray[0]].CumulativeCount;

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
                foreach (var t in letterArray) {
                    orderX.UpdateContext(context, t);
                }

                foreach (var t in letterArray) {
                    orderX.UpdateContext(context2, t);
                }

                foreach (var t in letterArray) {
                    orderX.UpdateContext(context3, t);
                }

                int actual = orderX.ContextDict.First().Value.Sum(p => p.Value.Count) + orderX.ContextDict.First().Value.EscapeInfo.Count;
                
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
                foreach (var t in letterArray) {
                    orderX.UpdateContext(context1, t);
                }

                foreach (var t in letterArray) {
                    orderX.UpdateContext(context2, t);
                }

                foreach (var t in letterArray) {
                    orderX.UpdateContext(context3, t);
                }
                
                
                int actual = orderX.ContextDict[context2].Sum(p => p.Value.Count) + orderX.ContextDict[context2].EscapeInfo.Count;

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
                foreach (var t in letterArray) {
                    orderX.UpdateContext(context1, t);
                }

                foreach (var t in letterArray) {
                    orderX.UpdateContext(context2, t);
                }

                foreach (var t in letterArray) {
                    orderX.UpdateContext(context3, t);
                }
                
                int actual = orderX.ContextDict[context3].Sum(p => p.Value.Count) + orderX.ContextDict[context3].EscapeInfo.Count;

                Assert.AreEqual(expected, actual);
            }

            [Test]
            public void FirstContextAddsToTable() {
                byte[] context = {74, 68};
                byte symbol = 103;

                ContextTable ct = new ContextTable();

                ct.UpdateContext(context, symbol);

                Assert.IsTrue(ct.ContextDict.ContainsKey(context));
            }

            [Test]
            public void SecondContextAddsToTable() {
                byte[] context = {74, 68};
                byte[] context2 = {70, 69};
                byte symbol = 103;
                ContextTable ct = new ContextTable();

                ct.UpdateContext(context, symbol);
                ct.UpdateContext(context2, symbol);

                Assert.IsTrue(ct.ContextDict.ContainsKey(context2));
            }

            [Test]
            public void NoContextAddsFirstSymbolToTable() {
                byte symbol = 103;
                ContextTable ct = new ContextTable();

                ct.UpdateContext(new byte[0], symbol);

                Assert.IsTrue(ct.ContextDict[new byte[0]].ContainsKey(symbol));
            }

            [Test]
            public void NoContextAddsSecondSymbolToTable() {
                byte symbol = 103;
                byte symbol2 = 104;
                ContextTable ct = new ContextTable();

                ct.UpdateContext(new byte[0], symbol);
                ct.UpdateContext(new byte[0], symbol2);

                Assert.IsTrue(ct.ContextDict[new byte[0]].ContainsKey(symbol2));
            }

            [Test]
            public void NoContextAddsFifthSymbolToTable() {
                byte symbol = 100; // d -> expected = 104
                ContextTable ct = new ContextTable();


                for (int i = 0; i <= 5; i++)
                    ct.UpdateContext(new byte[0], (byte) (symbol + i));

                Assert.IsTrue(ct.ContextDict[new byte[0]].ContainsKey(104));
            }
        }
    }
}
