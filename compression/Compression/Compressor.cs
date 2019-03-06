namespace Compression {
    public abstract class Compressor {
        public abstract DataFile Compress(DataFile dataFile);
        public abstract DataFile Decompress(DataFile dataFile);
    }
}