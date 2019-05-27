using System;
using Compression;
using Compression.LZ;
using NUnit.Framework;

namespace UnitTesting.LZ {
    [TestFixture]
    [Category("SlidingWindow")]
    public class TestSlidingWindow {
        [Test]
        public void SlideAtEndWhenAtEnd() {
            var path = TestContext.CurrentContext.TestDirectory + "../../../res/testfile1";
            var file = new DataFile(path);
            var sw = new SlidingWindow(file.GetAllBytes());

            for (var i = 0; i < 3; i++)
                sw.Slide();

            Assert.IsTrue(sw.AtEnd());
        }

        [Test]
        public void SlideNotAtEndWhenInBeginningOfFile() {
            var path = TestContext.CurrentContext.TestDirectory + "../../../res/testfile1";
            var file = new DataFile(path);
            var sw = new SlidingWindow(file.GetAllBytes());

            Assert.IsFalse(sw.AtEnd());
        }

        [Test]
        public void SlideReturnsPointer_3_FromTestFile3AtPos4() {
            var path = TestContext.CurrentContext.TestDirectory + "../../../res/testfile3";
            var file = new DataFile(path);
            var sw = new SlidingWindow(file.GetAllBytes());
            uint expected = 3;

            for (var i = 0; i < 4; i++)
                sw.Slide();
            var actual = (PointerByte) sw.Slide();

            Assert.AreEqual(expected, actual.Pointer);
        }

        [Test]
        public void SlideReturnsPointer_5_FromTestFile2AtPos10() {
            var path = TestContext.CurrentContext.TestDirectory + "../../../res/testfile2";
            var file = new DataFile(path);
            var sw = new SlidingWindow(file.GetAllBytes());
            uint expected = 5;

            for (var i = 0; i < 9; i++)
                sw.Slide();
            var actual = (PointerByte) sw.Slide();

            Assert.AreEqual(expected, actual.Pointer);
        }

        [Test]
        public void SlideReturnsPointerLength_2_FromTestFile2AtPos10() {
            var path = TestContext.CurrentContext.TestDirectory + "../../../res/testfile2";
            var file = new DataFile(path);
            var sw = new SlidingWindow(file.GetAllBytes());
            var expected = new PointerByte(5, 2);

            for (var i = 0; i < 9; i++)
                Console.WriteLine(sw.Slide());

            var actual = (PointerByte) sw.Slide();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void SlideReturnsPointerLength_3_FromTestFile3AtPos4() {
            var path = TestContext.CurrentContext.TestDirectory + "../../../res/testfile3";
            var file = new DataFile(path);
            var sw = new SlidingWindow(file.GetAllBytes());
            uint expected = 3;

            for (var i = 0; i < 4; i++)
                sw.Slide();
            var actual = (PointerByte) sw.Slide();

            Assert.AreEqual(expected, actual.Length);
        }

        [Test]
        public void SlideReturnsRawByte_a_AsFirstByte() {
            var path = TestContext.CurrentContext.TestDirectory + "../../../res/testfile1";
            var file = new DataFile(path);
            var sw = new SlidingWindow(file.GetAllBytes());
            var expected = new RawByte(97);

            var actual = (RawByte) sw.Slide();

            Assert.AreEqual(expected.Data, actual.Data);
        }
    }
}