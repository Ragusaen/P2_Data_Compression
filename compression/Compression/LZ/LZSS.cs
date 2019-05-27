using Compression.ByteStructures;

namespace Compression.LZ {
    /// <summary>
    ///     The class that allows for LZSS compression.
    /// </summary>
    public class LZSS : ICompressor {
        public DataFile Compress(DataFile input) {
            var slidingWindow = new SlidingWindow(input.GetAllBytes());
            var lzByteConverter = new LZByteConverter();
            var bitString = new BitString();

            while (!slidingWindow.AtEnd()) {
                // Encode the next byte
                var eb = slidingWindow.Slide();

                // Convert to UnevenByte
                var ub = lzByteConverter.ToUnevenByte(eb);

                // Add to bitString
                bitString.Append(ub);
            }

            return new DataFile(bitString.ToArray());
        }

        public DataFile Decompress(DataFile dataFile) {
            var inputBytes = dataFile.GetAllBytes();
            var outputList = new LZDecoderList();
            var lzByteConverter = new LZByteConverter();

            var bitIndexer = new BitIndexer(inputBytes);

            while (!bitIndexer.AtEnd()) {
                // Calculate the UnevenByte length in bits
                var ubLength =
                    lzByteConverter.GetUnevenByteLength(bitIndexer.GetNext());
                bitIndexer.GoToPrevious(); // Unread the bit

                // If it cannot fit within the remaining bits, we must be done
                if (ubLength > bitIndexer.Remaining)
                    break;

                // Get the bits
                var ub = bitIndexer.GetNextRange(ubLength);

                // Convert UnevenByte to EncodedLZByte
                var eb = lzByteConverter.ToEncodedByte(ub);

                // Decode and add encoded byte to list
                outputList.DecodeAndAddEncodedByte(eb);
            }

            // Turn the list into an array and return it
            return new DataFile(outputList.ToArray());
        }
    }
}