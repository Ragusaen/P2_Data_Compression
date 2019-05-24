using System.Collections.Generic;

namespace Compression.Huffman {
    public class HuffmanCompressor : ICompressor {
        public DataFile Compress(DataFile file) {
            byte[] data = file.GetAllBytes();
            
            List<Node> listOfNodes = CreateLeafNodes(data);
            if (listOfNodes.Count <= 1) {
                throw new OnlyOneUniqueByteException();
            }
            
            var huffmanTree = new HuffmanTree(listOfNodes);
            var huffmanEncoder = new HuffmanEncoder();

            byte[] encodedBytes = huffmanEncoder.EncodeAllBytes(huffmanTree, data);

            return new DataFile(encodedBytes);
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
    }
}
