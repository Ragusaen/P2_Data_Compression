using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Contexts;
using compression.ByteStructures;

namespace Compression.LZ {
    public class CFindMatchingBytes {

#if TEST
        private const string path = "CWrappers/libfindmatchingbytes.so";
#else
        private const string path = "../../CWrappers/libfindmatchingbytes.so";
#endif
        
        [DllImport(path, EntryPoint = "print")]
        public static extern void print(string message);

        public static MatchPointer FindLongestMatch(ArrayIndexer<byte> haystack, ArrayIndexer<byte> needle) {
            return FindLongestMatchWrapper(haystack.Array, haystack.Index, haystack.Length, needle.Array, needle.Length);
        }

        [DllImport(path, EntryPoint = "find_longest_match")]
        private static extern MatchPointer FindLongestMatchWrapper(byte[] haystack, int haystack_index, int haystack_length, byte[] needle, int needle_length );
    }
}