using System;
using Compression.AC;

namespace Compression.PPM {
    public class PredictionByPartialMatching : ICompressor {
        private readonly int _maxOrder;
        private readonly int _cleanUpLimit;
        
        public PredictionByPartialMatching(int maxOrder = 5, int cleanUpLimit = 100000) {
            _maxOrder = maxOrder;
            _cleanUpLimit = cleanUpLimit;
        }

        public DataFile Compress(DataFile toCompress) {
            var ppmTables = new PPMTables(_maxOrder);
            var ac = new ArithmeticCoder();

            for (int i = 0; i < toCompress.Length; i++) {
                if (i % _cleanUpLimit == 0) ppmTables.CleanUp();

                var entry = new Entry(toCompress.GetByte(i), GetContextFromFile(toCompress, i));
                ContextTable.ToEncode toEncode;
                EncodeInfo encodeInfo;

                while ((toEncode = ppmTables.LookUpAndUpdate(entry, out encodeInfo)) !=
                       ContextTable.ToEncode.EncodeSymbol) {
                    if (toEncode == ContextTable.ToEncode.EncodeEscape)
                        ac.Encode(encodeInfo.Count, encodeInfo.CumulativeCount, encodeInfo.TotalCount);
                    entry.NextContext();
                }
                ac.Encode(encodeInfo.Count, encodeInfo.CumulativeCount, encodeInfo.TotalCount);
            }

            ac.FinalizeInterval();
            var output = ac.GetEncodedBitString().ToArray();
            return new DataFile(output);
        }

        public DataFile Decompress(DataFile toDecompress) {
            throw new NotImplementedException();
        }

        public double GetStatus() {
            int i = 0;
            int _fileLength = 0;
            
            if (i == 0)
                return 0.0;

            return (double) i / _fileLength;
        }

        private byte[] GetContextFromFile(DataFile file, int i) {
            if (_maxOrder == 0)
                return new byte[0];

            if (i > _maxOrder)
                return file.GetBytes(i - _maxOrder, _maxOrder);
            return file.GetBytes(0, i);
        }
    }
}