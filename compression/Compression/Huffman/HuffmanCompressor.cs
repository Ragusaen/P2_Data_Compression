﻿using System;
using System.Collections.Generic;
using System.Linq;
using Compression;
using Compression.ByteStructures;

namespace Compression.Huffman {
    public class HuffmanCompressor : ICompressor {
        public DataFile Compress(DataFile file) {
            byte[] data = file.GetAllBytes();
            
            List<Node> listOfNodes = CreateLeafNodes(data);
            if (listOfNodes.Count <= 1) {
                throw new OnlyOneUniqueByteException();
            }
            
            var huffmanTree = new HuffmanTree(listOfNodes);

            byte[] encodedData = EncodeEveryByteFromData(huffmanTree, data);
            return new DataFile(encodedData);
        }

        public DataFile Decompress(DataFile file) {
            HuffmanDecoder huffmanDecoder = new HuffmanDecoder(file.GetAllBytes());
            return new DataFile(huffmanDecoder.Decode());
        }


        public List<Node> CreateLeafNodes(byte[] data) { //public for unit tests
            int[] counts = new int[byte.MaxValue + 1]; 
            for (int i = 0; i < data.Length; ++i) {
                counts[data[i]]++;
            }
            
            List<Node> listOfNodes = new List<Node>();
            for (int b = 0; b < byte.MaxValue + 1; ++b) {
                if (counts[b] > 0) {
                    listOfNodes.Add(new LeafNode((byte)b, counts[b]));
                }
            }
            
            listOfNodes.Sort();
            return listOfNodes;
        }

        public byte[] EncodeEveryByteFromData(HuffmanTree huffmanTree, byte[] data) {
            List<UnevenByte> unevenByteList = new List<UnevenByte>();
            unevenByteList.AddRange(huffmanTree.EncodedTreeList);
            
            for (int i = 0; i < data.Length; i++) {
                unevenByteList.Add(huffmanTree.CodeDictionary[data[i]]);
            }
            
            UnevenByte filler = CreateFillerUnevenByte(unevenByteList);
            unevenByteList.Insert(0, filler);

            var unevenByteConverter = new UnevenByteConverter();
            return unevenByteConverter.UnevenBytesToBytes(unevenByteList);
        }

        private UnevenByte CreateFillerUnevenByte(List<UnevenByte> NodeList) {
            int totalBitLength = 0;
            for (int i = 0; i < NodeList.Count; ++i)
                totalBitLength += NodeList[i].Length;

            int bitsInLastByte = totalBitLength % 8;
            
            uint fillOnes = 0;
            if ( bitsInLastByte > 0)
                fillOnes = (uint) 0b11111111 >> bitsInLastByte;
            
            return new UnevenByte(fillOnes, 8 - bitsInLastByte);
        }
    }
}
