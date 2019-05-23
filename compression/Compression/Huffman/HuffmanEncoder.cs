using System;
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
            BitString bitString = new BitString();
            
            // Calculate filler bits
            int bitsInLastByte = (huffmanTree.TotalLeafs * 10 - 1 + huffmanTree.TotalLength) % 8;
            UnevenByte fillerBits = new UnevenByte((uint)0b11111111 >> bitsInLastByte, 8 - bitsInLastByte);

            bitString.Append(fillerBits);
            
            // Append the encoded tree
            for (int i = 0; i < huffmanTree.EncodedTreeList.Count; ++i) {
                bitString.Append(huffmanTree.EncodedTreeList[i]);
            }
            
            // Encode all the bytes
            for (int i = 0; i < data.Length; i++) {
                bitString.Append(huffmanTree.CodeDictionary[data[i]]);
            }

            return new DataFile(bitString.ToArray());
        }

        private UnevenByte CreateFillerUnevenByte(List<UnevenByte> NodeList) {
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
