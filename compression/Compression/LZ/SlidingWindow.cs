using System;
using Compression.ByteStructures;

namespace Compression.LZ {
    public class SlidingWindow : DataFileIterator{
        private int _historyLength = PointerByte.GetPointerSpan();
        private int _lookAheadLength = PointerByte.GetLengthSpan();
        private FindLongestMatch findLongestMatch;
        
        private int _referenceCounts = 0;
        private int _smallReferenceCounts = 0;
        private int _referenceSpan = 0;
        private int _counts = 0;
        
        public SlidingWindow(DataFile file) : base(file) {
            findLongestMatch = CFindMatchingBytes.FindLongestMatch;
        }
        
        public EncodedLZByte Slide() {
            if (AtEnd())
                return null;
            
            var history = LoadHistory(_historyLength);
            var lookAhead = LoadLookAhead(_lookAheadLength);

            var smallHistory = LoadHistory(128);
            var smallLookAhead = LoadLookAhead(4);
            
            MatchPointer smallMatch = findLongestMatch(smallHistory, smallLookAhead);
            if (smallMatch.Length != 0)
                _smallReferenceCounts++;

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

                _referenceCounts++;
                _referenceSpan += match.Length;
            }
            else {
                r = new RawByte(lookAhead[0]);
                currentIndex++;                
            }

            _counts++;
            
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

        public void PrintProbabilities() {
            Console.WriteLine($"\nSmall reference probability: {(double)_smallReferenceCounts / _referenceCounts}");
            Console.WriteLine($"Pointer percentage: {(double)_referenceCounts / file.Length}");
            Console.WriteLine($"Raw percentage: {(double)(_counts-_referenceCounts)/ file.Length}");
            Console.WriteLine($"Reference span: {(double)_referenceSpan / file.Length}");
            Console.WriteLine($"Pointer percentage2: {(double)_referenceCounts / _counts}");
            Console.WriteLine($"Average reference length: {(double)_referenceSpan / _referenceCounts}");
        }
    }
}