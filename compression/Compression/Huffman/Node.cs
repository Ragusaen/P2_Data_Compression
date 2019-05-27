using System;
using Compression.ByteStructures;

namespace Compression.Huffman
{
    /// <summary>
    /// This class contains a byte's value, count and encoding code. It implements the 
    /// IComparable interface for sorting List of its class.
    /// </summary>
    public abstract class Node : IComparable<Node>
    {
        public byte Symbol;
        public int Count;
        public UnevenByte Code = default(UnevenByte);

        /// <summary>
        /// Simply compare whether a Node has appeared more than another node. It sorts 
        /// from lowest to highest, if equal then by highestget byte value.
        /// </summary>
        /// <returns> 1 if Node's count is larger than the node it's compared to, -1 if 
        /// Node's count is smaller </returns>
        public int CompareTo(Node other) {
            int c = Count.CompareTo(other.Count);

            if (c != 0) {
                return c;
            }
            else {
                return Symbol.CompareTo(other.Symbol);
            }
        }
    }

    /// <summary>
    /// Simpel sub-class of Node used to contain byte's value and frequenzy from input.
    /// It's used by other classes to differentiate between BranchNode.
    /// </summary>
    public class LeafNode : Node {
        public LeafNode(byte character, int frequency) {
            Symbol = character;
            Count = frequency;
        }
    }

    /// <summary>
    /// Another sub-class of Node and is created from other Nodes. It's used by other 
    /// classes to inherit code to LeafNode.
    /// </summary>
    public class BranchNode : Node {
        public Node LeftNode { get; }
        public Node RightNode { get; }
        
        public BranchNode(Node left, Node right) { 
            Symbol = right.Symbol;
            Count = left.Count + right.Count;

            LeftNode = left;
            RightNode = right;
        }
    }
}
