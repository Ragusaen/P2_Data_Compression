using System.Collections.Generic;
using Compression;
using Compression.ByteStructures;

namespace Compression.Huffman {
    public class Encode {
        public byte[] HuffmanEncode(byte[] data)
        {
            List<Nodes> ListOfNodes = HuffmanNodes(data);

            var TreeOfNodes = new Tree();
            Nodes NodeTree = TreeOfNodes.CreateTree(ListOfNodes);

            List<UnevenByte> NodeList = TreeOfNodes.TreeMap(NodeTree);
            Dictionary<byte, UnevenByte> NodeDict = TreeOfNodes.SetCode(NodeTree);

            return EncodeEveryByteFromData(NodeList, NodeDict, data);
        }


        public List<Nodes> HuffmanNodes(byte[] data) {
            List<Nodes> ListOfNodes = new List<Nodes>(); {
                for (int i = 0; i < data.Length; i++) {
                    if (ListOfNodes.Exists(x => x.symbol == data[i])) {
                        ListOfNodes[ListOfNodes.FindIndex(y => y.symbol == data[i])].IncreaseCount();
                    }
                    else {
                        ListOfNodes.Add(new Nodes(data[i]));
                    }
                ListOfNodes.Sort();
                }
            }
            return ListOfNodes;
        }

        public byte[] EncodeEveryByteFromData (List<UnevenByte> NodeList, Dictionary<byte, UnevenByte> NodeDict, byte[] data) {
            for (int i = 0; i < data.Length; i++) {
                NodeList.Add(NodeDict[data[i]]);
            }

            UnevenByte ub = CreateUnevenByteWithAll_1(NodeList);
            NodeList.Insert(0, ub);

            var unevenByteConverter = new UnevenByteConverter();

            return unevenByteConverter.UnevenBytesToBytes(NodeList);
        }

        private UnevenByte CreateUnevenByteWithAll_1(List<UnevenByte> NodeList) {
            UnevenByte ub = new UnevenByte();
            int stabilizer = 0;

            for (int i = 0; i < NodeList.Count; i++)
            {
                stabilizer += NodeList[i].Length;
            }

            stabilizer = (8 - (stabilizer % 8)) % 8;

            for (int j = 0; j < stabilizer; j++)
            {
                ub += new UnevenByte(0b1, 1);
            }

            return ub;
        }
    }
}
