using System.Runtime.InteropServices;
using Compression.ByteStructures;

namespace Compression.LZ {
    /// <summary>
    ///     This class allows running unmanaged C code for increased efficiency. See FindMathcingBytes for more
    ///     on how this works.
    /// </summary>
    public class CFindMatchingBytes {
        [DllImport("findmatchingbytes", EntryPoint = "find_longest_match", CallingConvention = CallingConvention.Cdecl)]
        public static extern MatchPointer FindLongestMatch(ByteArrayIndexer haystack, ByteArrayIndexer needle);
    }
}