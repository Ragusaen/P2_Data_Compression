using System.Collections.Generic;

namespace Compression.Huffman {
    public class HuffmanCompressor : ICompressor{
        private int _fileLength;
        private object _coder;
        
        public DataFile Compress(DataFile file) {
            var data = file.GetAllBytes();
            _fileLength = file.Length;

            var listOfNodes = CreateLeafNodes(data);
            if (listOfNodes.Count <= 1) throw new OnlyOneUniqueByteException();

            var huffmanTree = new HuffmanTree(listOfNodes);
            var huffmanEncoder = new HuffmanEncoder();
            
            _coder = huffmanEncoder;
            var encodedBytes = huffmanEncoder.EncodeAllBytes(huffmanTree, data);
            _coder = null;
            
            return new DataFile(encodedBytes);
        }

        public DataFile Decompress(DataFile file) {
            DataFile res;
            _fileLength = file.Length;
            var huffmanDecoder = new HuffmanDecoder(file.GetAllBytes());
            
            _coder = huffmanDecoder;
            res = new DataFile(huffmanDecoder.Decode());
            _coder = null;

            return res;
        }

        public double GetStatus() {
            if(_coder is HuffmanEncoder encoder)
                return (double) encoder.Progress / _fileLength;
            if (_coder is HuffmanDecoder decoder)
                return (_fileLength - (double)decoder.RemainingBits / 8) / _fileLength;
            return 0.0;
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