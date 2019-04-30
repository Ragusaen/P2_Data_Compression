using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compression;
using Compression.Huffman;


namespace Compression.Huffman
{
    public class Decode
    {
        public Dictionary<string, byte> DecodedFile(byte[] ByteArray) {
            string decodedTreeMap = ByteMethods.ByteArrayToString(ByteArray);

            Dictionary<string, byte> reconstrucedTree = new Dictionary<string, byte>();

            int level = 0;
            string tempSymbol = "";     //kan ikke bruge tempNode, fordi tempNode.symbol er byte
            string tempCode = "";
            byte tempByte;
            bool rightNode = false;

            //Lav string der samler træ kortet
            //Lav funktion der tæller antal '0' og '1', og stopper når der er 1 mere '1' end '0'
            //string.Remove(string.Length - 1) fjerner den sidste ch i stringen

            for(int i = 0; i < decodedTreeMap.Length; i++) {
                if(decodedTreeMap[i] == '0') {
                    level++;
                    tempCode += "0";
                    rightNode = false;
                }
                else if(decodedTreeMap[i] == '1') {
                    i++;
                    for (int j = i + 8; i < j; i++) {
                        tempSymbol += decodedTreeMap[i];
                    }


                    tempByte = ByteMethods.BinaryStringToByte(tempSymbol);
                    tempSymbol = "";

                    reconstrucedTree.Add(tempCode, tempByte);

                    if(rightNode == false) {
                        if(tempCode.Length > 1) { 
                            tempCode = tempCode.Remove(tempCode.Length - 1);
                        }
                        else {
                            tempCode = "";
                        }
                    }
                    else {//if (rightNode == true);
                        if(level > 0) {
                            tempCode = tempCode.Remove(tempCode.Length - 1 - level);
                        }
                        level = tempCode.Length;
                    }
                    tempCode += "1";

                    rightNode = true;
                }
            }
            return reconstrucedTree;
        }
    }
}
