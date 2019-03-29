using System.Collections.Generic;
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
                unevenByteArray[i] = encodedByteArray[i].ToUnevenBits();

            byte[] bytes = UnevenByte.UnEvenBytesToBytes(unevenByteArray);
            
            DataFile output = new DataFile(bytes);
            
            return output;
        }

        public DataFile Decompress(DataFile dataFile) {
            throw new System.NotImplementedException();
        }
    }
}