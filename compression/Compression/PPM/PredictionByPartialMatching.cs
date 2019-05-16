using System;
using System.Collections.Generic;
using System.Diagnostics.PerformanceData;
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
            ArithmeticCoder ac = new ArithmeticCoder();
            ContextTable.ToEncode ct;
            int byteMax = byte.MaxValue + 1;

            // encoding the first byte, this simplifies the algorithm
            Entry entry = new Entry(toCompress.GetByte(0), new byte[0]);
            EncodeInfo ei = ppmTables.LookUpAndUpdate(entry, out ct);
            ac.Encode(1, entry.Symbol, byteMax);
            
            
            for (int i = 1; i < toCompress.Length; i++) {
                entry.Context = GetContextFromFile(toCompress, i);
                entry.Symbol = toCompress.GetByte(i);

                for (int j = entry.Context.Length; j >= 0; j--) {
                    ei = ppmTables.LookUpAndUpdate(entry, out ct);

                    if (ct == ContextTable.ToEncode.EncodeSymbol) {
                        ac.Encode(ei.Count, ei.CumulativeCount, ei.TotalCount);
                        break;
                    }

                    if (ct == ContextTable.ToEncode.EncodeEscape) {
                        ac.Encode(ei.Count, ei.CumulativeCount, ei.TotalCount);
                    }

                    else if (ct == ContextTable.ToEncode.EncodeNothing) { }

                    else {
                        // negative count is to encode -1. order and escape symbol from 0. order
                        ac.Encode(-ei.Count, ei.CumulativeCount, ei.TotalCount);
                        ac.Encode(1, entry.Symbol, byteMax);
                        break;   
                    }
                    entry.NextContext();
                }
            }

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

        /*private byte[] Encode(byte[] input, PPMTables contextTables) {
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
        }*/
    }
}
    