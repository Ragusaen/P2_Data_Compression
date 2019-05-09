using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Contexts;
using compression.ByteStructures;
using Compression.ByteStructures;

namespace Compression.LZ {
    public class CFindMatchingBytes {
        [DllImport("findmatchingbytes", EntryPoint = "find_longest_match")]
        public static extern MatchPointer FindLongestMatch(ArrayIndexer<byte> haystack, ArrayIndexer<byte> needle);
    }
}