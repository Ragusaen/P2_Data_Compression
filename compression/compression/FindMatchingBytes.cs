using System;
using System.Linq;

namespace compression{
    public struct MatchPointer{
        public uint Offset;
        public uint Length;

        public MatchPointer(uint offset, uint length) {
            Offset = offset;
            Length = length;
        }
    }
    
    public class FindMatchingBytes{
        public static MatchPointer? FindLongestMatch(byte[] haystack, byte[] needle) {
            for (int l = needle.Length; l >= 2; l--) {
                ArraySegment<byte> match = new ArraySegment<byte>(needle, 0, l);

                uint? offset = FindArraySegment(haystack, match);
                
                if(offset.HasValue)
                    return new MatchPointer(offset.Value, (uint) l);
            }
            return null;
        }

        public static uint? FindArraySegment(byte[] haystack, ArraySegment<byte> match) {
            if (match.Count == 0)
                return null;
            
            for (int i = 0; i <= haystack.Length - match.Count; i++) {
                ArraySegment<byte> currentArray = new ArraySegment<byte>(haystack, i, match.Count);
                
                if (CompareByteArraySegment(currentArray, match))
                    return (uint) i;
            }
            return null;
        }

        public static Boolean CompareByteArraySegment(ArraySegment<byte> a, ArraySegment<byte> b) {
            if (a.Count != b.Count)
                return false;

            for (int i = 0; i < a.Count; i++)
                if (a.ElementAt(i) != b.ElementAt(i))
                    return false;

            return true;
        }
    }
}