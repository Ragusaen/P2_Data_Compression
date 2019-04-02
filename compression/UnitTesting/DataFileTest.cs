using System;
using NUnit.Framework;
using Compression;

namespace UnitTesting {
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
        public void FileLoadsCorrectlyByConstructor() {
            string path = TestContext.CurrentContext.TestDirectory + "../../../res/comparefile1";
            byte[] expected = {97, 98, 99, 100, 101, 102, 49, 48, 49, 48};
            DataFile file = new DataFile(path);

            byte[] actual = file.GetBytes(0, 10);
            
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
            uint expected = 3;

            file.LoadFromFile(path);
            uint actual = file.Length;

            Assert.AreEqual(expected, actual);
        }
        [Test]
        public void FileLengthIsCorrect2() {
            DataFile file = new DataFile();
            string path = TestContext.CurrentContext.TestDirectory + "../../../res/testfile2";
            uint expected = 50;

            file.LoadFromFile(path);
            uint actual = file.Length;

            Assert.AreEqual(expected, actual);
        }
        [Test]
        public void FileLengthIsCorrectZeroFromEmptyFile() {
            DataFile file = new DataFile();
            string path = TestContext.CurrentContext.TestDirectory + "../../../res/empty";
            uint expected = 0;

            file.LoadFromFile(path);
            uint actual = file.Length;

            Assert.AreEqual(expected, actual);
        }
        [Test]
        public void LoadBytesFromArray() {
            DataFile file = new DataFile();
            byte[] inputArray = {97, 98, 99};
            
            file.LoadBytes(inputArray);
            
            Assert.AreEqual(inputArray, file.GetBytes(0,(uint)inputArray.Length));
        }
        [Test]
        public void WriteToFile_abc() {
            DataFile file = new DataFile();
            byte[] inputArray = {97, 98, 99};
            file.LoadBytes(inputArray);
            string inputPath = TestContext.CurrentContext.TestDirectory + "../../../res/outputfile1";
            
            file.WriteToFile(inputPath);

            DataFile actualFile = new DataFile(inputPath);
            byte[] actual = actualFile.GetBytes(0, 3);
            
            Assert.AreEqual(inputArray, actual);
        }
        [Test]
        public void CompareDatafilesBothEqualReturnsTrue2() {
            string path1 = TestContext.CurrentContext.TestDirectory + "../../../res/comparefile1";
            string path2 = TestContext.CurrentContext.TestDirectory + "../../../res/comparefile2";
            DataFile file1 = new DataFile(path1);
            DataFile file2 = new DataFile(path2);

            Assert.IsTrue(DataFile.Compare(file1,file2));
        }
        [Test]
        public void CompareDatafilesBothEmpty() {
            string path1 = TestContext.CurrentContext.TestDirectory + "../../../res/empty";
            string path2 = TestContext.CurrentContext.TestDirectory + "../../../res/empty2";
            DataFile file1 = new DataFile(path1);
            DataFile file2 = new DataFile(path2);

            Assert.IsTrue(DataFile.Compare(file1,file2));
        }
    }
}