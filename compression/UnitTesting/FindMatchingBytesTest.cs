using Compression;
using Compression.ByteStructures;
using Compression.LZ;
using NUnit.Framework;

namespace UnitTesting {
    [TestFixture]
    [Category("FindMatchingBytes")]
    public class TestFindMatchingBytes {
        [Test]
        public void CFind_fl_in_femFlade_FromLongNeedle() {
            var haystack = ByteMethods.StringToByteArray("fem flade");
            var needle = ByteMethods.StringToByteArray(" flødeboller på");
            var expected = new MatchPointer(3, 3);

            MatchPointer? actual = CFindMatchingBytes.FindLongestMatch(
                new ByteArrayIndexer(haystack, 0, haystack.Length),
                new ByteArrayIndexer(needle, 0, needle.Length));

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CFind_p_m_In_hvad_op_mine_hjemmedrenge() {
            var haystack = ByteMethods.StringToByteArray("hvad op mine hjemmedrenge?");
            var needle = ByteMethods.StringToByteArray("p manden?");
            var expected = new MatchPointer(6, 3);

            MatchPointer? actual = CFindMatchingBytes.FindLongestMatch(
                new ByteArrayIndexer(haystack, 0, haystack.Length),
                new ByteArrayIndexer(needle, 0, needle.Length));

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Find_fl_in_femFlade() {
            var haystack = ByteMethods.StringToByteArray("fem flade");
            var needle = ByteMethods.StringToByteArray(" fl");
            var expected = new MatchPointer(3, 3);

            MatchPointer? actual = FindMatchingBytes.FindLongestMatch(
                new ByteArrayIndexer(haystack, 0, haystack.Length),
                new ByteArrayIndexer(needle, 0, needle.Length));

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Find_fl_in_femFlade_FromLongNeedle() {
            var haystack = ByteMethods.StringToByteArray("fem flade");
            var needle = ByteMethods.StringToByteArray(" flødeboller på");
            var expected = new MatchPointer(3, 3);

            MatchPointer? actual = FindMatchingBytes.FindLongestMatch(
                new ByteArrayIndexer(haystack, 0, haystack.Length),
                new ByteArrayIndexer(needle, 0, needle.Length));

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void FindMatchingBytesDoNotFindAnyMatchEmptyHaystack() {
            byte[] haystack = { };
            byte[] needle = {86, 102, 59, 108};
            var expected = new MatchPointer(0, 0);

            MatchPointer? actual = FindMatchingBytes.FindLongestMatch(
                new ByteArrayIndexer(haystack, 0, haystack.Length),
                new ByteArrayIndexer(needle, 0, needle.Length));

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void FindMatchingBytesDoNotFindAnyMatchEmptyNeedle() {
            byte[] haystack = {97, 98, 99, 100, 101, 114, 101, 115, 117, 108, 116, 100, 105, 110, 102, 97, 114};
            byte[] needle = { };
            var expected = new MatchPointer(0, 0);

            MatchPointer? actual = FindMatchingBytes.FindLongestMatch(
                new ByteArrayIndexer(haystack, 0, haystack.Length),
                new ByteArrayIndexer(needle, 0, needle.Length));

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void FindMatchingBytesDoNotFindAnyMatchReturnsNull() {
            byte[] haystack = {97, 98, 99, 100, 101, 114, 101, 115, 117, 108, 116, 100, 105, 110, 102, 97, 114};
            byte[] needle = {86, 102, 59, 108};
            var expected = new MatchPointer(0, 0);

            var actual = FindMatchingBytes.FindLongestMatch(
                new ByteArrayIndexer(haystack, 0, haystack.Length),
                new ByteArrayIndexer(needle, 0, needle.Length));

            Assert.AreEqual(expected, actual);
        }
        [Test]
        public void FindMatchingBytesFindFullLengthNeedle() {
            byte[] haystack = {97, 98, 99, 100, 101, 114, 101, 115, 117, 108, 116, 100, 105, 110, 102, 97, 114};
            byte[] needle = {114, 101, 115, 117, 108, 116};
            MatchPointer? expected = new MatchPointer(5, 6);

            MatchPointer? actual = FindMatchingBytes.FindLongestMatch(
                new ByteArrayIndexer(haystack, 0, haystack.Length),
                new ByteArrayIndexer(needle, 0, needle.Length));

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void FindMatchingBytesFindNeedleAsFirstElementInHistory() {
            byte[] haystack = {97, 98, 99, 102, 152};
            byte[] needle = {97, 98, 99, 102};
            MatchPointer? expected = new MatchPointer(0, 4);

            MatchPointer? actual = FindMatchingBytes.FindLongestMatch(
                new ByteArrayIndexer(haystack, 0, haystack.Length),
                new ByteArrayIndexer(needle, 0, needle.Length));

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void FindMatchingBytesFindSemiLengthNeedle() {
            byte[] haystack = {97, 98, 99, 100, 101, 114, 101, 115, 117, 107, 116, 100, 105, 110, 102, 97, 114};
            byte[] needle = {114, 101, 115, 117, 108, 116};
            MatchPointer? expected = new MatchPointer(5, 4);

            MatchPointer? actual = FindMatchingBytes.FindLongestMatch(
                new ByteArrayIndexer(haystack, 0, haystack.Length),
                new ByteArrayIndexer(needle, 0, needle.Length));

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void FindMathingBytesFindsDuplicateABCD() {
            byte[] haystack = {97, 98, 99, 100, 97, 98, 99, 100};
            byte[] needle = {97, 98, 99, 100};
            var expected = new MatchPointer(0, 4);

            MatchPointer? actual = FindMatchingBytes.FindLongestMatch(
                new ByteArrayIndexer(haystack, 0, haystack.Length),
                new ByteArrayIndexer(needle, 0, needle.Length));

            Assert.AreEqual(expected, actual);
        }
    }
}