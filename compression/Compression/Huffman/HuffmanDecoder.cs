using System;
using System.Collections.Generic;
using compression;
using Compression.ByteStructures;
using Xceed.Wpf.Toolkit.Primitives;

namespace Compression.Huffman
{
    public class HuffmanDecoder {
        private Dictionary<UnevenByte, byte> DecodeDictionary = new Dictionary<UnevenByte, byte>();
        private BitIndexer bitIndexer;

        public HuffmanDecoder(byte[] input) {
            bitIndexer = new BitIndexer(input);
            
            // Remove filler ones
            while (bitIndexer.GetNext() == UnevenByte.One);
            bitIndexer.GoToPrevious(); // Go back, because we read the first 0
            
            AddDictionaryEntries(default(UnevenByte));
        }

        public void AddDictionaryEntries(UnevenByte code) {
            if (bitIndexer.GetNext() == UnevenByte.Zero) {
                AddDictionaryEntries(code + UnevenByte.Zero);
                AddDictionaryEntries(code + UnevenByte.One);
            }
            else {
                byte b = (byte) bitIndexer.GetNextRange(8).Data;
                Console.WriteLine($"Letter, Code: {(char)b}, {code}");
                DecodeDictionary.Add(code, b);
            }
        }

        public byte[] Decode() {
            List<byte> output = new List<byte>();
            UnevenByte ub = new UnevenByte();
            
            while (!bitIndexer.AtEnd()) {
                ub += bitIndexer.GetNext();
                if (DecodeDictionary.ContainsKey(ub)) {
                   output.Add(DecodeDictionary[ub]);
                   ub = default(UnevenByte);
                }
            }

            return output.ToArray();
        }
    }
}
