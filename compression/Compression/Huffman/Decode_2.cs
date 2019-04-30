using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace Compression.Huffman
{
    public class Decode_2
    {
        public BitArray DecodeFile(byte[] ByteArray) { 
        //public Dictionary<byte, BitArray> DecodeFile(byte[] ByteArray) {
        //public int DecodeFile(byte[] ByteArray) { 
            BitArray encoded_file = new BitArray(ByteArray);

            Dictionary<byte, BitArray> reconstructedMap = new Dictionary<byte, BitArray>();
            //Dictionary<string, BitArray> reconstructedMap = new Dictionary<string, BitArray>();

            int deviation = 0;      //Antal 0'er i forhold til 1-taller
            int level = 0;          //Længden på koden

            bool rightNode = false; //

            string tempSymbol = "";
            byte tempByte = new byte();
            string tempCode = "";

            for(int i = 0; -1 == deviation; i++) {
                if(encoded_file[i] == true) { //encoded_file[i] == 0
                    tempCode += '0';
                    level++;
                    rightNode = false;
                }
                else if(encoded_file[i] == false) { //encoded_file[i] == 1
                    for (int j = i + 8; j >= i + 1; j--) {
                        if (encoded_file[i] == false) {
                            tempSymbol += '1';
                        }
                        else if(encoded_file[i] == true) {
                            tempSymbol += '0';
                        }
                        tempByte = (byte)((tempByte << 1) | (encoded_file[j] ? 1 : 0));
                    }
                    i += 8;

                    reconstructedMap.Add(tempByte, ConvertBinaryStringToBitArray(tempCode));
                    
                    if (rightNode == false) {
                        if (tempCode.Length > 1) {
                            tempCode = tempCode.Remove(tempCode.Length - 1);
                        }
                        else {
                            tempCode = "";
                        }
                    }
                    else {//if(rightNode == true) {
                        if (level > 0) {
                            tempCode = tempCode.Remove(tempCode.Length - level);
                        }
                        level = tempCode.Length;
                    }
                    
                    tempCode += "1";

                    rightNode = true;
                }
            }
            //return level;
            //return reconstructedMap;
            return ConvertBinaryStringToBitArray(tempCode);
        }   

        public BitArray ConvertBinaryStringToBitArray(string binaryString) {
            BitArray bitArray = new BitArray(binaryString.Length);

            for(int i = 0; i < binaryString.Length; i++) {
                if(binaryString[i] == '0') {
                    bitArray[i] = true;
                }    
                else {//if(binaryString[i] == '1') {
                    bitArray[i] = false;
                }
            }
            
            return bitArray;
        }
    }
}