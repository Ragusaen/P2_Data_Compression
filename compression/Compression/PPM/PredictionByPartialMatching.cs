using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace Compression.PPM{
    public class PredictionByPartialMatching : ICompressor{
        private readonly int _maxOrder;
        private readonly int _defaultEscaping;
        
        public PredictionByPartialMatching(int maxOrder = 5, int defaultEscaping = 0) {
            _maxOrder = maxOrder;
            _defaultEscaping = defaultEscaping;
        }
        
        public DataFile Compress(DataFile toCompress) {
            PPMTables ppmTables = new PPMTables(_maxOrder, _defaultEscaping);
            ppmTables.FillTables(toCompress);
            return toCompress;
        }

        public DataFile Decompress(DataFile toDecompress) {
            throw new System.NotImplementedException();
        }
    }
}
    