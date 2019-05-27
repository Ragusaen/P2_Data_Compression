using System.Collections.Generic;
using Compression.ByteStructures;

namespace Compression.Huffman
{
    /// <summary>
    /// This class creates the encoding dictionary and encodes the dictionary.
    /// </summary>
    public class HuffmanTree {
        private Node RootNode;
        public List<UnevenByte> EncodedTreeList;
        public Dictionary<byte, UnevenByte> CodeDictionary;
        
        // Total bitlength of the input
        public int TotalLength = 0;
        public int TotalLeafs = 0;
        
        // Create the huffman tree from a list of leafs
        public HuffmanTree(List<Node> listOfNodes) {
            TotalLeafs = listOfNodes.Count;
            
            // Create the tree and initialize RootNode
            CreateTree(listOfNodes);
            
            // Create CodeDictionary
            CodeDictionary = new Dictionary<byte, UnevenByte>();
            SetCode(RootNode);
            
            // Create EncodedTreeList
            EncodedTreeList = new List<UnevenByte>();
            EncodeTree(RootNode);
        }

        /// <summary>
        /// This method creates a single BranchNode containing all LeafNodes. By creating BranchNodes 
        /// of two first nodes in the listofNodes and then remove the two nodes from the list and 
        /// resort it, until there's 1 node left in the list.
        /// </summary>
        private void CreateTree(List<Node> listOfNodes) {
            while (listOfNodes.Count > 1) {
                listOfNodes.Add(new BranchNode(listOfNodes[0], listOfNodes[1]));

                listOfNodes.RemoveAt(0);
                listOfNodes.RemoveAt(0);

                listOfNodes.Sort();
            }
            RootNode = listOfNodes[0];
        }

        /// <summary>
        /// This method gives each LeafNode and encoding code and count the total length of all the
        /// encoded bytes. The method is recursive and it calls itself until it meets a LeafNode.
        /// </summary>
        /// <param name="inheritCode"> The inheritCode Node holds part of the encoded code and gets inherit to the LeafNodes. </param>

        private void SetCode(Node inheritCode) { //public for unit tests
            if (inheritCode is BranchNode branchNode) {
                branchNode.LeftNode.Code = inheritCode.Code + UnevenByte.Zero;
                branchNode.RightNode.Code = inheritCode.Code + UnevenByte.One;
                
                SetCode(branchNode.LeftNode);
                SetCode(branchNode.RightNode);
            } else if (inheritCode is LeafNode) {
                CodeDictionary.Add(inheritCode.Symbol, inheritCode.Code);
                TotalLength += inheritCode.Count * inheritCode.Code.Length;
            }
        }

        /// <summary>
        /// This method encodes the dictionary for the decoder. The method is rucursive and it calls
        /// itself until it meets a LeafNode.
        /// </summary>
        /// <param name="node"> Indicates what node it is in the RootNode </param>
        private void EncodeTree(Node node) { 
            if (node is BranchNode branchNode) {
                EncodedTreeList.Add(UnevenByte.Zero);

                EncodeTree(branchNode.LeftNode);
                EncodeTree(branchNode.RightNode);
            }
            else if (node is LeafNode leafNode) {
                var symbolAsUB = new UnevenByte(leafNode.Symbol, 8);
                EncodedTreeList.Add(UnevenByte.One + symbolAsUB);
            }
        }
    }
}
