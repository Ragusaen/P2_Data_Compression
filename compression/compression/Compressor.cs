using System.Threading;

namespace compression {
    public abstract class Compressor {
        public DataFile Compress(DataFile dataFile) {
            DataFile outputFile = new DataFile();

            InternalCompress(dataFile, outputFile);
            
            return outputFile;
        }
        public abstract DataFile Decompress(DataFile dataFile);
        
        protected abstract void InternalCompress(DataFile input, DataFile output);
    }
}