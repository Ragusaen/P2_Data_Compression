using System.Runtime.InteropServices;
using Compression.ByteStructures;

namespace Compression.LZ {
    public class CFindMatchingBytes {
        [DllImport("findmatchingbytes", EntryPoint = "find_longest_match", CallingConvention = CallingConvention.Cdecl)]
        public static extern MatchPointer FindLongestMatch(ByteArrayIndexer haystack, ByteArrayIndexer needle);
    }
}