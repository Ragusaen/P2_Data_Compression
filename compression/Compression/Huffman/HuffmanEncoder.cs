using System.Collections.Generic;
using Compression.ByteStructures;

namespace Compression.Huffman
{
    public class HuffmanEncoder {
        public DataFile dataFile;

        public HuffmanEncoder(HuffmanTree tree, byte[] data) {
            dataFile = EncodeAllBytes(tree, data);
        }

        private DataFile EncodeAllBytes (HuffmanTree huffmanTree, byte[] data) {
            List<UnevenByte> unevenByteList = new List<UnevenByte>();
            unevenByteList.AddRange(huffmanTree.EncodedTreeList);
            
            for (int i = 0; i < data.Length; i++) {
                unevenByteList.Add(huffmanTree.CodeDictionary[data[i]]);
            }
            
            UnevenByte filler = CreateFillerUnevenByte(unevenByteList);
            unevenByteList.Insert(0, filler);

            var unevenByteConverter = new UnevenByteConverter();
            return new DataFile(unevenByteConverter.UnevenBytesToBytes(unevenByteList));
        }

        private UnevenByte CreateFillerUnevenByte(List<UnevenByte> NodeList) {
            var ubConverter = new UnevenByteConverter();

            int totalBitLength = 0;
            for (int i = 0; i < NodeList.Count; ++i) { 
                totalBitLength += NodeList[i].Length;
            }

            int bitsInLastByte = totalBitLength % 8;
            
            uint fillOnes = 0;
            if ( bitsInLastByte > 0) { 
                fillOnes = (uint)0b11111111 >> bitsInLastByte;
            }

            return new UnevenByte(fillOnes, 8 - bitsInLastByte);
        }
    }
}
