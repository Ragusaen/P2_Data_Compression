using System;
using System.Linq;
using compression.ByteStructures;
using Compression.ByteStructures;

namespace Compression.LZ {
    public struct MatchPointer {
        public readonly int Index;
        public readonly int Length;

        public MatchPointer(int index, int length) {
            Index = index;
            Length = length;
        }

        public override string ToString() {
            return "(" + Index + ", " + Length + ")";
        }
    }

    public static class FindMatchingBytes{
        public static MatchPointer FindLongestMatch(ArrayIndexer<byte> haystack, ArrayIndexer<byte> needle) {
            int longestMatch = 1;
            int indexOfLongestMatch = 0;
            
            //Find the longest match in the haystack
            for (int i = 0; i < haystack.Length - longestMatch; ++i) {
                int matchedBytes = MatchingBytesCount(haystack, i, needle);
                if (matchedBytes > longestMatch) {
                    longestMatch = matchedBytes;
                    indexOfLongestMatch = i;
                }
            }

            return (longestMatch > 1)? new MatchPointer(indexOfLongestMatch, longestMatch):
                                       default(MatchPointer);
        }
        
        public static int MatchingBytesCount(ArrayIndexer<byte> a, int index, ArrayIndexer<byte> b ) {
            int i = 0;
            for (; i < b.Length && index + i < a.Length; ++i)
                if (a[index + i] != b[i])
                    return i;
            return i;
        }
    }
}