namespace Compression {
    public interface ICompressor {
        DataFile Compress(DataFile toCompress);
        DataFile Decompress(DataFile toDecompress);
    }
}