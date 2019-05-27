using System.Collections.Generic;

namespace Compression.LZ {
    /// <summary>
    ///     A simple extension of List<byte> which allows for adding and decoding EncodedĹZBytes easily.
    /// </summary>
    public class LZDecoderList : List<byte> {
        public void DecodeAndAddEncodedByte(EncodedLZByte eb) {
            if (eb is PointerByte pb) {
                // If it is a pointer byte, add the bytes, that it points to, to the output
                var bi = Count - pb.Pointer;
                for (var ai = 0; ai < pb.Length; ++ai) Add(this[bi + ai]);
            }
            else {
                // If it is a raw byte, add the raw bytes to the output
                Add(((RawByte) eb).Data);
            }
        }
    }
}