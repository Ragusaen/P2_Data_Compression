using System.Collections.Generic;
using Compression.ByteStructures;

namespace Compression.Huffman
{
    public class HuffmanDecoder {
        private readonly Dictionary<UnevenByte, byte> _decodeDictionary = new Dictionary<UnevenByte, byte>();
        private readonly BitIndexer _bitIndexer;

        public HuffmanDecoder(byte[] input) {
            _bitIndexer = new BitIndexer(input);
            
            // Remove filler ones
            while (_bitIndexer.GetNext() == UnevenByte.One) { }
            _bitIndexer.GoToPrevious(); // Go back, because we read the first 0
            
            AddDictionaryEntries(default(UnevenByte));
        }

        private void AddDictionaryEntries(UnevenByte code) {
            if (_bitIndexer.GetNext() == UnevenByte.Zero) {
                AddDictionaryEntries(code + UnevenByte.Zero);
                AddDictionaryEntries(code + UnevenByte.One);
            }
            else {
                byte b = (byte) _bitIndexer.GetNextRange(8).Data;
                _decodeDictionary.Add(code, b);
            }
        }

        public byte[] Decode() {
            int shortestKey = 8;

            foreach (var decodeDictionaryKey in _decodeDictionary.Keys) {
                if (decodeDictionaryKey.Length < shortestKey) {
                    shortestKey = decodeDictionaryKey.Length;
                }
            }

            List<byte> output = new List<byte>();
            UnevenByte ub = default(UnevenByte);

            while (!_bitIndexer.AtEnd()) {
                ub += ub == default(UnevenByte)? _bitIndexer.GetNextRange(shortestKey) : _bitIndexer.GetNext();

                if (_decodeDictionary.ContainsKey(ub)) {
                    output.Add(_decodeDictionary[ub]);
                    ub = default(UnevenByte);
                }
            }
            return output.ToArray();
        }
    }
}
