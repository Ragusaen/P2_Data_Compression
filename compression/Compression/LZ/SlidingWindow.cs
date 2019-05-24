using System;
using Compression.ByteStructures;

namespace Compression.LZ {
    public class SlidingWindow : DataFileIterator{
        private int _historyLength = PointerByte.GetPointerSpan();
        private int _lookAheadLength = PointerByte.GetLengthSpan();
        private FindLongestMatch findLongestMatch;
        
        public SlidingWindow(DataFile file) : base(file) {
            findLongestMatch = CFindMatchingBytes.FindLongestMatch;
        }
        
        public EncodedLZByte Slide() {
            if (AtEnd())
                return null;
            
            var history = LoadHistory(_historyLength);
            var lookAhead = LoadLookAhead(_lookAheadLength);

            MatchPointer match;
            try {
                match = findLongestMatch(history, lookAhead);
            } catch (Exception exception) { // If the loading went bad
                if (exception is DllNotFoundException ||
                    exception is System.Runtime.InteropServices.MarshalDirectiveException) {
                    // Change to using the C# implementation
                    findLongestMatch = FindMatchingBytes.FindLongestMatch;
                    match = findLongestMatch(history, lookAhead);
                }
                else {
                    throw;
                }
            }
            
            EncodedLZByte r;
            if (match.Length != 0) {
                r = new PointerByte( history.Length - match.Index - 1, match.Length - 1);
                currentIndex += match.Length;
            }
            else {
                r = new RawByte(lookAhead[0]);
                currentIndex++;                
            }

            return r;
        }

        private ByteArrayIndexer LoadHistory(int length) {
            int historyIndex;
            if(length > currentIndex) {
                historyIndex = 0;
            }
            else {
                historyIndex = currentIndex - length;
            }
            return file.GetArrayIndexer(historyIndex, currentIndex - historyIndex);
        }

        private ByteArrayIndexer LoadLookAhead(int length) {
            if (length + currentIndex > file.Length)
                length = (int)file.Length - currentIndex;
            return file.GetArrayIndexer(currentIndex, length);
        }
    }
}