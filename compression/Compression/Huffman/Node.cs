using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Compression.ByteStructures;

namespace Compression.Huffman
{
    public abstract class Node : IComparable<Node>, IEquatable<Node>
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

        public bool Equals(Node other) //kun for unit tests
        {
            return symbol == other.symbol && count == other.count;
        }
    }

    public class LeafNode : Node {
        public LeafNode(byte character) {
            symbol = character;
        }

        public LeafNode(byte character, int frequency) //Unit test
        {
            symbol = character;
            count = frequency;
        }
    }

    public class BranchNode : Node {
        public Node LeftNode;
        public Node RightNode;
        
        public BranchNode(Node left, Node right) { //kun for unit tests
            symbol = right.symbol;
            count = left.count + right.count;

            LeftNode = left;
            RightNode = right;
        }
    }
}
