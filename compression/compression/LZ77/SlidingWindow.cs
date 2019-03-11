using System;
using System.Reflection;

namespace compression.LZ77 {
    public class SlidingWindow{
        private byte[] history;
        private byte[] lookAhead;
        private DataFile file;
        private uint currentIndex = 0;
        private uint historyLength = PointerByte.GetPointerSpan();
        private uint lookAheadLength = PointerByte.GetLengthSpan();

        public EncodedByte Slide() {
            if (currentIndex == file.Length())
                return null;
            LoadHistory();
            LoadLookAhead();
            
            return null;
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
        
        public SlidingWindow(DataFile file) {
            this.file = file;
        }
    }
}