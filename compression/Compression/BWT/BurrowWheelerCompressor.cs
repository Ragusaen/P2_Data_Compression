using System;
using Compression.RLE;

namespace Compression.BWT {
    public class BurrowWheelerCompressor : ICompressor {
        public const int CHUNK_SIZE = 1024;
        
        public DataFile Compress(DataFile toCompress) {
            var bwt = new BurrowWheelerTransform();
            byte[] input = toCompress.GetAllBytes();
            byte[] transformed = new byte[input.Length];

            for (int i = 0; i < input.Length; i += CHUNK_SIZE) {
                int size = Math.Min(CHUNK_SIZE, input.Length - i);
                byte[] chunk = new byte[size];
                Array.Copy(input, i, chunk, 0, size);

                byte[] output = bwt.Transform(chunk);
                Array.Copy(output, 0, transformed, i, size);
            }
            
            ByteChangeEncoder bce = new ByteChangeEncoder();
            return new DataFile(bce.EncodeBytes(transformed).ToBytes());
        }

        public DataFile Decompress(DataFile toDecompress) {
            return new DataFile(new byte[] {98});
        }
    }
}