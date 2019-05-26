using Compression.ByteStructures;

namespace Compression.LZ {
    /// <summary>
    /// Delegate to allow switching between using the C and C# implementation.
    /// </summary>
    public delegate MatchPointer FindLongestMatch(ByteArrayIndexer haystack, ByteArrayIndexer needle);
    
    /// <summary>
    /// Description of the match that has been found. If no match, then Length should be 0.
    /// </summary>
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
        /// <summary>
        /// This method finds the longest match of the needle in the haystack.
        /// </summary>
        /// <param name="haystack"> Large array to search in. </param>
        /// <param name="needle"> Needle to search after. </param>
        /// <returns> A MatchPointer describing the index of the needle and the length of the match. </returns>
        public static MatchPointer FindLongestMatch(ByteArrayIndexer haystack, ByteArrayIndexer needle) {
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
        
        /// <summary>
        /// Find the length of the sequence of bytes that are equal from array a, starring at index index,
        /// and array b, starting at 0.
        /// </summary>
        /// <param name="a"> Larger array to look at from index. </param>
        /// <param name="index"> Index in a to look for match. </param>
        /// <param name="b"> Smaller array to match in a at index. </param>
        /// <returns></returns>
        public static int MatchingBytesCount(ByteArrayIndexer a, int index, ByteArrayIndexer b ) {
            int i = 0;
            for (; i < b.Length && index + i < a.Length; ++i)
                if (a[index + i] != b[i])
                    return i;
            return i;
        }
    }
}