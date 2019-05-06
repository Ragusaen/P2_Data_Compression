using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using compression.ByteStructures;
using Compression.ByteStructures;

namespace Compression.LZ {
    public class LZ77 : ICompressor {
        public DataFile Compress(DataFile input) {
            SlidingWindow slidingWindow = new SlidingWindow(input);
            List<UnevenByte> unevenBytes = new List<UnevenByte>();
            LZByteConverter lzByteConverter = new LZByteConverter();

            while (!slidingWindow.AtEnd()) {
                // Encode the next byte
                EncodedLZByte eb = slidingWindow.Slide();

                // Convert to UnevenByte
                UnevenByte ub = lzByteConverter.ToUnevenByte(eb);
                
                // Add ub to list
                unevenBytes.Add(ub);
            }
            
            // Convert unevenBytes to even bytes
            var unevenByteConverter = new UnevenByteConverter();
            byte[] bytes = unevenByteConverter.UnevenBytesToBytes(unevenBytes);
            
            return new DataFile(bytes);
        }

        public DataFile Decompress(DataFile dataFile) {
            byte[] inputBytes = dataFile.GetAllBytes();
            LZDecoderList outputList = new LZDecoderList();
            LZByteConverter lzByteConverter = new LZByteConverter();
            UnevenByteConverter ubConverter = new UnevenByteConverter();

            int bitIndex = 0;

            for (int i = 0; i < inputBytes.Length;) {
                // Calculate the UnevenByte length in bits
                int ubLength =
                    lzByteConverter.GetUnevenByteLength((byte) (inputBytes[i] << bitIndex));
                
                // If it cannot fit within the remaining bits, we must be done
                if (ubLength > (inputBytes.Length - i) * 8 - (bitIndex + 1))
                    break;

                UnevenByte ub = ubConverter.CreateUnevenByteFromBytes(
                    new ArrayIndexer<byte>(inputBytes, i, 0),
                    ubLength,
                    bitIndex);
                
                // Convert UnevenByte to EncodedLZByte
                EncodedLZByte eb = lzByteConverter.ToEncodedByte(ub);
                
                // Decode and add encoded byte to list
                outputList.DecodeAndAddEncodedByte(eb);
                
                // Update input array index
                i += (bitIndex + ubLength) / 8;
                
                // Update the bit index
                bitIndex = (bitIndex + (ubLength % 8)) % 8;
            }
            
            // Turn the list into an array and return it
            return new DataFile(outputList.ToArray());
        }
    }
}