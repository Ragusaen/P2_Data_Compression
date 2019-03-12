using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Resolvers;

namespace compression.LZ77 {
    public class LZ77 : Compressor {
        protected override DataFile InternalCompress(DataFile input) {
            SlidingWindow slidingWindow = new SlidingWindow(input);
            List<EncodedByte> encodedByteArray = new List<EncodedByte>(0);

            while(!slidingWindow.AtEnd()) {
                encodedByteArray.Add(slidingWindow.Slide());
            }

            encodedByteArray.ForEach(Console.WriteLine);
            
            UnevenBits[] unevenBitsArray = new UnevenBits[encodedByteArray.Count];
            for (int i = 0; i < encodedByteArray.Count; i++)
                unevenBitsArray[i] = encodedByteArray[i].ToUnevenBits();

            foreach (UnevenBits ub in unevenBitsArray) {
                Console.Write(ub.Length + ", ");
            }

            byte[] encodedBytes = ByteEncoder.EncodeBytes(unevenBitsArray);
            
            DataFile output = new DataFile();
            output.LoadBytes(encodedBytes);
            return output;
        }

        public override DataFile Decompress(DataFile dataFile) {
            throw new System.NotImplementedException();
        }
    }
}