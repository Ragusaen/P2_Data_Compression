using System.Collections.Generic;

namespace Compression.PPM{
    public class PredictionByPartialMatching : ICompressor{
        private int Order;
        private List<ContextTable> _contextTableList;

        public PredictionByPartialMatching(int order) {
            Order = order;
        }
        
        public DataFile Compress(DataFile to_compress) {
            throw new System.NotImplementedException();
        }

        public DataFile Decompress(DataFile to_decompress) {
            throw new System.NotImplementedException();
        }

        private void MakeTable(DataFile file) {
            for (int i = 0; i < Order; i++) {
                _contextTableList[i] = new ContextTable();
            }
        }
    }
}