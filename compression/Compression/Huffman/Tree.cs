using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compression;
using Compression.Huffman;


namespace Compression.Huffman {
    public class Tree {
        public Nodes CreateTree(List<Nodes> ListOfNodes) {
            while(ListOfNodes.Count > 1 ) {
                Nodes Value1 = ListOfNodes[0];
                Nodes Value2 = ListOfNodes[1];
                ListOfNodes.RemoveAt(1);
                ListOfNodes.RemoveAt(0);
                ListOfNodes.Add(new Nodes(Value1, Value2));

                ListOfNodes.Sort();
            }
            Nodes HuffmanTree = ListOfNodes[0];

            // SetCodeToNode(HuffmanTree);

            return HuffmanTree;
        }

        public void SetCodeToNode(Nodes InheritCode) {          //Necessary ?
            if(InheritCode == null) { }
            else if(InheritCode.isLeaf == false) {
                InheritCode.leftNode.code = InheritCode.code + "0";
                InheritCode.rightNode.code = InheritCode.code + "1";

                SetCodeToNode(InheritCode.leftNode);
                SetCodeToNode(InheritCode.rightNode);
            }
            else if(InheritCode.isLeaf == true) {
                InheritCode.code = InheritCode.code + "0";
                InheritCode.code = InheritCode.code + "1";
            }
        }

        public string TreeMap(Nodes node)
        {
            if(node != null) {
                return EncodeLeftSide(node, "");
            }
            return "The file contains nothing"; // brug evt. exceptions
        }

        private string EncodeLeftSide(Nodes node, string output)
        {
            if(node.leftNode != null) {
                if (node.leftNode.isLeaf == true)
                {
                    //if(node.leftNode.leftNode == null) { // nødvendig (?)
                    output += "01" + ByteMethods.ByteToBinaryString(node.leftNode.symbol);
                    //}
                    node.leftNode = null;

                    output = EncodeRightSide(node, output);
                }
                else { //if(node.LeftNode.IsLeaf == false)
                    output = EncodeLeftSide(node.leftNode, output + "0");

                    if(node.rightNode != null) {
                        output = EncodeLeftSide(node.rightNode, output);
                    }
                }
            }
            else { //if(node.leftNode == null && node.rightNode == null) {
                output += "1" + ByteMethods.ByteToBinaryString(node.symbol);
            }
            return output;
        }

        private string EncodeRightSide(Nodes node, string output) {
            if (node.rightNode.isLeaf == true) {
                output += "1" + ByteMethods.ByteToBinaryString(node.rightNode.symbol);

                node.rightNode = null;
            }
            else { //if(node.RightNode.IsLeaf == false) {
                output = EncodeLeftSide(node.rightNode, output);
            }
            return output;
        }
    }
}
