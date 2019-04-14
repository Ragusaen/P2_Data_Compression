using System;
using compression.ByteStructures;
using Compression.ByteStructures;

namespace Compression.LZ {
    public class SlidingWindow{
        private DataFile file;
        private uint currentIndex = 0;
        private uint historyLength = PointerByte.GetPointerSpan();
        private uint lookAheadLength = PointerByte.GetLengthSpan();

        public SlidingWindow(DataFile file) {
            this.file = file;
        }

        public EncodedLZByte Slide() {
            if (AtEnd())
                return null;
            
            var history = LoadHistory();
            var lookAhead = LoadLookAhead();
            
            MatchPointer? match = FindMatchingBytes.FindLongestMatch(history, lookAhead);
            
            EncodedLZByte r;
            if (match.HasValue) {
                r = new PointerByte((uint) history.Length - match.Value.Offset - 1, match.Value.Length - 1);
                currentIndex += match.Value.Length;
            }
            else {
                r = new RawByte(lookAhead[0]);
                currentIndex++;                
            }
            
            // Console print
            if (currentIndex % 100 == 0) {
                string str = (Math.Truncate((decimal) currentIndex / file.Length * 10000)/100).ToString();
                Console.Write("\rPercentage complete: "  + str + "%   ");
            }

            return r;
        }
        
        private ArrayIndexer<byte> LoadHistory() {
            uint historyIndex;
            if(historyLength > currentIndex) {
                historyIndex = 0;
            }
            else {
                historyIndex = currentIndex - historyLength;
            }
            return file.GetArrayIndexer((int)historyIndex, (int)currentIndex - (int)historyIndex);
        }

        private ArrayIndexer<byte> LoadLookAhead() {
            if (lookAheadLength + currentIndex > file.Length)
                lookAheadLength = file.Length - currentIndex;
            return file.GetArrayIndexer((int)currentIndex, (int)lookAheadLength);
        }
    
        public Boolean AtEnd() {
            return currentIndex >= file.Length;
        }
    }
}