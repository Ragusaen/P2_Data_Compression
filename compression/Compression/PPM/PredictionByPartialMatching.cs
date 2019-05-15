using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using compression.AC_R;
using Compression.ByteStructures;

namespace Compression.PPM{
    public class PredictionByPartialMatching : ICompressor{
        private readonly int _maxOrder;
        
        public PredictionByPartialMatching(int maxOrder = 5) {
            _maxOrder = maxOrder;
        }
        
        public DataFile Compress(DataFile toCompress) {
            PPMTables ppmTables = new PPMTables(_maxOrder);
            ppmTables.FillTables(toCompress);
            byte[] output = Encode(toCompress.GetAllBytes(), ppmTables);
            return new DataFile(output);
        }

        public DataFile Decompress(DataFile toDecompress) {
            throw new System.NotImplementedException();
        }

        private byte[] Encode(byte[] input, PPMTables contextTables) {
            ArithmeticCoder arithmeticCoder = new ArithmeticCoder();

            for (int i = 0; i < input.Length; i++) {
                int maxOrder = i < _maxOrder ? i : _maxOrder;
                for (int contextLength = maxOrder; contextLength >= -1; --contextLength) {
                    byte[] context = new byte[contextLength];
                    Array.Copy(input, i, context, 0, contextLength);

                    SymbolInfo symbolInfo = contextTables.LookUp(context, input[i]);
                    if (symbolInfo.Count > 0) {
                        int totalCount = contextTables.TotalCountOfContext(context);
                        arithmeticCoder.Encode(symbolInfo, totalCount);
                        break;
                    }
                }
            }

            return arithmeticCoder.GetEncodedBitString().ToArray();
        }
    }
}
    