namespace Compression {
    public abstract class Compressor {

        public abstract File Compress(File file);
        public abstract File Decompress(File file);
    }
}