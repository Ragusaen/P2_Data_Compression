using System;
using NUnit.Framework;

using Compression;

namespace UnitTesting {
    [TestFixture, Category("DataFile")]
    public class DataFileTest{
        [Test]
        public void FileLoadsCorrectly() {
            DataFile file = new DataFile();
            byte[] expected = {65, 66, 67};
            string path = "res/testfile1";
            
            file.LoadFromFile(path);
            byte[] actual = file.GetBytes(0, 3);

            Assert.Equals(expected, actual);
        }
    }