using System;
using NUnit.Framework;
using Compression;
using Compression.LZ;
using Compression.ByteStructures;

namespace UnitTesting.LZ {
    [TestFixture, Category("LZ77")]
    public class LZ77Test{
        public class CompressTests {
            [Test]
            public void CompressSimpleText() {
                string expectedPath = TestContext.CurrentContext.TestDirectory + "../../../res/compressedTestFile";
                DataFile expectedFile = new DataFile(expectedPath);
                string inputPath = TestContext.CurrentContext.TestDirectory + "../../../res/testfile2";
                DataFile inputFile = new DataFile(inputPath);
                
                LZ77 comp = new LZ77();
                DataFile actualFile = comp.Compress(inputFile);
                
                Assert.AreEqual(expectedFile.GetBytes(0,expectedFile.Length), actualFile.GetBytes(0, actualFile.Length));;
            }
            [Test]
            public void CompressSimpleTextNoPointersByteArraysOnly() {
                byte[] inputBytes = {97, 98, 99};
                DataFile inputFile = new DataFile();
                inputFile.LoadBytes(inputBytes);
                byte[] expected = {48, 152, 140, 96};
                
                LZ77 comp = new LZ77();
                DataFile output = comp.Compress(inputFile);
                byte[] actual = output.GetBytes(0, output.Length);
                
                Assert.AreEqual(expected, actual);
            }
            
            [Test]
            public void CompressSimpleTextNoPointersDataFile() {
                string inputPath = TestContext.CurrentContext.TestDirectory + "../../../res/testfile1";
                DataFile inputFile = new DataFile(inputPath);
                byte[] expectedData = {48, 152, 140, 96};
                DataFile expected = new DataFile();
                expected.LoadBytes(expectedData);
                
                LZ77 comp = new LZ77();
                DataFile actual = comp.Compress(inputFile);
                
                Assert.IsTrue(DataFile.Compare(expected, actual));
            }
            
            [Test]
            public void CompressSimpleTextPointers() {
                string inputPath = TestContext.CurrentContext.TestDirectory + "../../../res/testfile3";
                DataFile inputFile = new DataFile(inputPath);
                byte[] expectedData = {48, 152, 140, 102, 72, 1, 152};
                DataFile expected = new DataFile();
                expected.LoadBytes(expectedData);
                
                LZ77 comp = new LZ77();
                DataFile actual = comp.Compress(inputFile);
                
                ByteArrayPrinter.PrintBits(expected.GetBytes(0, expected.Length));
                Console.WriteLine();
                ByteArrayPrinter.PrintBits(actual.GetBytes(0,actual.Length));
                
                Assert.AreEqual(expected.GetBytes(0, expected.Length), actual.GetBytes(0, actual.Length));
            }
        }

        public class DecompressTests {
            [Test]
            public void DecompressSimpleText() {
                string inputPath = TestContext.CurrentContext.TestDirectory + "../../../res/compressedTestFile";
                string expectedPath = TestContext.CurrentContext.TestDirectory + "../../../res/testfile2";
                DataFile expectedFile = new DataFile(expectedPath);
                DataFile inputFile = new DataFile(inputPath);
            
                LZ77 comp = new LZ77();
                DataFile actualFile = comp.Decompress(inputFile);
            
                Assert.AreEqual(expectedFile.GetBytes(0,expectedFile.Length), actualFile.GetBytes(0, actualFile.Length));;
            }
        }
    }
}