using System;
using System.Linq;
using compression.ByteStructures;

namespace Compression{
    public struct MatchPointer{
        public uint Offset;
        public uint Length;

        public MatchPointer(uint offset, uint length) {
            Offset = offset;
            Length = length;
        }

        public override string ToString() {
            return "(" + Offset + ", " + Length + ")";
        }
    }

    public class FindMatchingBytes{
        public static MatchPointer? FindLongestMatch(ArrayIndexer<byte> haystack, ArrayIndexer<byte> needle) {
            for (; needle.Length >= 2; --needle.Length) {
                uint? offset = FindMatch(haystack, needle);
                
                if(offset.HasValue)
                    return new MatchPointer(offset.Value, (uint) needle.Length);
            }
            return null;
        }

        public static uint? FindMatch(ArrayIndexer<byte> haystack, ArrayIndexer<byte> match) {
            for (int i = 0; i <= haystack.Length - match.Length; i++) {
                if (CompareByteArraysByIndexing(haystack, i, match))
                    return (uint) i;
            }
            return null;
        }

        public static Boolean CompareByteArraysByIndexing(ArrayIndexer<byte> a, int index, ArrayIndexer<byte> b ) {
            for (int i = 0; i < b.Length; ++i)
                if (a[index + i] != b[i])
                    return false;

            return true;
        }
    }
}