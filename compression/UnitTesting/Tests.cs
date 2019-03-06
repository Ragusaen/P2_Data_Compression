using System;
using NUnit.Framework;

using Compression;

namespace UnitTesting{
    [TestFixture, Category("DataFile")]
    public class DataFileTest{
        [Test]
        public void FileLoadsCorrectly() {
            DataFile file = new DataFile();
            byte[] expected = {97, 98, 99};
            string path = TestContext.CurrentContext.TestDirectory + "../../../res/testfile1";

            file.LoadFromFile(path);
            byte[] actual = file.GetBytes(0, 3);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void FileLoadsBinProgram() {
            DataFile file = new DataFile();
            byte[] expected = {127, 69, 76, 70, 2, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 62, 0, 1, 0, 0, 0, 16, 6, 0, 0, 0, 0, 0, 0, 64, 0, 0, 0, 0, 0, 0, 0, 176, 25};
            string path = TestContext.CurrentContext.TestDirectory + "../../../res/testbin1";

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
    }
}
            