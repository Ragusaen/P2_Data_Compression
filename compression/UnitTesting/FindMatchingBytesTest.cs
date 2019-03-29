using System;
using Compression;
using NUnit.Framework;

namespace UnitTesting {
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
}