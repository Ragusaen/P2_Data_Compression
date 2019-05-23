using System.Collections.Generic;
using Compression.ByteStructures;

namespace Compression.LZ {
    public class LZ77 : ICompressor {
        public DataFile Compress(DataFile input) {
            SlidingWindow slidingWindow = new SlidingWindow(input);
            List<UnevenByte> unevenBytes = new List<UnevenByte>();
            LZByteConverter lzByteConverter = new LZByteConverter();
            BitString bitString = new BitString();

            while (!slidingWindow.AtEnd()) {
                // Encode the next byte
                EncodedLZByte eb = slidingWindow.Slide();

                // Convert to UnevenByte
                UnevenByte ub = lzByteConverter.ToUnevenByte(eb);
                
                // Add to bitString
                bitString.Append(ub);
            }
            
            return new DataFile(bitString.ToArray());
        }

        public DataFile Decompress(DataFile dataFile) {
            byte[] inputBytes = dataFile.GetAllBytes();
            LZDecoderList outputList = new LZDecoderList();
            LZByteConverter lzByteConverter = new LZByteConverter();

            BitIndexer bitIndexer = new BitIndexer(inputBytes);

            while (!bitIndexer.AtEnd()) {
                // Calculate the UnevenByte length in bits
                int ubLength =
                    lzByteConverter.GetUnevenByteLength(bitIndexer.GetNext());
                bitIndexer.GoToPrevious(); // Unread the bit
                
                // If it cannot fit within the remaining bits, we must be done
                if (ubLength > bitIndexer.Remaining)
                    break;
                
                // Get the bits
                UnevenByte ub = bitIndexer.GetNextRange(ubLength);
                
                // Convert UnevenByte to EncodedLZByte
                EncodedLZByte eb = lzByteConverter.ToEncodedByte(ub);
                
                // Decode and add encoded byte to list
                outputList.DecodeAndAddEncodedByte(eb);
            }
            
            // Turn the list into an array and return it
            return new DataFile(outputList.ToArray());
        }
    }
}