using System;
using System.Reflection;

namespace compression.LZ77 {
    public class SlidingWindow{
        private byte[] history;
        private byte[] lookAhead;
        private DataFile file;
        private uint currentIndex = 0;
        private uint historyLength = PointerByte.GetPointerSpan();
        private uint lookAheadLength = PointerByte.GetLengtSpan();
        
        public SlidingWindow(DataFile file) {
            this.file = file;
        }

        public EncodedByte Slide() {
            if (currentIndex == file.Length())
                return null;
            LoadHistory();
            LoadLookAhead();
            
            Nullable<MatchPointer> match = FindMatchingBytes.FindLongestMatch(history, lookAhead);

            EncodedByte r;
            if (match != null) {
                r = new PointerByte((uint) history.Length - match.Value.Offset, match.Value.Length);
                currentIndex += match.Value.Length;
            }
            else {
                r = new RawByte(lookAhead[0]);
                currentIndex++;                
            }

            return r;
        }
        
        private void LoadHistory() {
            uint historyIndex;
            if(historyLength > currentIndex) {
                historyIndex = 0;
            }
            else {
                historyIndex = currentIndex - historyLength;
            }
            
            history = file.GetBytes(historyIndex, currentIndex - historyIndex);
            
        }

        private void LoadLookAhead() {
            if (lookAheadLength + currentIndex > file.Length())
                lookAheadLength = file.Length() - currentIndex;
            lookAhead = file.GetBytes(currentIndex, lookAheadLength);
        }


    }
}