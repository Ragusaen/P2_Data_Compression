using System.Threading;

namespace compression {
    public abstract class Compressor {
        public DataFile Compress(DataFile dataFile) {
            DataFile outputFile = new DataFile();

            return InternalCompress(dataFile);
        }
        public abstract DataFile Decompress(DataFile dataFile);
        
        protected abstract DataFile InternalCompress(DataFile input);
    }
}