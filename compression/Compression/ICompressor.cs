namespace Compression {
    /// <summary>
    /// Interface that all the compression algorithm implements to allow for generalized use of them.
    /// </summary>
    public interface ICompressor {
        DataFile Compress(DataFile toCompress);
        DataFile Decompress(DataFile toDecompress);
    }
}