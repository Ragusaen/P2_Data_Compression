using System;
using Compression.ByteStructures;

namespace Compression.LZ {
    public class SlidingWindow : DataFileIterator{
        private int _historyLength = PointerByte.GetPointerSpan();
        private int _lookAheadLength = PointerByte.GetLengthSpan();

        public SlidingWindow(DataFile file) : base(file) {
        }

        public EncodedLZByte Slide() {
            if (AtEnd())
                return null;
            
            var history = LoadHistory();
            var lookAhead = LoadLookAhead();
            
            MatchPointer match = FindMatchingBytes.FindLongestMatch(history, lookAhead);
            
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
            if (currentIndex % 100 == 0) {
                string str = (Math.Truncate((decimal) currentIndex / file.Length * 10000)/100).ToString();
                Console.Write("\rPercentage complete: "  + str + "%   ");
            }

            return r;
        }
        
        private ArrayIndexer<byte> LoadHistory() {
            int historyIndex;
            if(_historyLength > currentIndex) {
                historyIndex = 0;
            }
            else {
                historyIndex = currentIndex - _historyLength;
            }
            return file.GetArrayIndexer(historyIndex, currentIndex - historyIndex);
        }

        private ArrayIndexer<byte> LoadLookAhead() {
            if (_lookAheadLength + currentIndex > file.Length)
                _lookAheadLength = (int)file.Length - currentIndex;
            return file.GetArrayIndexer(currentIndex, _lookAheadLength);
        }
    }
}