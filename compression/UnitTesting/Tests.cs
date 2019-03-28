﻿using System;
using System.Web.UI;
using NUnit.Framework;

using compression;
using compression.NBWT;
using compression.LZ;
using compression.RLE;

namespace UnitTesting{
    [TestFixture, Category("BurrowsWheelerTransform")]
    public class BWTTest {
        [Test]
        public void TransformReturnsBNNAAAFromBANANA() {
            byte[] input = new byte[] {(byte)'^',(byte) 'B', (byte) 'A', (byte) 'N', (byte) 'A', (byte) 'N', (byte) 'A', (byte)'|'};
            byte[] expected = new byte[] {(byte) 'B', (byte) 'N', (byte) 'N', (byte) '^', (byte) 'A', (byte) 'A', (byte) '|', (byte) 'A'};
            
            byte[] result = BWT.Transform(input);
            
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void InverseTransformReturnsBANANAFromBNNAAA() {
            byte[] expected = new byte[] {(byte)'^',(byte) 'B', (byte) 'A', (byte) 'N', (byte) 'A', (byte) 'N', (byte) 'A', (byte)'|'};
            byte[] input = new byte[] {(byte) 'B', (byte) 'N', (byte) 'N', (byte) '^', (byte) 'A', (byte) 'A', (byte) '|', (byte) 'A'};
            
            byte[] result = BWT.InverseTransform(input);
            
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void TransformReturnsFromFemFladeFloedeboller() {
            string input = "^fem flade flødeboller på et fladt flødebolle fad|";
            string expected = "åemtter|llfeeaøøaaldddfl  ^    ffllooffebb eeddpll"; //Has not been validated
            
            byte[] inArr = BWT.StringToByteArray(input);
            byte[] output = BWT.Transform(inArr);
            string actual = BWT.ByteArrayToString(output);
            
            Assert.AreEqual(expected, actual);
        }
        
        [Test]
        public void InverseTransformReturnsFromFemFladeFloedeboller() {
            string expected = "^fem flade flødeboller på et fladt flødebolle fad|";
            string input = "åemtter|llfeeaøøaaldddfl  ^    ffllooffebb eeddpll"; //Has not been validated
            
            byte[] inArr = BWT.StringToByteArray(input);
            byte[] output = BWT.InverseTransform(inArr);
            string actual = BWT.ByteArrayToString(output);
            
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TransformFromText1() {
            string input = "^This is some small file, that noone wants to read because it is boring. Therefore it has been hidden here. Please do not share with any hostile intelligence.|";
            string expected = "..hdsseltnnyetseetootes,seeeege  ^|emw hhehc   enaid slrsmnrlrcrlbbrtdeghh enits t T T hltfr  h   wliiPlaesoeeeoi  iaadtsonfbhnoae eoaiiitau   oiiaoni s na  n."; //Has not been validated
            
            byte[] inArr = BWT.StringToByteArray(input);
            byte[] output = BWT.Transform(inArr);
            string actual = BWT.ByteArrayToString(output);
            
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void InverseTransformFromText1() {
            string expected = "^This is some small file, that noone wants to read because it is boring. Therefore it has been hidden here. Please do not share with any hostile intelligence.|";
            string input = "..hdsseltnnyetseetootes,seeeege  ^|emw hhehc   enaid slrsmnrlrcrlbbrtdeghh enits t T T hltfr  h   wliiPlaesoeeeoi  iaadtsonfbhnoae eoaiiitau   oiiaoni s na  n."; //Has not been validated
            
            byte[] inArr = BWT.StringToByteArray(input);
            byte[] output = BWT.InverseTransform(inArr);
            string actual = BWT.ByteArrayToString(output);
            
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ByteArrayToStringFromFile3() {
            string path = TestContext.CurrentContext.TestDirectory + "../../../res/testfile1";
            DataFile input_file = new DataFile(path);
            
        }
    }
    
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
            
            ByteArrayPrinter.PrintBits(actualFile.GetBytes(0, actualFile.Length));
            
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
            EncodedByte pb = new PointerByte(150,9);

            uint actual = pb.ToUnevenBits().Data;
            
            Assert.AreEqual(expected, actual);
        }
        [Test]
        public void ToUnevenBitsRawByte_189_returns_189() {
            uint expected = 189;
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
            
            uint actual = FindMatchingBytes.FindArraySegment(haystack, needleAsSegment).Value;
            
            Assert.AreEqual(expected, actual);
        }          
        [Test]        
        public void FindArraySegmentDoNotFindNeedle() {
            byte[] haystack = {97, 98, 99, 100, 101, 114, 101, 115, 117, 108, 116, 100, 105, 110, 102, 97, 114};
            byte[] needle = {114, 101, 115, 100, 108, 116};
            uint expected = 5;
            ArraySegment<byte> needleAsSegment= new ArraySegment<byte>(needle,0,needle.Length);
            
            Nullable<uint>  actual = FindMatchingBytes.FindArraySegment(haystack, needleAsSegment);
            
            Assert.AreNotEqual(expected, actual);
        }
        [Test]        
        public void FindArraySegmentDoNotFindEmptyHaystack() {
            byte[] haystack = {};
            byte[] needle = {114, 101, 115, 117, 108, 116};
            Nullable<uint> expected = null;
            ArraySegment<byte> needleAsSegment= new ArraySegment<byte>(needle,0,needle.Length);
            
            Nullable<uint> actual = FindMatchingBytes.FindArraySegment(haystack, needleAsSegment);
            
            Assert.AreEqual(expected, actual);
        }             
        [Test]        
        public void FindArraySegmentDoNotFindEmptyNeedle() {
            byte[] haystack = {97, 98, 99, 100, 101, 114, 101, 115, 117, 108, 116, 100, 105, 110, 102, 97, 114};
            byte[] needle = {};
            ArraySegment<byte> needleAsSegment= new ArraySegment<byte>(needle,0,needle.Length);
            
            Nullable<uint>  actual = FindMatchingBytes.FindArraySegment(haystack, needleAsSegment);
            
            Assert.IsNull(actual);
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
        public void FindMatchingBytesFindNeedleAsFirstElementInHistory() {
            byte[] haystack = {97, 98, 99, 102, 152};
            byte[] needle = {97, 98, 99, 102};
            Nullable<MatchPointer> expected = new MatchPointer(0, (uint) 4);

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

        [Test]
        public void FindMathingBytesFindsDuplicateABCD() {
            byte[] haystack = {97, 98, 99, 100, 97, 98, 99, 100};
            byte[] needle = {97, 98, 99, 100};
            MatchPointer expected = new MatchPointer(0,4);

            Nullable<MatchPointer> actual = FindMatchingBytes.FindLongestMatch(haystack, needle);
            
            Assert.AreEqual(expected, actual);
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
        [Test]
        public void SlideNotAtEndWhenInBeginningOfFile() {
            string path = TestContext.CurrentContext.TestDirectory + "../../../res/testfile1";
            DataFile file  = new DataFile(path);
            SlidingWindow sw = new SlidingWindow(file);
            
            Assert.IsFalse(sw.AtEnd());
        }
        [Test]
        public void SlideAtEndWhenAtEnd() {
            string path = TestContext.CurrentContext.TestDirectory + "../../../res/testfile1";
            DataFile file  = new DataFile(path);
            SlidingWindow sw = new SlidingWindow(file);

            for (int i = 0; i < 3; i++)
                sw.Slide();
            
            Assert.IsTrue(sw.AtEnd());
        }

        [Test]
        public void SlideReturnsPointer_3_FromTestFile3AtPos4() {
            string path = TestContext.CurrentContext.TestDirectory + "../../../res/testfile3";
            DataFile file  = new DataFile(path);
            SlidingWindow sw = new SlidingWindow(file);
            uint expected = 3;

            for (int i = 0; i < 4; i++)
                sw.Slide();
            PointerByte actual = (PointerByte)sw.Slide();
            
            Assert.AreEqual(expected, actual.Pointer);
        }
        
        [Test]
        public void SlideReturnsPointerLength_3_FromTestFile3AtPos4() {
            string path = TestContext.CurrentContext.TestDirectory + "../../../res/testfile3";
            DataFile file  = new DataFile(path);
            SlidingWindow sw = new SlidingWindow(file);
            uint expected = 3;

            for (int i = 0; i < 4; i++)
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

    [TestFixture, Category("ByteChangeEncoding")]
    public class ByteChangeEncodingTests {
        [Test]
        public void AddsEntries_AAABAAABAA_as_ABABA_1001100110() {
            byte[] input = BWT.StringToByteArray("AAABAAABAA");
            byte[] expected = new byte[7];
            byte[] a = BWT.StringToByteArray("ABABA");
            byte[] b = {153, 128};
            a.CopyTo(expected, 0);
            b.CopyTo(expected, a.Length);

            byte[] actual = ByteChangeEncoder.EncodeBytes(input).ToBytes();
            
            Assert.AreEqual(expected, actual);
        }
    }
}
            