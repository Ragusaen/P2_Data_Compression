using System;
using System.Collections.Generic;
using Compression.ByteStructures;

namespace Compression.Huffman {
    public class HuffmanDecoder{
        public int RemainingBits;
        
        /// <summary>
        /// This class implements Huffman decoding. It creates a decoding dictionary from BitIndexer until
        /// Huffman tree is complete. 
        /// </summary>
        private readonly Dictionary<UnevenByte, byte> _decodeDictionary = new Dictionary<UnevenByte, byte>();
        private readonly BitIndexer _bitIndexer;

        public HuffmanDecoder(byte[] input) {
            _bitIndexer = new BitIndexer(input);
            
            // Remove filler ones
            while (_bitIndexer.GetNext() == UnevenByte.One) { }
            _bitIndexer.GoToPrevious(); // Go back, because we read the first 0
            
            AddDictionaryEntries(default(UnevenByte));
        }

        /// <summary>
        /// This method is a recursive method. It creates a decoding dictionary from BitIndexer until
        /// the Huffman tree is complete. A 0 bit indicate a 'branch' and a 1 bit indicate a 'leaf'.
        /// </summary>
        /// <param name="code"> It is the decoding code that gets inherit to a 'leaf'. </param>
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

        /// <summary>
        /// This method goes through the rest of the BitIndexer and adds bits till an UnevenByte until
        /// it matches with the decode dictionary where it adds them till a list of byte. When the 
        /// BitIndexer has been through all bits the list gets converted to a byte array.
        /// </summary>
        /// <returns> The decoded file as a byte array. </returns>
        public byte[] Decode() {
            var shortestKey = 8;
            foreach (var decodeDictionaryKey in _decodeDictionary.Keys) {
                if (decodeDictionaryKey.Length < shortestKey) {
                    shortestKey = decodeDictionaryKey.Length;
                }
            }

            var output = new List<byte>();
            var ub = default(UnevenByte);

            while (!_bitIndexer.AtEnd()) {
                RemainingBits = _bitIndexer.Remaining;
                
                ub += ub == default(UnevenByte)? _bitIndexer.GetNextRange(shortestKey) : _bitIndexer.GetNext();

                if (_decodeDictionary.ContainsKey(ub)) {
                    output.Add(_decodeDictionary[ub]);
                    ub = default(UnevenByte);
                }
            }

            RemainingBits = 0;
            return output.ToArray();
        }
    }
}