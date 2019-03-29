namespace Compression {
    public interface ICompressor {
        DataFile Compress(DataFile to_compress);
        DataFile Decompress(DataFile to_decompress);
    }
}