using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace Compression.PPM{
    public class PredictionByPartialMatching : ICompressor{
        private readonly int _maxOrder;
        
        public PredictionByPartialMatching(int maxOrder = 5) {
            _maxOrder = maxOrder;
        }
        
        public DataFile Compress(DataFile toCompress) {
            PPMTables ppmTables = new PPMTables(_maxOrder);
            ppmTables.FillTables(toCompress);
            return toCompress;
        }

        public DataFile Decompress(DataFile toDecompress) {
            throw new System.NotImplementedException();
        }
    }
}
    