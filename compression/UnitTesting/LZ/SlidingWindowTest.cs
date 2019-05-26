using System;
using NUnit.Framework;
using Compression;
using Compression.LZ;

namespace UnitTesting.LZ {
    [TestFixture, Category("SlidingWindow")]
    public class TestSlidingWindow {
        [Test]
        public void SlideReturnsRawByte_a_AsFirstByte() {
            string path = TestContext.CurrentContext.TestDirectory + "../../../res/testfile1";
            DataFile file = new DataFile(path);
            SlidingWindow sw = new SlidingWindow(file.GetAllBytes());
            RawByte expected = new RawByte(97);

            RawByte actual = (RawByte) sw.Slide();

            Assert.AreEqual(expected.Data, actual.Data);
        }

        [Test]
        public void SlideReturnsPointer_5_FromTestFile2AtPos10() {
            string path = TestContext.CurrentContext.TestDirectory + "../../../res/testfile2";
            DataFile file = new DataFile(path);
            SlidingWindow sw = new SlidingWindow(file.GetAllBytes());
            uint expected = 5;

            for (int i = 0; i < 9; i++)
                sw.Slide();
            PointerByte actual = (PointerByte) sw.Slide();

            Assert.AreEqual(expected, actual.Pointer);
        }

        [Test]
        public void SlideReturnsPointerLength_2_FromTestFile2AtPos10() {
            string path = TestContext.CurrentContext.TestDirectory + "../../../res/testfile2";
            DataFile file = new DataFile(path);
            SlidingWindow sw = new SlidingWindow(file.GetAllBytes());
            var expected = new PointerByte(5, 2);

            for (int i = 0; i < 9; i++)
                Console.WriteLine(sw.Slide());
            
            PointerByte actual = (PointerByte) sw.Slide();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void SlideNotAtEndWhenInBeginningOfFile() {
            string path = TestContext.CurrentContext.TestDirectory + "../../../res/testfile1";
            DataFile file = new DataFile(path);
            SlidingWindow sw = new SlidingWindow(file.GetAllBytes());

            Assert.IsFalse(sw.AtEnd());
        }

        [Test]
        public void SlideAtEndWhenAtEnd() {
            string path = TestContext.CurrentContext.TestDirectory + "../../../res/testfile1";
            DataFile file = new DataFile(path);
            SlidingWindow sw = new SlidingWindow(file.GetAllBytes());

            for (int i = 0; i < 3; i++)
                sw.Slide();

            Assert.IsTrue(sw.AtEnd());
        }

        [Test]
        public void SlideReturnsPointer_3_FromTestFile3AtPos4() {
            string path = TestContext.CurrentContext.TestDirectory + "../../../res/testfile3";
            DataFile file = new DataFile(path);
            SlidingWindow sw = new SlidingWindow(file.GetAllBytes());
            uint expected = 3;

            for (int i = 0; i < 4; i++)
                sw.Slide();
            PointerByte actual = (PointerByte) sw.Slide();

            Assert.AreEqual(expected, actual.Pointer);
        }

        [Test]
        public void SlideReturnsPointerLength_3_FromTestFile3AtPos4() {
            string path = TestContext.CurrentContext.TestDirectory + "../../../res/testfile3";
            DataFile file = new DataFile(path);
            SlidingWindow sw = new SlidingWindow(file.GetAllBytes());
            uint expected = 3;

            for (int i = 0; i < 4; i++)
                sw.Slide();
            PointerByte actual = (PointerByte) sw.Slide();

            Assert.AreEqual(expected, actual.Length);
        }
    }
}