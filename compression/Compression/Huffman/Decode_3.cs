using System.Collections.Generic;
using Compression.ByteStructures;

namespace Compression.Huffman
{
    public class Decode_3
    {
        public Dictionary<UnevenByte,byte> Huffman_decode(byte[] ByteArray) {
            Dictionary<UnevenByte, byte> DecodeDict = new Dictionary<UnevenByte, byte>();
            int i = 3;

            UnevenByte ub = CreateDecodeDictionary(ByteArray, DecodeDict, ref i);

            DecodeEncodedBytes(ByteArray, DecodeDict, ref i, ub);

            return DecodeDict;
        }

        public UnevenByte CreateDecodeDictionary(byte[] ByteArray, Dictionary<UnevenByte, byte> DecodeDict, ref int i) { //i starter med at være 3
            int deviation = 0;  //antal 0'er i forhold til 1-taller
            int level = 0;      //Hvor mange bits symbolet er indkodet med
            bool rightNode = false;

            UnevenByte DecodeCode = new UnevenByte();

            UnevenByte ub = RemoveFiller_1s(ByteArray[0]) 
                + new UnevenByte(ByteArray[1], 8) 
                + new UnevenByte(ByteArray[2], 8);

            while(deviation != -1) { 
                if(ub.Length < 10) {
                    ub += new UnevenByte(ByteArray[i], 8);
                    i++;
                }
                else {//if(ub.Length >= 10) {
                    if(ub.GetBits(1) == 0) {
                        ub -= 1;
                        level++;
                        deviation++;
                        rightNode = false;

                        DecodeCode += new UnevenByte(0b0, 1);
                    }
                    else if(ub.GetBits(1) == 1) {
                        ub -= 1;
                        deviation--;
                        byte b = ub.GetBits(8);
                        ub -= 8;

                        DecodeDict.Add(DecodeCode, b);

                        if (rightNode == false) { 
                            if (DecodeCode.Length > 0) {
                                DecodeCode = new UnevenByte(DecodeCode.GetBits(DecodeCode.Length - 1), DecodeCode.Length - 1);
                            }
                        }
                        else if(rightNode == true) {
                            if(level > 0) {
                                DecodeCode = new UnevenByte(DecodeCode.GetBits(DecodeCode.Length - level), DecodeCode.Length - level);
                            }
                            level = DecodeCode.Length;
                        }
                        DecodeCode += new UnevenByte(0b1, 1);

                        rightNode = true;
                    }
                }
            }
            return ub;
        }

        public byte[] DecodeEncodedBytes(byte[] ByteArray, Dictionary<UnevenByte, byte> DecodeDict, ref int i, UnevenByte ub) {

            return new byte[1];
        }

        public UnevenByte RemoveFiller_1s(byte b) {
            UnevenByte ub = new UnevenByte(b, 8);

            while(ub.GetBits(1) != 0) {
                ub -= 1;
            }

            return ub;
        }

        public UnevenByte ConvertCodeToUnevenByte(string s) {
            UnevenByte ub = new UnevenByte();
            byte[] bArr;

            bArr = ByteMethods.BinaryStringToByteArray(s);

            var unevenByteConverter = new UnevenByteConverter();

            ub = unevenByteConverter
                .CreateUnevenByteFromBytes(
                new ArrayIndexer<byte>(bArr, 0, s.Length / 8 + 1), s.Length, 8 - (s.Length % 8));

            return ub;
        }


    }
}
