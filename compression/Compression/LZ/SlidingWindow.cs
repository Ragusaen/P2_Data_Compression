using System;
using System.Runtime.InteropServices;
using Compression.ByteStructures;

namespace Compression.LZ {
    /// <summary>
    ///     Sliding window which has 2 windows, one looking forward and one looking backwards. This is used for
    ///     finding the bytes to encode with LZSS.
    /// </summary>
    public class SlidingWindow {
        private readonly byte[] _bytes;
        private int _currentIndex;
        private readonly int _historyLength = PointerByte.GetPointerSpan();
        private readonly int _lookAheadLength = PointerByte.GetLengthSpan();
        private FindLongestMatch findLongestMatch;

        public SlidingWindow(byte[] bytes) {
            _bytes = bytes;
            findLongestMatch = CFindMatchingBytes.FindLongestMatch;
        }

        public bool AtEnd() {
            return _currentIndex >= _bytes.Length;
        }

        /// <summary>
        ///     This method creates the windows and finds longest match from lookahead in history. If one
        ///     longer than 2 is found, then a PointerByte is returned indicating where the match is relative
        ///     to the current index.
        /// </summary>
        /// <returns> EncodedLZByte that is either a pointer to match or raw data. </returns>
        public EncodedLZByte Slide() {
            if (AtEnd())
                return null;

            var history = LoadHistory(_historyLength);
            var lookAhead = LoadLookAhead(_lookAheadLength);

            MatchPointer match;
            try {
                match = findLongestMatch(history, lookAhead);
            }
            catch (Exception exception) {
                // If the loading went bad
                if (exception is DllNotFoundException ||
                    exception is MarshalDirectiveException) {
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
                r = new PointerByte(history.Length - match.Index - 1, match.Length - 1);
                _currentIndex += match.Length;
            }
            else {
                r = new RawByte(lookAhead[0]);
                _currentIndex++;
            }

            return r;
        }

        private ByteArrayIndexer LoadHistory(int length) {
            int historyIndex;
            if (length > _currentIndex)
                historyIndex = 0;
            else
                historyIndex = _currentIndex - length;
            return new ByteArrayIndexer(_bytes, historyIndex, _currentIndex - historyIndex);
        }

        private ByteArrayIndexer LoadLookAhead(int length) {
            if (length + _currentIndex > _bytes.Length)
                length = _bytes.Length - _currentIndex;
            return new ByteArrayIndexer(_bytes, _currentIndex, length);
        }

        public int GetStatus() {
            return _currentIndex;
        }
    }
}