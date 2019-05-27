using System;
using Compression.ByteStructures;

namespace Compression.Huffman {
    /// <summary>
    ///     This class contains a byte's value, count and encoding code. It implements the
    ///     IComparable interface for sorting List of its class.
    /// </summary>
    public abstract class Node : IComparable<Node> {
        public UnevenByte code = default(UnevenByte);
        public int count;
        public byte symbol;

        /// <summary>
        ///     Simply compare whether a Node has appeared more than another node. It sorts
        ///     from lowest to highest, if equal then by highest byte value.
        /// </summary>
        /// <returns>
        ///     1 if Node's count is larger than the node it's compared to, -1 if
        ///     Node's count is smaller
        /// </returns>
        public int CompareTo(Node other) {
            var c = count.CompareTo(other.count);

            if (c != 0)
                return c;
            return symbol.CompareTo(other.symbol);
        }
    }

    /// <summary>
    ///     Simpel sub-class of Node used to contain byte's value and frequenzy from input.
    ///     It's used by other classes to differentiate between BranchNode.
    /// </summary>
    public class LeafNode : Node {
        public LeafNode(byte character, int frequency) {
            symbol = character;
            count = frequency;
        }
    }

    /// <summary>
    ///     Another sub-class of Node and is created from other Nodes. It's used by other
    ///     classes to inherit code to LeafNode.
    /// </summary>
    public class BranchNode : Node {
        public Node LeftNode;
        public Node RightNode;

        public BranchNode(Node left, Node right) {
            symbol = right.symbol;
            count = left.count + right.count;

            LeftNode = left;
            RightNode = right;
        }
    }
}