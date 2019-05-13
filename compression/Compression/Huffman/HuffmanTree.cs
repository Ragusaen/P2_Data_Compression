using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Compression;
using Compression.Huffman;
using Compression.ByteStructures;

namespace Compression.Huffman
{
    public class HuffmanTree {
        public Node RootNode;
        public List<UnevenByte> EncodedTreeList;
        public Dictionary<byte, UnevenByte> CodeDictionary;
        
        // Create the huffman tree from a list of leafs
        public HuffmanTree(List<Node> listOfNodes) {
            // Create the tree and initialize RootNode
            CreateTree(listOfNodes);
            
            // Create CodeDictionary
            CodeDictionary = new Dictionary<byte, UnevenByte>();
            SetCode(RootNode);
            
            // Create EncodedTreeList
            EncodedTreeList = new List<UnevenByte>();
            EncodeTree(RootNode);
        }

        public void CreateTree(List<Node> listOfNodes) { //public for unit tests
            while (listOfNodes.Count > 1) {
                listOfNodes.Add(new BranchNode(listOfNodes[0], listOfNodes[1]));

                listOfNodes.RemoveAt(0);
                listOfNodes.RemoveAt(0);

                listOfNodes.Sort();
            }
            RootNode = listOfNodes[0];
        }
        
        public void SetCode(Node inheritCode) { //public for unit tests
            if (inheritCode is BranchNode branchNode) {
                branchNode.LeftNode.code = inheritCode.code + UnevenByte.Zero;
                branchNode.RightNode.code = inheritCode.code + UnevenByte.One;
                
                SetCode(branchNode.LeftNode);
                SetCode(branchNode.RightNode);
            } else if (inheritCode is LeafNode) {
                CodeDictionary.Add(inheritCode.symbol, inheritCode.code);
            }
        }

        public void EncodeTree(Node node) { //public for unit tests
            if (node is LeafNode leafNode) {
                var symbolAsUB = new UnevenByte(leafNode.symbol, 8);
                EncodedTreeList.Add(UnevenByte.One + symbolAsUB);
            } else if (node is BranchNode branchNode) {
                EncodedTreeList.Add(UnevenByte.Zero);
                EncodeTree(branchNode.LeftNode);
                EncodeTree(branchNode.RightNode);
            }
        }
    }
}
