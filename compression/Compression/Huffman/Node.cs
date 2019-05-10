using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Compression.ByteStructures;

namespace Compression.Huffman
{
    public abstract class Node : IComparable<Node>
    {
        public byte symbol;
        public int count = 1;
        public UnevenByte code = default(UnevenByte);

        public int CompareTo(Node other) {
            int c = count.CompareTo(other.count);

            if (c != 0) {
                return c;
            }
            else {
                return symbol.CompareTo(other.symbol);
            }
        }
    }

    public class LeafNode : Node {
        public LeafNode(byte character) {
            symbol = character;
        }
        
        public void IncreaseCount() {
            count++;
        }
    }

    public class BranchNode : Node {
        public Node LeftNode;
        public Node RightNode;
        
        public BranchNode(Node left, Node right) {
            count = left.count + right.count;

            LeftNode = left;
            RightNode = right;
            symbol = right.symbol;
        }
    }

    public class ReconstructedNode {
        public byte symbol;
        public string code;

        public ReconstructedNode() { }
    }
}
