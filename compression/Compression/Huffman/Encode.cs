using System;
using System.Collections.Generic;
using Compression;

namespace Compression.Huffman {
    public class Encode {
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
               return ListOfNodes;
            }
            //Tree TreeOfList = new Tree();

            //Nodes HuffmanTree = TreeOfList.CreateTree(ListOfNodes);
        }
    }
}
