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
        public void FileLoadsCProgram() {
            DataFile file = new DataFile();
            byte[] expected = {};
            string path = TestContext.CurrentContext.TestDirectory + "../../../res/testfile2";

            file.LoadFromFile(path);
            byte[] actual = file.GetBytes(0, 100);

            Assert.AreEqual(expected, actual);
        }
    }
}
            