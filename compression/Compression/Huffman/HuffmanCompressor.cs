using System.Collections.Generic;

namespace Compression.Huffman {
    public class HuffmanCompressor : ICompressor {
        public DataFile Compress(DataFile file) {
            var data = file.GetAllBytes();

            var listOfNodes = CreateLeafNodes(data);
            if (listOfNodes.Count <= 1) throw new OnlyOneUniqueByteException();

            var huffmanTree = new HuffmanTree(listOfNodes);
            var huffmanEncoder = new HuffmanEncoder();

            var encodedBytes = huffmanEncoder.EncodeAllBytes(huffmanTree, data);

            return new DataFile(encodedBytes);
        }

        public DataFile Decompress(DataFile file) {
            var huffmanDecoder = new HuffmanDecoder(file.GetAllBytes());
            return new DataFile(huffmanDecoder.Decode());
        }


        private List<Node> CreateLeafNodes(byte[] data) {
            //public for unit tests
            var counts = new int[byte.MaxValue + 1];
            for (var i = 0; i < data.Length; ++i) counts[data[i]]++;

            var listOfNodes = new List<Node>();
            for (var b = 0; b < byte.MaxValue + 1; ++b)
                if (counts[b] > 0)
                    listOfNodes.Add(new LeafNode((byte) b, counts[b]));

            listOfNodes.Sort();
            return listOfNodes;
        }
    }
}