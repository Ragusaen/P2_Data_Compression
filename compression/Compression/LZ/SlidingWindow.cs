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
            } catch (TypeLoadException) { // If the loading went bad
                // Change to using the C# implementation
                findLongestMatch = FindMatchingBytes.FindLongestMatch;
                match = findLongestMatch(history, lookAhead);
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
            
            // Console print
            if (currentIndex % 100000 == 0) {
                string str = (Math.Truncate((decimal) currentIndex / file.Length * 10000)/100).ToString();
                Console.Write("\rPercentage complete: "  + str + "%   ");
            }

            return r;
        }

        private ArrayIndexer<byte> LoadHistory(int length) {
            int historyIndex;
            if(length > currentIndex) {
                historyIndex = 0;
            }
            else {
                historyIndex = currentIndex - length;
            }
            return file.GetArrayIndexer(historyIndex, currentIndex - historyIndex);
        }

        private ArrayIndexer<byte> LoadLookAhead(int length) {
            if (length + currentIndex > file.Length)
                length = (int)file.Length - currentIndex;
            return file.GetArrayIndexer(currentIndex, length);
        }
    }
}