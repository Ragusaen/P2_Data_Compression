using System.Collections.Generic;

namespace Compression.LZ {
    public class LZDecoderList : List<byte> {
        public void DecodeAndAddEncodedByte( EncodedLZByte eb ) {
            if (eb is PointerByte) {
                // If it is pointer byte, add the bytes, that it points to, to the output
                PointerByte pb = (PointerByte) eb;
                int bi = Count - pb.Pointer;
                for (int ai = 0; ai < pb.Length; ++ai) {
                    Add(this[bi + ai]);
                }
            } else {
                // If it is a raw byte, add the raw bytes to the output
                Add(((RawByte) eb).Data);
            }
        }
    }
}