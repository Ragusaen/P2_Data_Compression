using System;
using System.Collections.Generic;
using System.Linq;
using Compression;
using Compression.ByteStructures;

namespace Compression.Huffman {
    public class HuffmanEncoder : ICompressor {
        public DataFile Compress(DataFile file) {
            byte[] data = file.GetAllBytes();
            
            List<Node> ListOfNodes = CreateLeafNodes(data);
            if (ListOfNodes.Count <= 1) {
                throw new OnlyOneUniqueByteException();
            }

            var huffmanTree = new HuffmanTree(ListOfNodes);

            byte[] encodedData = EncodeEveryByteFromData(huffmanTree, data);
            return new DataFile(encodedData);
        }

        public DataFile Decompress(DataFile file) {
            throw new NotImplementedException();
        }


        public List<Node> CreateLeafNodes(byte[] data) { //public for unit tests
            List<Node> ListOfNodes = new List<Node>();
            for (int i = 0; i < data.Length; i++) {
                // If the symbol has already been added
                if (ListOfNodes.Exists(x => x.symbol == data[i])) {
                    int index = ListOfNodes.FindIndex(y => y.symbol == data[i]);
                    ListOfNodes[index].count++;
                } else {
                    ListOfNodes.Add(new LeafNode(data[i]));
                }
            }
            ListOfNodes.Sort();
            return ListOfNodes;
        }

        public byte[] EncodeEveryByteFromData (HuffmanTree huffmanTree, byte[] data) {
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
            var ubConverter = new UnevenByteConverter();

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
