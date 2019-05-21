using System;
using Compression.ByteStructures;

namespace PDC {
    public class ContextWindow {
        private uint _historyLength;
        private uint _contextLength;
        private uint _predictionLength;
        private uint _currentIndex;

        private byte[] _data;

        public ContextWindow( uint historyLength, uint contextLength, uint predictionLength) {
            _historyLength = historyLength;
            _contextLength = contextLength;
            _predictionLength = predictionLength;
            _currentIndex = 0;
            _data = new byte[0];
        }

        public EncodedByte EncodeByte() {
            throw new NotImplementedException();
            //FindMatchingBytes.FindLongestMostProbableSequenceMatch();
        }

        public void Reset() {
            _currentIndex = 0;
        }

        public void load_data(byte[] data) {
            _data = data;
        }
    }
}