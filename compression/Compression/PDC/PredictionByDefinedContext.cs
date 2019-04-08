using System;
using Compression;

namespace PDC {
    public class PredictionByDefinedContext : ICompressor {
        private ContextWindow _contextWindow;
        
        public PredictionByDefinedContext(uint history_length, uint context_size, uint prediction_size) {
            _contextWindow = new ContextWindow(
                history_length,
                (uint)(1 << (int)context_size),
                (uint) (1 << (int)prediction_size)
            );
        }
        
        //Default values
        public PredictionByDefinedContext() : this(1024, 2, 2) {}
        
        public DataFile Compress(DataFile to_compress) {
            _contextWindow.load_data(to_compress.GetBytes(0, to_compress.Length));
            
            
            return new DataFile();
        }

        public DataFile Decompress(DataFile to_decompress) {
            throw new NotImplementedException();
        }
    }
}