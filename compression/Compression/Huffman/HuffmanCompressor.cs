using System.Collections.Generic;

namespace Compression.Huffman {
    public class HuffmanCompressor : ICompressor {
        public DataFile Compress(DataFile file) {
            byte[] data = file.GetAllBytes();
            
            List<Node> ListOfNodes = CreateLeafNodes(data);
            if (ListOfNodes.Count <= 1) {
                throw new OnlyOneUniqueByteException();
            }

            var huffmanTree = new HuffmanTree(ListOfNodes);
            var huffmanEncoder = new HuffmanEncoder(huffmanTree, data);

            return huffmanEncoder.dataFile;
        }

        public DataFile Decompress(DataFile file) {
            HuffmanDecoder huffmanDecoder = new HuffmanDecoder(file.GetAllBytes());
            return new DataFile(huffmanDecoder.Decode());
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
    }
}
