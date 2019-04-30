using System;
using System.ComponentModel;
using compression.ByteStructures;
using Compression;
using Compression.ByteStructures;
using Compression.LZ;
using NUnit.Framework;

namespace UnitTesting {
    [TestFixture, NUnit.Framework.Category("FindMatchingBytes")]
    public class TestFindMatchingBytes {
        /*
        [Test]
        public void CompareArrayIndexersFrom0_True() {
            byte[] first = {102, 103, 104};
            byte[] second = {102, 103, 104};
            var firstIndexer = new ArrayIndexer<byte>(first, 0, first.Length);
            var secondIndexer = new ArrayIndexer<byte>(second, 0, second.Length);

            Boolean actual = FindMatchingBytes.ByteArrayEqualityByIndexing(firstIndexer, 0, secondIndexer);
            
            Assert.IsTrue(actual);
        }
        
        [Test]
        public void CompareArrayIndexersFrom0_False() {
            byte[] first = {102, 102, 104};
            byte[] second = {102, 103, 104};
            var firstIndexer = new ArrayIndexer<byte>(first, 0, first.Length);
            var secondIndexer = new ArrayIndexer<byte>(second, 0, second.Length);

            Boolean actual = FindMatchingBytes.ByteArrayEqualityByIndexing(firstIndexer, 0, secondIndexer);
            
            Assert.IsFalse(actual);
        }
        
        [Test]
        public void CompareArrayIndexersFromMid_True() {
            byte[] first = {102, 103, 104, 105, 97, 125, 38, 17, 97};
            byte[] second = {105, 97, 125};
            var firstIndexer = new ArrayIndexer<byte>(first, 0, first.Length);
            var secondIndexer = new ArrayIndexer<byte>(second, 0, second.Length);

            Boolean actual = FindMatchingBytes.ByteArrayEqualityByIndexing(firstIndexer, 3, secondIndexer);
            
            Assert.IsTrue(actual);
        }
        
        [Test]
        public void CompareArrayIndexersFromMid_False() {
            byte[] first = {102, 103, 104, 105, 97, 125, 38, 17, 97};
            byte[] second = {105, 98, 125};
            var firstIndexer = new ArrayIndexer<byte>(first, 0, first.Length);
            var secondIndexer = new ArrayIndexer<byte>(second, 0, second.Length);

            Boolean actual = FindMatchingBytes.ByteArrayEqualityByIndexing(firstIndexer, 3, secondIndexer);
            
            Assert.IsFalse(actual);
        }
        
        [Test]
        public void CompareArrayIndexersFromMidWithOffset_True() {
            byte[] first = {102, 103, 104, 105, 97, 125, 38, 17, 97};
            byte[] second = {105, 97, 125};
            var firstIndexer = new ArrayIndexer<byte>(first, 2, 5);
            var secondIndexer = new ArrayIndexer<byte>(second, 0, second.Length);

            Boolean actual = FindMatchingBytes.ByteArrayEqualityByIndexing(firstIndexer, 1, secondIndexer);
            
            Assert.IsTrue(actual);
        }
        
        [Test]
        public void CompareArrayIndexersFromMidWithOffset_False() {
            byte[] first = {102, 103, 104, 105, 97, 125, 38, 17, 97};
            byte[] second = {105, 98, 125};
            var firstIndexer = new ArrayIndexer<byte>(first, 2, first.Length);
            var secondIndexer = new ArrayIndexer<byte>(second, 0, second.Length);

            Boolean actual = FindMatchingBytes.ByteArrayEqualityByIndexing(firstIndexer, 1, secondIndexer);
            
            Assert.IsFalse(actual);
        }
        */
        [Test]
        public void FindMatchingBytesFindFullLengthNeedle() {
            byte[] haystack = {97, 98, 99, 100, 101, 114, 101, 115, 117, 108, 116, 100, 105, 110, 102, 97, 114};
            byte[] needle = {114, 101, 115, 117, 108, 116};
            MatchPointer? expected = new MatchPointer(5, (int) 6);

            MatchPointer? actual = FindMatchingBytes.FindLongestMatch(
                new ArrayIndexer<byte>(haystack, 0, haystack.Length),
                new ArrayIndexer<byte>(needle, 0, needle.Length));
            
            Assert.AreEqual(expected, actual);
        }
        [Test]
        public void FindMatchingBytesFindNeedleAsFirstElementInHistory() {
            byte[] haystack = {97, 98, 99, 102, 152};
            byte[] needle = {97, 98, 99, 102};
            MatchPointer? expected = new MatchPointer(0, (int) 4);

            MatchPointer? actual = FindMatchingBytes.FindLongestMatch(
                new ArrayIndexer<byte>(haystack, 0, haystack.Length),
                new ArrayIndexer<byte>(needle, 0, needle.Length));
            
            Assert.AreEqual(expected, actual);
        }
        
        [Test]
        public void FindMatchingBytesFindSemiLengthNeedle() {
            byte[] haystack = {97, 98, 99, 100, 101, 114, 101, 115, 117, 107, 116, 100, 105, 110, 102, 97, 114};
            byte[] needle = {114, 101, 115, 117, 108, 116};
            MatchPointer? expected = new MatchPointer(5, (int) 4);

            MatchPointer? actual = FindMatchingBytes.FindLongestMatch(
                new ArrayIndexer<byte>(haystack, 0, haystack.Length),
                new ArrayIndexer<byte>(needle, 0, needle.Length));
            
            Assert.AreEqual(expected, actual);
        }
        [Test]
        public void FindMatchingBytesDoNotFindAnyMatchReturnsNull() {
            byte[] haystack = {97, 98, 99, 100, 101, 114, 101, 115, 117, 108, 116, 100, 105, 110, 102, 97, 114};
            byte[] needle = {86, 102, 59, 108};
            var expected = new MatchPointer(0,0);

            MatchPointer actual = FindMatchingBytes.FindLongestMatch(
                new ArrayIndexer<byte>(haystack, 0, haystack.Length),
                new ArrayIndexer<byte>(needle, 0, needle.Length));
            
            Assert.AreEqual(expected, actual);
        }
        [Test]
        public void FindMatchingBytesDoNotFindAnyMatchEmptyHaystack() {
            byte[] haystack = {};
            byte[] needle = {86, 102, 59, 108};
            var expected = new MatchPointer(0,0);

            MatchPointer? actual = FindMatchingBytes.FindLongestMatch(
                new ArrayIndexer<byte>(haystack, 0, haystack.Length),
                new ArrayIndexer<byte>(needle, 0, needle.Length));
            
            Assert.AreEqual(expected, actual);
        }
        [Test]
        public void FindMatchingBytesDoNotFindAnyMatchEmptyNeedle() {
            byte[] haystack = {97, 98, 99, 100, 101, 114, 101, 115, 117, 108, 116, 100, 105, 110, 102, 97, 114};
            byte[] needle = {};
            var expected = new MatchPointer(0,0);

            MatchPointer? actual = FindMatchingBytes.FindLongestMatch(
                new ArrayIndexer<byte>(haystack, 0, haystack.Length),
                new ArrayIndexer<byte>(needle, 0, needle.Length));
            
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void FindMathingBytesFindsDuplicateABCD() {
            byte[] haystack = {97, 98, 99, 100, 97, 98, 99, 100};
            byte[] needle = {97, 98, 99, 100};
            MatchPointer expected = new MatchPointer(0,4);

            MatchPointer? actual = FindMatchingBytes.FindLongestMatch(
                new ArrayIndexer<byte>(haystack, 0, haystack.Length),
                new ArrayIndexer<byte>(needle, 0, needle.Length));
            
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Find_fl_in_femFlade() {
            byte[] haystack = ByteMethods.StringToByteArray("fem flade");
            byte[] needle = ByteMethods.StringToByteArray(" fl");
            MatchPointer expected = new MatchPointer(3,3);

            MatchPointer? actual = FindMatchingBytes.FindLongestMatch(
                new ArrayIndexer<byte>(haystack, 0, haystack.Length),
                new ArrayIndexer<byte>(needle, 0, needle.Length));
            
            Assert.AreEqual(expected, actual);
        }
        
        [Test]
        public void Find_fl_in_femFlade_FromLongNeedle() {
            byte[] haystack = ByteMethods.StringToByteArray("fem flade");
            byte[] needle = ByteMethods.StringToByteArray(" flødeboller på");
            MatchPointer expected = new MatchPointer(3,3);

            MatchPointer? actual = FindMatchingBytes.FindLongestMatch(
                new ArrayIndexer<byte>(haystack, 0, haystack.Length),
                new ArrayIndexer<byte>(needle, 0, needle.Length));
            
            Assert.AreEqual(expected, actual);
        }
        
        [Test]
        public void CFind_fl_in_femFlade_FromLongNeedle() {
            byte[] haystack = ByteMethods.StringToByteArray("fem flade");
            byte[] needle = ByteMethods.StringToByteArray(" flødeboller på");
            MatchPointer expected = new MatchPointer(3,3);

            Console.WriteLine(TestContext.CurrentContext.TestDirectory);

            MatchPointer? actual = CFindMatchingBytes.FindLongestMatch(
                new ArrayIndexer<byte>(haystack, 0, haystack.Length),
                new ArrayIndexer<byte>(needle, 0, needle.Length));
            
            Assert.AreEqual(expected, actual);
        }
    }
}