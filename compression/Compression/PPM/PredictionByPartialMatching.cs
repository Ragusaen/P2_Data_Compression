using System;
using Compression.AC;

namespace Compression.PPM{
    public class PredictionByPartialMatching : ICompressor{
        private readonly int _maxOrder;
        private int _cleanUpLimit;
        
        public PredictionByPartialMatching(int maxOrder = 5, int cleanUpLimit = 100000) {
            _maxOrder = maxOrder;
            _cleanUpLimit = cleanUpLimit;
        }
        
        public DataFile Compress(DataFile toCompress) {
            PPMTables ppmTables = new PPMTables(_maxOrder);
            ArithmeticCoder ac = new ArithmeticCoder();
            
            for (int i = 0; i < toCompress.Length; i++) {
                if (i % _cleanUpLimit == 0) {
                    ppmTables.CleanUp();
                }
                
                Entry entry = new Entry(toCompress.GetByte(i), GetContextFromFile(toCompress, i));
                ContextTable.ToEncode toEncode;
                EncodeInfo encodeInfo;
                
                while ( (toEncode = ppmTables.LookUpAndUpdate(entry, out encodeInfo)) != ContextTable.ToEncode.EncodeSymbol) {
                    if (toEncode == ContextTable.ToEncode.EncodeEscape)
                        ac.Encode(encodeInfo.Count, encodeInfo.CumulativeCount, encodeInfo.TotalCount);
                    entry.NextContext();
                }
            }
            
            ac.FinalizeInterval();
            var output = ac.GetEncodedBitString().ToArray();

            return new DataFile(output);
        }
        
        private byte[] GetContextFromFile(DataFile file, int i) {
            if (_maxOrder == 0)
                return new byte[0];
            
            if(i > _maxOrder)
                return file.GetBytes(i - _maxOrder, _maxOrder);
            return file.GetBytes(0, i);
        }

        public DataFile Decompress(DataFile toDecompress) {
            throw new System.NotImplementedException();
        }
    }
}
    