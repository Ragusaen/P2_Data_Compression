using System.Collections.Generic;

namespace Compression.PPM{
    public class PredictionByPartialMatching : ICompressor{
        private int order;
        private List<ContextTable> _orderList;

        public PredictionByPartialMatching(int order) {
            this.order = order;
        }
        
        public DataFile Compress(DataFile to_compress) {
            throw new System.NotImplementedException();
        }

        public DataFile Decompress(DataFile to_decompress) {
            throw new System.NotImplementedException();
        }

        private void MakeTables(DataFile file) {
            for (int i = 0; i < order; i++) {
                _orderList[i] = new ContextTable();
            }
        }
    }
}