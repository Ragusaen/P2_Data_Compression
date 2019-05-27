using System;
using Compression;
using NUnit.Framework;

namespace UnitTesting {
    [TestFixture]
    [Category("DataFile")]
    public class DataFileTest {
        public class LoadFromFileTests {
            [Test]
            public void Loads_abc_From_testfile1() {
                var file = new DataFile();
                var path = TestContext.CurrentContext.TestDirectory + "../../../res/testfile1";
                byte[] expected = {97, 98, 99};

                file.LoadFromFile(path);
                var actual = file.GetBytes(0, 3);

                Assert.AreEqual(expected, actual);
            }

            [Test]
            public void Loads_comparefile1_WithConstructor() {
                var path = TestContext.CurrentContext.TestDirectory + "../../../res/comparefile1";
                byte[] expected = {97, 98, 99, 100, 101, 102, 49, 48, 49, 48};
                var file = new DataFile(path);

                var actual = file.GetBytes(0, 10);

                Assert.AreEqual(expected, actual);
            }

            [Test]
            public void LoadsBinaryTestFile_testbin1() {
                var file = new DataFile();
                var path = TestContext.CurrentContext.TestDirectory + "../../../res/testbin1";
                byte[] expected = {
                    127, 69, 76, 70, 2, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 3, 0, 62,
                    0, 1, 0, 0, 0, 16, 6, 0, 0, 0, 0, 0, 0, 64, 0, 0, 0, 0, 0, 0, 0, 176, 25
                };

                file.LoadFromFile(path);
                var actual = file.GetBytes(0, expected.Length);

                Assert.AreEqual(expected, actual);
            }
        }

        public class GetBytesTests {
            [Test]
            public void ThrowsOutOfBoundsExceptionWhenGetStartAtLargerThanSize() {
                var file = new DataFile();
                var path = TestContext.CurrentContext.TestDirectory + "../../../res/testfile1";

                file.LoadFromFile(path);
                TestDelegate act = () => file.GetBytes(4, 1);

                Assert.Throws<IndexOutOfRangeException>(act);
            }

            [Test]
            public void ThrowsOutOfBoundsExceptionWhenLengthTooLarge() {
                var file = new DataFile();
                var path = TestContext.CurrentContext.TestDirectory + "../../../res/testfile1";

                file.LoadFromFile(path);
                TestDelegate act = () => file.GetBytes(1, 3);

                Assert.Throws<IndexOutOfRangeException>(act);
            }

            [Test]
            public void GetBytesLenIsZeroOutputsEmptyArray() {
                var file = new DataFile();
                byte[] expected = { };
                var path = TestContext.CurrentContext.TestDirectory + "../../../res/testfile1";

                file.LoadFromFile(path);
                var actual = file.GetBytes(0, 0);

                Assert.AreEqual(expected, actual);
            }
        }

        public class GetAllBytes {
            [Test]
            public void ReturnsAllBytesFrom_testfile1() {
                var path = TestContext.CurrentContext.TestDirectory + "../../../res/testfile1";
                var input = new DataFile(path);
                byte[] expected = {97, 98, 99};

                var actual = input.GetAllBytes();

                Assert.AreEqual(expected, actual);
            }
        }

        public class LengthTests {
            [Test]
            public void FileLengthIsCorrect() {
                var file = new DataFile();
                var path = TestContext.CurrentContext.TestDirectory + "../../../res/testfile1";
                var expected = 3;

                file.LoadFromFile(path);
                var actual = file.Length;

                Assert.AreEqual(expected, actual);
            }

            [Test]
            public void FileLengthIsCorrect2() {
                var file = new DataFile();
                var path = TestContext.CurrentContext.TestDirectory + "../../../res/testfile2";
                var expected = 50;

                file.LoadFromFile(path);
                var actual = file.Length;

                Assert.AreEqual(expected, actual);
            }

            [Test]
            public void FileLengthIsCorrectZeroFromEmptyFile() {
                var file = new DataFile();
                var path = TestContext.CurrentContext.TestDirectory + "../../../res/empty";
                var expected = 0;

                file.LoadFromFile(path);
                var actual = file.Length;

                Assert.AreEqual(expected, actual);
            }
        }

        [Test]
        public void CompareDatafilesBothEmpty() {
            var path1 = TestContext.CurrentContext.TestDirectory + "../../../res/empty";
            var path2 = TestContext.CurrentContext.TestDirectory + "../../../res/empty2";
            var file1 = new DataFile(path1);
            var file2 = new DataFile(path2);

            Assert.IsTrue(DataFile.Compare(file1, file2));
        }

        [Test]
        public void CompareDatafilesBothEqualReturnsTrue2() {
            var path1 = TestContext.CurrentContext.TestDirectory + "../../../res/comparefile1";
            var path2 = TestContext.CurrentContext.TestDirectory + "../../../res/comparefile2";
            var file1 = new DataFile(path1);
            var file2 = new DataFile(path2);

            Assert.IsTrue(DataFile.Compare(file1, file2));
        }

        [Test]
        public void LoadBytesFromArray() {
            var file = new DataFile();
            byte[] inputArray = {97, 98, 99};

            file.LoadBytes(inputArray);

            Assert.AreEqual(inputArray, file.GetBytes(0, inputArray.Length));
        }

        [Test]
        public void WriteToFile_abc() {
            var file = new DataFile();
            byte[] inputArray = {97, 98, 99};
            file.LoadBytes(inputArray);
            var inputPath = TestContext.CurrentContext.TestDirectory + "../../../res/outputfile1";

            file.WriteToFile(inputPath);

            var actualFile = new DataFile(inputPath);
            var actual = actualFile.GetBytes(0, 3);

            Assert.AreEqual(inputArray, actual);
        }
    }
}