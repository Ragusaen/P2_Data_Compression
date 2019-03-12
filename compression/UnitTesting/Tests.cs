using System;
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
    
    [TestFixture, Category("LZ77")]
    public class LZ77Test{
        [Test]
        public void CompressSimpleText() {
            string expectedPath = TestContext.CurrentContext.TestDirectory + "../../../res/compressedTestFile";
            DataFile expectedFile = new DataFile(expectedPath);
            string inputPath = TestContext.CurrentContext.TestDirectory + "../../../res/testfile2";
            DataFile inputFile = new DataFile(inputPath);
            
            LZ77 comp = new LZ77();
            DataFile actualFile = comp.Compress(inputFile);
            actualFile.WriteToFile("/home/odum/output");
            
            Assert.AreEqual(expectedFile.GetBytes(0,expectedFile.Length), actualFile.GetBytes(0, actualFile.Length));;
        }
        [Test]
        public void CompressSimpleText2() {
            string expectedPath = TestContext.CurrentContext.TestDirectory + "../../../res/compressedTestFile";
            DataFile expectedFile = new DataFile(expectedPath);
            string inputPath = TestContext.CurrentContext.TestDirectory + "../../../res/testfile2";
            DataFile inputFile = new DataFile(inputPath);
            
            LZ77 comp = new LZ77();
            DataFile actualFile = comp.Compress(inputFile);
            actualFile.WriteToFile("/home/odum/output");
            
            Assert.AreEqual(expectedFile.GetBytes(0,expectedFile.Length), actualFile.GetBytes(0, actualFile.Length));;
        }
    }

    [TestFixture, Category("EncodedByte")]
    public class TestEncodedByte{
        [Test]
        public void PointerBytePointSpanIs4096WhenSize12() {
            uint expected = 4096;

            uint actual = PointerByte.GetPointerSpan();
            
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ToUnevenBitsPointerByte_150_9_returns_67945() {
            uint expected = 67945;
            PointerByte pb = new PointerByte(150,9);

            uint actual = pb.ToUnevenBits().Data;
            
            Assert.AreEqual(expected, actual);
        }
        [Test]
        public void ToUnevenBitsRawByte_189_returns_445() {
            uint expected = 445;
            RawByte pb = new RawByte(189);

            uint actual = pb.ToUnevenBits().Data;
            
            Assert.AreEqual(expected, actual);
        }
        [Test]
        public void GetBits_3_from_1000() {
            byte expected = 7;
            UnevenBits ub = new UnevenBits(1000,10);

            byte actual = ub.GetBits(3);
            
            Assert.AreEqual(expected, actual);
        }
    }

    [TestFixture, Category("FindMatchingBytes")]
    public class TestFindMatchingBytes {
        [Test]
        public void CompareArraySegmentsResultAsTrue() {
            byte[] firstBytes = {102, 103, 104};
            ArraySegment<byte> first = new ArraySegment<byte>(firstBytes);
            byte[] secondBytes = {102, 103, 104};
            ArraySegment<byte> second = new ArraySegment<byte>(secondBytes);
            
            Assert.IsTrue(FindMatchingBytes.CompareByteArraySegment(first, second));
        }
        [Test]
        public void CompareArraySegmentsResultAsFalse() {
            byte[] firstBytes = {102, 103, 104};
            ArraySegment<byte> first = new ArraySegment<byte>(firstBytes);
            byte[] secondBytes = {102, 103, 105};
            ArraySegment<byte> second = new ArraySegment<byte>(secondBytes);
            
            Assert.IsFalse(FindMatchingBytes.CompareByteArraySegment(first, second));
        }
        [Test]
        public void CompareArraySegmentsDifferentLengths() {
            byte[] firstBytes = {102, 103};
            ArraySegment<byte> first = new ArraySegment<byte>(firstBytes);
            byte[] secondBytes = {102, 103, 104};
            ArraySegment<byte> second = new ArraySegment<byte>(secondBytes);
            
            Assert.IsFalse(FindMatchingBytes.CompareByteArraySegment(first, second));
        }
        [Test]
        public void CompareArraySegmentsSingleArrayIsZero() {
            byte[] firstBytes = {};
            ArraySegment<byte> first = new ArraySegment<byte>(firstBytes);
            byte[] secondBytes = {102, 103, 104};
            ArraySegment<byte> second = new ArraySegment<byte>(secondBytes);
            
            Assert.IsFalse(FindMatchingBytes.CompareByteArraySegment(first, second));
        }
        [Test]
        public void CompareArraySegmentsBothArraysAreZero() {
            byte[] firstBytes = {};
            ArraySegment<byte> first = new ArraySegment<byte>(firstBytes);
            byte[] secondBytes = {};
            ArraySegment<byte> second = new ArraySegment<byte>(secondBytes);
            
            Assert.IsTrue(FindMatchingBytes.CompareByteArraySegment(first, second));
        }
        
        [Test]        
        public void FindArraySegmentFindNeedle() {
            byte[] haystack = {97, 98, 99, 100, 101, 114, 101, 115, 117, 108, 116, 100, 105, 110, 102, 97, 114};
            byte[] needle = {114, 101, 115, 117, 108, 116};
            uint expected = 5;
            ArraySegment<byte> needleAsSegment= new ArraySegment<byte>(needle,0,needle.Length);
            
            uint actual = FindMatchingBytes.FindArraySegment(haystack, needleAsSegment);
            
            Assert.AreEqual(expected, actual);
        }          
        [Test]        
        public void FindArraySegmentDoNotFindNeedle() {
            byte[] haystack = {97, 98, 99, 100, 101, 114, 101, 115, 117, 108, 116, 100, 105, 110, 102, 97, 114};
            byte[] needle = {114, 101, 115, 100, 108, 116};
            uint expected = 5;
            ArraySegment<byte> needleAsSegment= new ArraySegment<byte>(needle,0,needle.Length);
            
            uint actual = FindMatchingBytes.FindArraySegment(haystack, needleAsSegment);
            
            Assert.AreNotEqual(expected, actual);
        }
        [Test]        
        public void FindArraySegmentDoNotFindEmptyHaystack() {
            byte[] haystack = {};
            byte[] needle = {114, 101, 115, 117, 108, 116};
            uint expected = 5;
            ArraySegment<byte> needleAsSegment= new ArraySegment<byte>(needle,0,needle.Length);
            
            uint actual = FindMatchingBytes.FindArraySegment(haystack, needleAsSegment);
            
            Assert.AreNotEqual(expected, actual);
        }             
        [Test]        
        public void FindArraySegmentDoNotFindEmptyNeedle() {
            byte[] haystack = {97, 98, 99, 100, 101, 114, 101, 115, 117, 108, 116, 100, 105, 110, 102, 97, 114};
            byte[] needle = {};
            uint expected = 0;
            ArraySegment<byte> needleAsSegment= new ArraySegment<byte>(needle,0,needle.Length);
            
            uint actual = FindMatchingBytes.FindArraySegment(haystack, needleAsSegment);
            
            Assert.AreEqual(expected, actual);
        }
        [Test]
        public void FindMatchingBytesFindFullLengthNeedle() {
            byte[] haystack = {97, 98, 99, 100, 101, 114, 101, 115, 117, 108, 116, 100, 105, 110, 102, 97, 114};
            byte[] needle = {114, 101, 115, 117, 108, 116};
            Nullable<MatchPointer> expected = new MatchPointer(5, (uint) 6);

            Nullable<MatchPointer> actual = FindMatchingBytes.FindLongestMatch(haystack, needle);
            
            Assert.AreEqual(expected, actual);
        }
        [Test]
        public void FindMatchingBytesFindSemiLengthNeedle() {
            byte[] haystack = {97, 98, 99, 100, 101, 114, 101, 115, 117, 107, 116, 100, 105, 110, 102, 97, 114};
            byte[] needle = {114, 101, 115, 117, 108, 116};
            Nullable<MatchPointer> expected = new MatchPointer(5, (uint) 4);

            Nullable<MatchPointer> actual = FindMatchingBytes.FindLongestMatch(haystack, needle);
            
            Assert.AreEqual(expected, actual);
        }
        [Test]
        public void FindMatchingBytesDoNotFindAnyMatchReturnsNull() {
            byte[] haystack = {97, 98, 99, 100, 101, 114, 101, 115, 117, 108, 116, 100, 105, 110, 102, 97, 114};
            byte[] needle = {86, 102, 59, 108};

            Nullable<MatchPointer> actual = FindMatchingBytes.FindLongestMatch(haystack, needle);
            
            Assert.IsNull(actual);
        }
        [Test]
        public void FindMatchingBytesDoNotFindAnyMatchEmptyHaystack() {
            byte[] haystack = {};
            byte[] needle = {86, 102, 59, 108};

            Nullable<MatchPointer> actual = FindMatchingBytes.FindLongestMatch(haystack, needle);
            
            Assert.IsNull(actual);
        }
        [Test]
        public void FindMatchingBytesDoNotFindAnyMatchEmptyNeedle() {
            byte[] haystack = {97, 98, 99, 100, 101, 114, 101, 115, 117, 108, 116, 100, 105, 110, 102, 97, 114};
            byte[] needle = {};

            Nullable<MatchPointer> actual = FindMatchingBytes.FindLongestMatch(haystack, needle);
            
            Assert.IsNull(actual);
        }
    }


    [TestFixture, Category("SlidingWindow")]
    public class TestSlidingWindow {
        [Test]
        public void SlideReturnsRawByte_a_AsFirstByte() {
            string path = TestContext.CurrentContext.TestDirectory + "../../../res/testfile1";
            DataFile file  = new DataFile(path);
            SlidingWindow sw = new SlidingWindow(file);
            RawByte expected = new RawByte(97);
            
            RawByte actual = (RawByte)sw.Slide();
            
            Assert.AreEqual(expected.Data, actual.Data);
        }
        
        [Test]
        public void SlideReturnsPointer_5_FromTestFile2AtPos10() {
            string path = TestContext.CurrentContext.TestDirectory + "../../../res/testfile2";
            DataFile file  = new DataFile(path);
            SlidingWindow sw = new SlidingWindow(file);
            uint expected = 5;

            for (int i = 0; i < 9; i++)
                sw.Slide();
            PointerByte actual = (PointerByte)sw.Slide();
            
            Assert.AreEqual(expected, actual.Pointer);
        }
        
        [Test]
        public void SlideReturnsPointerLength_2_FromTestFile2AtPos10() {
            string path = TestContext.CurrentContext.TestDirectory + "../../../res/testfile2";
            DataFile file  = new DataFile(path);
            SlidingWindow sw = new SlidingWindow(file);
            uint expected = 2;

            for (int i = 0; i < 9; i++)
                sw.Slide();
            PointerByte actual = (PointerByte)sw.Slide();
            
            Assert.AreEqual(expected, actual.Length);
        }
    }

    [TestFixture, Category("ByteEncoder")]
    public class TestByteEncoder{
        [Test]
        public void ByteEncoderConvert3_UnevenBits_abc() {
            UnevenBits[] array = {new UnevenBits(97, 8), new UnevenBits(98,8), new UnevenBits(99, 8)};
            byte[] expected = {97, 98, 99};

            byte[] actual = ByteEncoder.EncodeBytes(array);
            
            Assert.AreEqual(expected, actual);
        }
        [Test]
        public void ByteEncoderConvertLen_9_17_3() {
            UnevenBits[] array = {new UnevenBits(500, 9), new UnevenBits(78642,17), new UnevenBits(5, 3)};
            byte[] expected = {250, 76, 204, 168};

            byte[] actual = ByteEncoder.EncodeBytes(array);
            
            Assert.AreEqual(expected, actual);
        }
    }
}
            