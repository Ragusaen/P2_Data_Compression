using System;
using System.Security.Cryptography;
using NUnit.Framework;

using compression;
using compression.LZ77;

namespace UnitTesting{
    [TestFixture, Category("DataFile")]
    public class DataFileTest{
        [Test]
        public void FileLoadsCorrectly() {
            DataFile file = new DataFile();
            string path = TestContext.CurrentContext.TestDirectory + "../../../res/testfile1";
            byte[] expected = {97, 98, 99};

            file.LoadFromFile(path);
            byte[] actual = file.GetBytes(0, 3);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void FileLoadsBinProgram() {
            DataFile file = new DataFile();
            string path = TestContext.CurrentContext.TestDirectory + "../../../res/testbin1";
            byte[] expected = {127, 69, 76, 70, 2, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 62, 
                0, 1, 0, 0, 0, 16, 6, 0, 0, 0, 0, 0, 0, 64, 0, 0, 0, 0, 0, 0, 0, 176, 25};

            file.LoadFromFile(path);
            byte[] actual = file.GetBytes(0, (uint)expected.Length);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ThrowsOutOfBoundsExceptionWhenGetStartAtLargerThanSize() {
            DataFile file = new DataFile();
            string path = TestContext.CurrentContext.TestDirectory + "../../../res/testfile1";
            
            file.LoadFromFile(path);
            TestDelegate act = () => file.GetBytes(4, 1);

            Assert.Throws<IndexOutOfRangeException>(act);
        }

        [Test]
        public void ThrowsOutOfBoundsExceptionWhenLengthTooLarge() {
            DataFile file = new DataFile();
            string path = TestContext.CurrentContext.TestDirectory + "../../../res/testfile1";

            file.LoadFromFile(path);
            TestDelegate act = () => file.GetBytes(1, 3);

            Assert.Throws<IndexOutOfRangeException>(act);
        }

        [Test]
        public void GetBytesLenIsZeroOutputsEmptyArray() {
            DataFile file = new DataFile();
            byte[] expected = {};
            string path = TestContext.CurrentContext.TestDirectory + "../../../res/testfile1";

            file.LoadFromFile(path);
            byte[] actual = file.GetBytes(0, 0);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void FileLengthIsCorrect() {
            DataFile file = new DataFile();
            string path = TestContext.CurrentContext.TestDirectory + "../../../res/testfile1";
            int expected = 3;

            file.LoadFromFile(path);
            int actual = file.Length();

            Assert.AreEqual(expected, actual);
        }
        [Test]
        public void FileLengthIsCorrect2() {
            DataFile file = new DataFile();
            string path = TestContext.CurrentContext.TestDirectory + "../../../res/testfile2";
            int expected = 50;

            file.LoadFromFile(path);
            int actual = file.Length();

            Assert.AreEqual(expected, actual);
        }
        [Test]
        public void FileLengthIsCorrectZeroFromEmptyFile() {
            DataFile file = new DataFile();
            string path = TestContext.CurrentContext.TestDirectory + "../../../res/empty";
            int expected = 0;

            file.LoadFromFile(path);
            int actual = file.Length();

            Assert.AreEqual(expected, actual);
        }
    }

    [TestFixture, Category("LZ77")]
    public class LZ77Test{
        [Test]
        public void LZ77CompressSimpleText() {
            DataFile file = new DataFile();
            DataFile actualFile = new DataFile();
            LZ77 compressor = new LZ77();

            byte[] expected = { };
            string path = TestContext.CurrentContext.TestDirectory + "../../../res/testfile1";

            file.LoadFromFile(path);

            actualFile = compressor.Compress(file);
            
            Assert.Equals(file, actualFile);
        }
    }
}
            