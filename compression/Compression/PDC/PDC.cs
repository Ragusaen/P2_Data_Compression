using System;

namespace Compression.NPDC {
    public class PDC : ICompressor {
        private uint _history_length;
        private uint _context_length;
        private uint _prediction_length;
        
        public PDC(uint history_length, uint context_size, uint prediction_size) {
            _history_length = history_length;
            _context_length = (uint)(1 << (int)context_size);
            _prediction_length = (uint) (1 << (int)prediction_size);
        }
        
        //Default values
        public PDC() : this(1024, 2, 2) {}
        
        public DataFile Compress(DataFile to_compress) {
            
            
            
            return new DataFile();
        }

        public DataFile Decompress(DataFile to_decompress) {
            throw new NotImplementedException();
        }
    }
}