using System.Linq;
using System.Xml.Resolvers;

namespace compression.LZ77 {
    public class LZ77 : Compressor {
        protected override void InternalCompress(DataFile input, DataFile output) {
            SlidingWindow slidingWindow = new SlidingWindow(input);
            EncodedByte[] encodedByteArray = new EncodedByte[0];

            while(!slidingWindow.AtEnd()) {
                encodedByteArray.Append(slidingWindow.Slide());
            }
            
            UnevenBits[] unevenBitsArray = new UnevenBits[encodedByteArray.Length];
            for (int i = 0; i < encodedByteArray.Length; i++)
                unevenBitsArray[i] = encodedByteArray[i].ToUnevenBits();

            byte[] encodedBytes = ByteEncoder.EncodeBytes(unevenBitsArray);
            
            output.LoadBytes(encodedBytes);
        }

        public override DataFile Decompress(DataFile dataFile) {
            throw new System.NotImplementedException();
        }
    }
}