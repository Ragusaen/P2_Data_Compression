using Compression;
using Compression.PPM;
using NUnit.Framework;

namespace UnitTesting.PPM{
    [TestFixture, Category("PPM")]
    public class PPMTest{
        public class ContextTableContentTest{
            [Test]
            public void NewSymbolAddsToList() {
                byte letter = 65; // 'e'
                byte[] context = {74, 68}; // "th"
                byte expected = 65;

                Context a = new Context(context, letter);
                byte actual = a.SymbolList[0].Letter;
                
                Assert.AreEqual(expected, actual);
            }
            [Test]
            public void NewSymbolCountIsOne() {
                byte letter = 65; // 'e'
                byte[] context = {74, 68}; // "th"
                uint expected = 1;

                Context a = new Context(context, letter);
                uint actual = a.SymbolList[0].Count;
                
                Assert.AreEqual(expected, actual);
            }
            [Test]
            public void UpdateNoPreviousContextAddsNewContext() {
                byte letter = 65; // 'e'
                byte[] context = {74, 68}; // "th"
                byte[] expected = {74, 68};

                Context a = new Context(context, letter);
                byte[] actual = a.ContextBytes;
                
                ByteArrayComparer byteArrayComparer = new ByteArrayComparer();
                Assert.AreEqual(0,byteArrayComparer.Compare(expected, actual));
            }
            [Test]
            public void UpdateIfNoPreviousContextThenAddNewContext() {
                byte letter = 65; // 'e'
                byte[] context = {74, 68}; // "th"
                byte[] expected = {74, 68};

                Context a = new Context(context, letter);
                byte[] actual = a.ContextBytes;
                
                ByteArrayComparer byteArrayComparer = new ByteArrayComparer();
                Assert.AreEqual(0,byteArrayComparer.Compare(expected, actual));
            }
        }
    }
}