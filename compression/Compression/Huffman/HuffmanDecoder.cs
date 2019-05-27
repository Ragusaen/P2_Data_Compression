using System.Collections.Generic;
using Compression.ByteStructures;

namespace Compression.Huffman {
    public class HuffmanDecoder {
        private readonly BitIndexer bitIndexer;
        private readonly Dictionary<UnevenByte, byte> DecodeDictionary = new Dictionary<UnevenByte, byte>();

        public HuffmanDecoder(byte[] input) {
            bitIndexer = new BitIndexer(input);

            // Remove filler ones
            while (bitIndexer.GetNext() == UnevenByte.One) ;
            bitIndexer.GoToPrevious(); // Go back, because we read the first 0

            AddDictionaryEntries(default(UnevenByte));
        }

        private void AddDictionaryEntries(UnevenByte code) {
            if (bitIndexer.GetNext() == UnevenByte.Zero) {
                AddDictionaryEntries(code + UnevenByte.Zero);
                AddDictionaryEntries(code + UnevenByte.One);
            }
            else {
                var b = (byte) bitIndexer.GetNextRange(8).Data;
                DecodeDictionary.Add(code, b);
            }
        }

        public byte[] Decode() {
            var shortestKey = 8;

            foreach (var decodeDictionaryKey in DecodeDictionary.Keys)
                if (decodeDictionaryKey.Length < shortestKey)
                    shortestKey = decodeDictionaryKey.Length;

            var output = new List<byte>();
            var ub = default(UnevenByte);

            while (!bitIndexer.AtEnd()) {
                ub += ub == default(UnevenByte) ? bitIndexer.GetNextRange(shortestKey) : bitIndexer.GetNext();

                if (DecodeDictionary.ContainsKey(ub)) {
                    output.Add(DecodeDictionary[ub]);
                    ub = default(UnevenByte);
                }
            }

            return output.ToArray();
        }
    }
}