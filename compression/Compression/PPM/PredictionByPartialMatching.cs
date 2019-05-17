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
                    if (contextLength == -1) {
                        SymbolInfo symbolInfo = contextTables.LookUpMinusFirstOrder(input[i]);
                        arithmeticCoder.Encode(symbolInfo, contextTables.TotalCountOfMinusFirstOrder());
                    }
                    
                    byte[] context = new byte[contextLength];
                    Array.Copy(input, i - contextLength, context, 0, contextLength);

                    Console.WriteLine($"Context length: {contextLength}, char {(char)input[i]}");

                    int totalCount = contextTables.TotalCountOfContext(context);
                    if (totalCount > 0) {
                        SymbolInfo symbolInfo = contextTables.LookUp(context, input[i]);
                        if (symbolInfo.Count > 0) {
                            arithmeticCoder.Encode(symbolInfo, totalCount);
                            break;
                        }
                        arithmeticCoder.Encode(contextTables.GetEscapeInfo(context), totalCount);
                    }
                }
            }

            Console.WriteLine(arithmeticCoder.GetEncodedBitString());
            return arithmeticCoder.GetEncodedBitString().ToArray();
        }
    }
}
    