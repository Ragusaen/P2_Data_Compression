using System;
using System.IO;
using System.Runtime.CompilerServices;
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
            uint expected = 3;

            file.LoadFromFile(path);
            uint actual = file.Length();

            Assert.AreEqual(expected, actual);
        }
        [Test]
        public void FileLengthIsCorrect2() {
            DataFile file = new DataFile();
            string path = TestContext.CurrentContext.TestDirectory + "../../../res/testfile2";
            uint expected = 50;

            file.LoadFromFile(path);
            uint actual = file.Length();

            Assert.AreEqual(expected, actual);
        }
        [Test]
        public void FileLengthIsCorrectZeroFromEmptyFile() {
            DataFile file = new DataFile();
            string path = TestContext.CurrentContext.TestDirectory + "../../../res/empty";
            uint expected = 0;

            file.LoadFromFile(path);
            uint actual = file.Length();

            Assert.AreEqual(expected, actual);
        }
    }
    
    [TestFixture, Category("LZ77")]
    public class LZ77Test{
        [Test]
        public void CompressSimpleText() {
            DataFile file = new DataFile();
            DataFile actualFile = new DataFile();
            LZ77 compressor = new LZ77();
            string path = TestContext.CurrentContext.TestDirectory + "../../../res/testfile1";
            byte[] expected = { };

            file.LoadFromFile(path);

            actualFile = compressor.Compress(file);
            
            Assert.Equals(file, actualFile);
        }
    }

    [TestFixture, Category("EncodedChar")]
    public class EncodedChar{
        [Test]
        public void PointerBytePointSpanIs4096WhenSize12() {
            uint expected = 4096;

            uint actual = PointerByte.GetPointerSpan();
            
            Assert.AreEqual(expected, actual);
        }
    }

    [TestFixture, Category("FindMatchingBytes")]
    public class TestFindMatchingBytes {
        [Test]
        public void AssertArraySegments() {
            byte[] firstBytes = {102, 103, 104};
            ArraySegment<byte> first = new ArraySegment<byte>(firstBytes);
            byte[] secondBytes = {102, 103, 104};
            ArraySegment<byte> second = new ArraySegment<byte>(secondBytes);
            
            Assert.IsTrue(FindMatchingBytes.CompareByteArraySegment(first, second));
        }
        
        [Test]        
        public void FindMatchingBytesFindNeedle() {
            byte[] haystack = {97, 98, 99, 100, 101, 114, 101, 115, 117, 108, 116, 100, 105, 110, 102, 97, 114};
            byte[] needle = {114, 101, 115, 117, 108, 116};
            uint expected = 5;
            ArraySegment<byte> needleAsSegment= new ArraySegment<byte>(needle,0,6);
            
            uint actual = FindMatchingBytes.FindArraySegment(haystack, needleAsSegment);
            
            Assert.AreEqual(expected, actual);
        }                
        
        [Test]
        public void FindMatchingBytesFindFullLengthNeedle() {
            byte[] haystack = {97, 98, 99, 100, 101, 114, 101, 115, 117, 108, 116, 100, 105, 110, 102, 97, 114};
            byte[] needle = {114, 101, 115, 117, 108, 116};
            Nullable<MatchPointer> expected = new MatchPointer(5, 6);

            Nullable<MatchPointer> actual = FindMatchingBytes.FindLongestMatch(haystack, needle);
            
            Assert.AreEqual(expected, actual);
        }
    }
    
    
}
            