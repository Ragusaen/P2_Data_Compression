using System.Reflection;
using Compression;
using Compression.PPM;
using NUnit.Framework;

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
                byte letter = 65; // 'e'
                byte[] context = {74, 68}; // "th"
                byte expected = 65;

                Context a = new Context(context);
                a.Update(letter);
                byte actual = ((Letter) a.SymbolList[0].Data).Data;

                Assert.AreEqual(expected, actual);
            }

            [Test]
            public void NewSymbolCountIsOne() {
                byte letter = 65; // 'e'
                byte[] context = {74, 68}; // "th"
                uint expected = 1;

                Context a = new Context(context);
                a.Update(letter);
                uint actual = a.SymbolList[0].Count;

                Assert.AreEqual(expected, actual);
            }

            [Test]
            public void UpdateNoPreviousContextAddsNewContext() {
                byte letter = 65; // 'e'
                byte[] context = {74, 68}; // "th"
                byte[] expected = {74, 68};

                Context a = new Context(context);
                a.Update(letter);
                byte[] actual = a.ContextBytes;

                ByteArrayComparer byteArrayComparer = new ByteArrayComparer();
                Assert.AreEqual(0, byteArrayComparer.Compare(expected, actual));
            }

            [Test]
            public void UpdatePreviousContextUpdatesContextWithNewSymbol() {
                byte letter = 65; // 'e'
                byte letter2 = 66; // 'f'
                byte[] context = {74, 68}; // "th"
                byte expected = 66;

                Context a = new Context(context);
                a.Update(letter);
                a.Update(letter2);
                byte actual = ((Letter) a.SymbolList[1].Data).Data;

                ByteArrayComparer byteArrayComparer = new ByteArrayComparer();
                Assert.AreEqual(expected, actual);
            }

            [Test]
            public void UpdatePreviousContextIncrementsSymbolCount() {
                byte letter = 65; // 'e'
                byte[] context = {74, 68}; // "th"
                uint expected = 2;

                Context a = new Context(context);
                a.Update(letter);
                a.Update(letter);
                uint actual = a.SymbolList[0].Count;

                ByteArrayComparer byteArrayComparer = new ByteArrayComparer();
                Assert.AreEqual(expected, actual);
            }
        }

        public class ContextTableTest{
            [Test]
            public void UpdatePreviousContextIncrementsTotalCount() {
                byte letter = 65; // 'e'
                byte[] context = {74, 68}; // "th"
                uint expected = 2;
                
                ContextTable orderX = new ContextTable();
                orderX.UpdateContext(context, letter);
                orderX.UpdateContext(context, letter);
                orderX.CalculateTotalCount();
                uint actual = orderX.TotalCount;

                ByteArrayComparer byteArrayComparer = new ByteArrayComparer();
                Assert.AreEqual(expected, actual);
            }
        }
    }
}
