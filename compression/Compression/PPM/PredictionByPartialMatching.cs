using System;
using Compression.AC_R;

namespace Compression.PPM{
    public class PredictionByPartialMatching : ICompressor{
        private readonly int _maxOrder;
        
        public PredictionByPartialMatching(int maxOrder = 5) {
            _maxOrder = maxOrder;
        }
        
        public DataFile Compress(DataFile toCompress) {
            PPMTables ppmTables = new PPMTables(_maxOrder);
            ArithmeticCoder ac = new ArithmeticCoder();
            int byteMax = byte.MaxValue + 1;

            for (int i = 0; i < toCompress.Length; i++) {
                if ( i % 100000 == 0)
                    Console.Write($"\r{(double)i / toCompress.Length}");
                
                
                Entry entry = new Entry(toCompress.GetByte(i), GetContextFromFile(toCompress, i));
                
                ContextTable.ToEncode toEncode;
                EncodeInfo encodeInfo;

                int infiniteCounter = 0;
                while ( (toEncode = ppmTables.LookUpAndUpdate(entry, out encodeInfo)) != ContextTable.ToEncode.EncodeSymbol) {
                    if (toEncode == ContextTable.ToEncode.EncodeEscape)
                        ac.Encode(encodeInfo.Count, encodeInfo.CumulativeCount, encodeInfo.TotalCount);
                    entry.NextContext();
                    infiniteCounter++;
                    if (infiniteCounter > 6) {
                        Console.WriteLine($"Loop gone wrong: {entry}");
                    }
                }
                ac.Encode(encodeInfo.Count, encodeInfo.CumulativeCount, encodeInfo.TotalCount);
            }
            ac.Finalize();
            var output = ac.GetEncodedBitString().ToArray();
//            Console.WriteLine($"Output: {ac.GetEncodedBitString()}");
            
            
            ContextTablePrinter ctp = new ContextTablePrinter();
//            ctp.PrintAll(ppmTables._orderList);

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
    