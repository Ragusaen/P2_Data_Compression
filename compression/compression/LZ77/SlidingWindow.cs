using System;
using System.Linq;
using System.Reflection;

namespace compression.LZ77 {
    public class SlidingWindow{
        private DataFile file;
        private uint currentIndex = 0;
        private uint historyLength = PointerByte.GetPointerSpan();
        private uint lookAheadLength = PointerByte.GetLengthSpan();

        public SlidingWindow(DataFile file) {
            this.file = file;
        }

        public EncodedByte Slide() {
            if (AtEnd())
                return null;
            
            byte[] history = LoadHistory();
            byte[] lookAhead = LoadLookAhead();
            
            MatchPointer? match = FindMatchingBytes.FindLongestMatch(history, lookAhead);
            
            EncodedByte r;
            if (match.HasValue) {
                r = new PointerByte((uint) history.Length - match.Value.Offset - 1, match.Value.Length - 1);
                currentIndex += match.Value.Length;
            }
            else {
                r = new RawByte(lookAhead[0]);
                currentIndex++;                
            }

            return r;
        }
        
        private byte[] LoadHistory() {
            uint historyIndex;
            if(historyLength > currentIndex) {
                historyIndex = 0;
            }
            else {
                historyIndex = currentIndex - historyLength;
            }
            
            return file.GetBytes(historyIndex, currentIndex - historyIndex);
        }

        private byte[] LoadLookAhead() {
            if (lookAheadLength + currentIndex > file.Length)
                lookAheadLength = file.Length - currentIndex;
            return file.GetBytes(currentIndex, lookAheadLength);
        }

        public Boolean AtEnd() {
            return currentIndex >= file.Length;
        }
    }
}