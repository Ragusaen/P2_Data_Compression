using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Compression.ByteStructures;

namespace Compression.LZ {
    public class LZ77 : ICompressor {
        public DataFile Compress(DataFile input) {
            SlidingWindow slidingWindow = new SlidingWindow(input);
            List<EncodedByte> encodedByteArray = new List<EncodedByte>(0);
            
            while(!slidingWindow.AtEnd()) {
                encodedByteArray.Add(slidingWindow.Slide());
            }
            
            UnevenByte[] unevenByteArray = new UnevenByte[encodedByteArray.Count];
            for (int i = 0; i < encodedByteArray.Count; i++)
                unevenByteArray[i] = encodedByteArray[i].ToUnevenByte();

            byte[] bytes = UnevenByte.UnEvenBytesToBytes(unevenByteArray);
            
            return new DataFile(bytes);
        }

        public DataFile Decompress(DataFile dataFile) {
            byte[] inputBytes = dataFile.GetAllBytes();
            List<byte> outputBytes = new List<byte>();
            
            PointerByte StupidMethodCallerObject = new PointerByte(0,0);

            uint bitIndex = 0;

            for (int i = 0; i < inputBytes.Length;) {
                // Calculate the UnevenByte length in bits
                uint ubLength =
                    StupidMethodCallerObject.GetUnevenByteLength((byte) (inputBytes[i] << (int)bitIndex));
                
                // If it cannot fit within the remaining bits, we must be done
                if (ubLength > (inputBytes.Length - i) * 8 - (bitIndex + 1))
                    break;
                
                // Calculate the amount of relevant bytes
                int c = (int) (bitIndex + ubLength) / 8 + ((bitIndex + ubLength) % 8 == 0 ? 0 : 1);
                
                // Add relevant bytes to array
                byte[] relevantBytes = new byte[c];
                Array.Copy(inputBytes, i, relevantBytes, 0, c);
                
                // Create a new UnevenByte from the relevant bytes
                UnevenByte ub = new UnevenByte(relevantBytes, bitIndex, ubLength);
                
                // Convert UnevenByte to EncodedLZByte
                EncodedLZByte eb = (EncodedLZByte) StupidMethodCallerObject.UnevenByteToEncodedByte(ub);
                
                // Determine which type it is
                if (eb is PointerByte) {
                    // If it is pointer by, add the bytes, that it points to, to the output
                    PointerByte pb = (PointerByte) eb;
                    int bi = (int) (outputBytes.Count - pb.Pointer);
                    Console.WriteLine("Pointer(" + pb.Pointer + ", " + pb.Length + ")");
                    for (int ai = 0; ai < pb.Length; ++ai) {
                        outputBytes.Add(outputBytes[bi + ai]);
                    }
                } else {
                    // If it is a raw byte, add the raw bytes to the output
                    outputBytes.Add(((RawByte) eb).Data);
                    Console.WriteLine( "BI: " + bitIndex +  " " + (char)outputBytes.Last());
                }
                
                // Update input array index
                i += (int)(bitIndex + ubLength) / 8;
                
                // Update the bit index
                bitIndex = (bitIndex + (ubLength % 8)) % 8;
            }
            
            outputBytes.ForEach((b) => Console.Write((char)b));
            Console.WriteLine();
            
            return new DataFile(outputBytes.ToArray());
        }
    }
}