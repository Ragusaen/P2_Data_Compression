using  System; 
using compression.ByteStructures;
namespace Compression {
    public class DataFileIterator {
        protected DataFile file;
        protected int currentIndex = 0;

        public DataFileIterator(DataFile file) {
            this.file = file; 
        }

        public Boolean AtEnd() {
            return currentIndex >= file.Length; 
        }
    }
}