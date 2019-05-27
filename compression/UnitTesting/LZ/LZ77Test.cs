using Compression;
using Compression.LZ;
using NUnit.Framework;

namespace UnitTesting.LZ {
    [TestFixture]
    [Category("LZ77")]
    public class LZ77Test {
        public class CompressTests {
            [Test]
            public void CompressSimpleText() {
                var expectedPath = TestContext.CurrentContext.TestDirectory + "../../../res/compressedTestFile";
                var expectedFile = new DataFile(expectedPath);
                var inputPath = TestContext.CurrentContext.TestDirectory + "../../../res/testfile2";
                var inputFile = new DataFile(inputPath);

                var comp = new LZSS();
                var actualFile = comp.Compress(inputFile);

                Assert.AreEqual(expectedFile.GetBytes(0, expectedFile.Length),
                    actualFile.GetBytes(0, actualFile.Length));
                ;
            }

            [Test]
            public void CompressSimpleTextNoPointersByteArraysOnly() {
                byte[] inputBytes = {97, 98, 99};
                var inputFile = new DataFile();
                inputFile.LoadBytes(inputBytes);
                byte[] expected = {48, 152, 140, 96};

                var comp = new LZSS();
                var output = comp.Compress(inputFile);
                var actual = output.GetBytes(0, output.Length);

                Assert.AreEqual(expected, actual);
            }

            [Test]
            public void CompressSimpleTextNoPointersDataFile() {
                var inputPath = TestContext.CurrentContext.TestDirectory + "../../../res/testfile1";
                var inputFile = new DataFile(inputPath);
                byte[] expectedData = {48, 152, 140, 96};
                var expected = new DataFile();
                expected.LoadBytes(expectedData);

                var comp = new LZSS();
                var actual = comp.Compress(inputFile);

                Assert.IsTrue(DataFile.Compare(expected, actual));
            }

            [Test]
            public void CompressSimpleTextPointers() {
                var inputPath = TestContext.CurrentContext.TestDirectory + "../../../res/testfile3";
                var inputFile = new DataFile(inputPath);
                byte[] expectedData = {48, 152, 140, 102, 72, 1, 152};
                var expected = new DataFile();
                expected.LoadBytes(expectedData);

                var comp = new LZSS();
                var actual = comp.Compress(inputFile);

                Assert.AreEqual(expected.GetBytes(0, expected.Length), actual.GetBytes(0, actual.Length));
            }
        }

        public class DecompressTests {
            [Test]
            public void DecompressSimpleText() {
                var inputPath = TestContext.CurrentContext.TestDirectory + "../../../res/compressedTestFile";
                var expectedPath = TestContext.CurrentContext.TestDirectory + "../../../res/testfile2";
                var expectedFile = new DataFile(expectedPath);
                var inputFile = new DataFile(inputPath);

                var comp = new LZSS();
                var actualFile = comp.Decompress(inputFile);

                Assert.AreEqual(expectedFile.GetBytes(0, expectedFile.Length),
                    actualFile.GetBytes(0, actualFile.Length));
                ;
            }
        }
    }
}