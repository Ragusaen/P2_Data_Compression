using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compression.Huffman {
    public class Nodes : IComparable<Nodes> {
        public byte symbol;
        public int count;
        public string code;

        public Nodes leftNode;
        public Nodes rightNode;
        public Nodes parentNode;

        public bool isLeaf { get; set; }

        public bool IsLeaf {
            set {
                if(leftNode == null && rightNode == null) {
                    isLeaf = true;
                }
                else {
                    isLeaf = false;
                }
            }
        }

        public Nodes() {
            leftNode = null;
            rightNode = null;
            parentNode = null;
        }

        public Nodes(byte character) {
            symbol = character;
            count = 1;
            code = "";

            leftNode = null;
            rightNode = null;
            parentNode = null;
        }

        public void IncreaseCount() {
            count++;
        }

        public Nodes(Nodes node1, Nodes node2) {
            count = node1.count + node2.count;
            code = "";
            parentNode = null;

            if (node1.count > node2.count) {
                symbol = node1.symbol;

                leftNode = node2;
                rightNode = node1;
            }
            else if(node1.count < node2.count) {
                symbol = node2.symbol;

                leftNode = node1;
                rightNode = node2;
            }
            else { //node1.Count = node2.Count
                symbol = node1.symbol;

                leftNode = node1;
                rightNode = node2;
            }
            leftNode.parentNode = this;
            rightNode.parentNode = this;
        }

        public int CompareTo(Nodes other)
        {
            int c = this.count.CompareTo(other.count);

            if (c != 0) { 
                return c;
            }
            else { 
                return this.symbol.CompareTo(other.symbol);
            }
        }
    }

    public class reconstructedNode {
        public byte symbol;
        public string code;

        public reconstructedNode() { }
    }
}
