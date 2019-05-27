using System.Linq;
using Compression;
using Compression.PPM;
using NUnit.Framework;

namespace UnitTesting.PPM {
    [TestFixture]
    [Category("PPM")]
    public class PPMTest {
        public class EntryTest {
            [Test]
            public void LowerContextInputLengthIs0() {
                byte[] input = { };
                var entry = new Entry(11, input);
                var expectedLen = 0;

                entry.NextContext();
                var actual = entry.Context;

                Assert.AreEqual(expectedLen, actual.Length);
            }

            [Test]
            public void LowerContextInputLengthIs1() {
                byte[] input = {11};
                var entry = new Entry(11, input);
                var expectedLen = 0;

                entry.NextContext();
                var actual = entry.Context.Length;

                Assert.AreEqual(expectedLen, actual);
            }

            [Test]
            public void LowerContextInputLengthIs2() {
                byte[] input = {10, 11};
                var entry = new Entry(11, input);
                byte expected = 11;

                entry.NextContext();
                var actual = entry.Context;

                Assert.AreEqual(expected, actual[0]);
            }

            [Test]
            public void LowerContextInputLengthIs5() {
                byte[] input = {10, 11, 12, 13, 14};
                byte[] expected = {11, 12, 13, 14};
                var entry = new Entry(11, input);

                entry.NextContext();
                var actual = entry.Context;

                var byteArrayComparer = new ByteArrayComparer();
                Assert.AreEqual(0, byteArrayComparer.Compare(expected, actual));
            }
        }

        public class ContextTableTest {
            [Test]
            public void UpdateCumCountIncrementsCumulativeCount() {
                byte letter = 65;
                byte letter2 = 66;
                byte[] context = {74, 68};
                var expected = 1;
                var orderX = new ContextTable();

                var e = new Entry(letter, context);

                orderX.UpdateContext(e);
                orderX[context].CalculateCumulativeCounts();
                var first = orderX[context][letter].CumulativeCount;
                e.Symbol = letter2;
                orderX.UpdateContext(e);
                orderX[context].CalculateCumulativeCounts();
                var second = orderX[context][letter2].CumulativeCount;
                var actual = second - first;

                Assert.AreEqual(expected, actual);
            }

            [Test]
            public void CumCountIncrementsBy1() {
                byte[] letterArray = {61, 62};
                byte[] context = {74, 68};
                var expected = 1;
                var e = new Entry {Context = context};

                var orderX = new ContextTable();
                foreach (var t in letterArray) {
                    e.Symbol = t;
                    orderX.UpdateContext(e);
                }

                var actual = orderX[context][letterArray[1]].CumulativeCount -
                             orderX[context][letterArray[0]].CumulativeCount;

                Assert.AreEqual(expected, actual);
            }

            [Test]
            public void UpdateCumCountWithTwoSymbolArrays() {
                byte letter = 65; // A
                byte[] context = {74, 68};
                byte[] context2 = {68, 61};
                var expected = 4 * 10;

                var e = new Entry {Context = context};
                var e2 = new Entry {Context = context2};

                var orderX = new ContextTable();
                for (byte i = 0; i < 10; i++) {
                    e.Symbol = (byte) (letter + i);
                    orderX.UpdateContext(e);
                }

                for (var i = 0; i < 10; i++) {
                    e2.Symbol = (byte) (letter + i);
                    orderX.UpdateContext(e2);
                }

                var actual = 0;

                foreach (var t in orderX) actual += t.Value.TotalCount;

                var ctp = new ContextTablePrinter();
                ctp.ConsolePrint(orderX);

                Assert.AreEqual(expected, actual);
            }

            [Test]
            public void SymbolListHasCorrectCountsInFirstContext() {
                byte[] letterArray = {61, 62, 63, 64, 65, 66, 67, 68, 69, 70}; // 10 elements: 'a - j'
                byte[] context1 = {74, 68};
                byte[] context2 = {42, 77};
                byte[] context3 = {55, 22};
                var expected = 20;
                var e = new Entry {Context = context1};
                var e2 = new Entry {Context = context2};
                var e3 = new Entry {Context = context3};

                var orderX = new ContextTable();
                foreach (var t in letterArray) {
                    e.Symbol = t;
                    orderX.UpdateContext(e);
                }

                foreach (var t in letterArray) {
                    e2.Symbol = t;
                    orderX.UpdateContext(e2);
                }

                foreach (var t in letterArray) {
                    e3.Symbol = t;
                    orderX.UpdateContext(e3);
                }

                var actual = orderX.First().Value.Sum(p => p.Value.Count) + orderX.First().Value.EscapeInfo.Count;

                Assert.AreEqual(expected, actual);
            }

            [Test]
            public void SymbolListHasCorrectCountsInMiddleContext() {
                byte[] letterArray = {61, 62, 63, 64, 65, 66, 67, 68, 69, 70}; // 10 elements: 'a - j'
                byte[] context1 = {74, 68};
                byte[] context2 = {42, 77};
                byte[] context3 = {55, 22};

                var expected = 20;
                var e = new Entry {Context = context1};
                var e2 = new Entry {Context = context2};
                var e3 = new Entry {Context = context3};

                var orderX = new ContextTable();
                foreach (var t in letterArray) {
                    e.Symbol = t;
                    orderX.UpdateContext(e);
                }

                foreach (var t in letterArray) {
                    e2.Symbol = t;
                    orderX.UpdateContext(e2);
                }

                foreach (var t in letterArray) {
                    e3.Symbol = t;
                    orderX.UpdateContext(e3);
                }


                var actual = orderX[context2].Sum(p => p.Value.Count) + orderX[context2].EscapeInfo.Count;

                Assert.AreEqual(expected, actual);
            }

            [Test]
            public void SymbolListHasCorrectCountsInLastContext() {
                byte[] letterArray = {61, 62, 63, 64, 65, 66, 67, 68, 69, 70}; // 10 elements: 'a - j'
                byte[] context1 = {74, 68};
                byte[] context2 = {42, 77};
                byte[] context3 = {55, 22};

                var expected = 20;
                var e = new Entry {Context = context1};
                var e2 = new Entry {Context = context2};
                var e3 = new Entry {Context = context3};

                var orderX = new ContextTable();
                foreach (var t in letterArray) {
                    e.Symbol = t;
                    orderX.UpdateContext(e);
                }

                foreach (var t in letterArray) {
                    e2.Symbol = t;
                    orderX.UpdateContext(e2);
                }

                foreach (var t in letterArray) {
                    e3.Symbol = t;
                    orderX.UpdateContext(e3);
                }

                var actual = orderX[context3].Sum(p => p.Value.Count) + orderX[context3].EscapeInfo.Count;

                Assert.AreEqual(expected, actual);
            }

            [Test]
            public void FirstContextAddsToTable() {
                byte[] context = {74, 68};
                byte symbol = 103;

                var ct = new ContextTable();

                ct.UpdateContext(new Entry(symbol, context));

                Assert.IsTrue(ct.ContainsKey(context));
            }

            [Test]
            public void SecondContextAddsToTable() {
                byte[] context = {74, 68};
                byte[] context2 = {70, 69};
                byte symbol = 103;
                var ct = new ContextTable();


                ct.UpdateContext(new Entry(symbol, context));
                ct.UpdateContext(new Entry(symbol, context2));

                Assert.IsTrue(ct.ContainsKey(context2));
            }

            [Test]
            public void NoContextAddsFirstSymbolToTable() {
                byte symbol = 103;
                var ct = new ContextTable();

                ct.UpdateContext(new Entry(symbol, new byte[0]));

                Assert.IsTrue(ct[new byte[0]].ContainsKey(symbol));
            }

            [Test]
            public void NoContextAddsSecondSymbolToTable() {
                byte symbol = 103;
                byte symbol2 = 104;
                var ct = new ContextTable();

                ct.UpdateContext(new Entry(symbol, new byte[0]));
                ct.UpdateContext(new Entry(symbol2, new byte[0]));

                Assert.IsTrue(ct[new byte[0]].ContainsKey(symbol2));
            }

            [Test]
            public void NoContextAddsFifthSymbolToTable() {
                byte symbol = 100; // d -> expected = 104
                var ct = new ContextTable();


                for (var i = 0; i <= 5; i++)
                    ct.UpdateContext(new Entry((byte) (symbol + i), new byte[0]));

                Assert.IsTrue(ct[new byte[0]].ContainsKey(104));
            }
        }
    }
}