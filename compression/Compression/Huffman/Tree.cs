using System.Collections.Generic;
using System.Linq;
using Compression;
using Compression.Huffman;
using Compression.ByteStructures;

namespace Compression.Huffman
{
    public class Tree {
        public Nodes CreateTree(List<Nodes> ListOfNodes) {
            while (ListOfNodes.Count > 1) {
                ListOfNodes.Add(new Nodes(ListOfNodes[0], ListOfNodes[1]));

                ListOfNodes.RemoveAt(0);
                ListOfNodes.RemoveAt(0);

                ListOfNodes.Sort();
            }
            return ListOfNodes[0];
        }

        public Dictionary<byte, UnevenByte> SetCode(Nodes InheritCode) {
            var NodeDict = new Dictionary<byte, UnevenByte>();

            if (InheritCode.isLeaf == false) {
                InheritCode.leftNode.code = InheritCode.code + '0';
                InheritCode.rightNode.code = InheritCode.code + '1';

                NodeDict = SetCode(InheritCode.leftNode)
                    .Concat(SetCode(InheritCode.rightNode))
                    .GroupBy(v => v.Key)
                    .ToDictionary(k => k.Key, v => v.First().Value);
            }
            else if (InheritCode.isLeaf == true) {
                var unevenByteConverter = new UnevenByteConverter();
                //UnevenByte codeUnevenByte = new UnevenByte(ByteMethods.BinaryStringToByte(InheritCode.code), InheritCode.code.Length);
                UnevenByte codeUnevenByte = unevenByteConverter
                    .CreateUnevenByteFromBytes(new ArrayIndexer<byte>(
                        ByteMethods.BinaryStringToByteArray(InheritCode.code),
                        0,
                        (InheritCode.code.Length / 8) + 1),
                    InheritCode.code.Length,
                    8 - (InheritCode.code.Length % 8));

                NodeDict.Add(InheritCode.symbol, codeUnevenByte);
            }
            return NodeDict;
        }

        public List<UnevenByte> TreeMap(Nodes node) {
            List<UnevenByte> unevenByteList = new List<UnevenByte>();

            if (node != null) {
                EncodeLeftSide(node, "", unevenByteList);

                return unevenByteList;
            }
            return unevenByteList; // brug evt. exceptions
        }

        private void EncodeLeftSide(Nodes node, string output, List<UnevenByte> unevenByteList) {
            if (node.leftNode != null) {
                if (node.leftNode.isLeaf == true) {
                    AddToUnevenbyteList(node.leftNode.symbol, output + "01", unevenByteList);

                    output = "";
                    node.leftNode = null;
                    node.isLeaf = true; //mangler at blive testet

                    EncodeRightSide(node, output, unevenByteList);
                }
                else { //if(node.LeftNode.IsLeaf == false) {
                    EncodeLeftSide(node.leftNode, output + "0", unevenByteList);

                    output = "";
                    if (node.rightNode != null) {
                        EncodeLeftSide(node.rightNode, output, unevenByteList);
                    }
                }
            }
            else { //if(node.leftNode == null && node.rightNode == null) {
                AddToUnevenbyteList(node.symbol, output + "1", unevenByteList);

                output = "";
            }
        }

        private void EncodeRightSide(Nodes node, string output, List<UnevenByte> unevenByteList) {
            if (node.rightNode.isLeaf == true) {
                AddToUnevenbyteList(node.rightNode.symbol, output + "1", unevenByteList);

                //output = "";
                node.rightNode = null;
                node.isLeaf = true; // mangler at blive testet
            }
            else { //if(node.RightNode.IsLeaf == false) {
                EncodeLeftSide(node.rightNode, output, unevenByteList);
            }
        }

        private void AddToUnevenbyteList(byte symbol, string output, List<UnevenByte> unevenByteList) {
            UnevenByteConverter unevenByteConverter = new UnevenByteConverter();

            unevenByteList.Add(
                unevenByteConverter.CreateUnevenByteFromBytes(
                    new ArrayIndexer<byte>(
                        new byte[] { ByteMethods.BinaryStringToByte(output), symbol },
                        0,
                        (output.Length / 8) + 2),
                    8 + output.Length,
                    8 - (output.Length % 8))
                );
        }
    }
}
