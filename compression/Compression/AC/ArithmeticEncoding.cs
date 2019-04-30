using System;
using System.Collections.Generic;
using System.Linq;
using Compression.PPM;

namespace Compression.Arithmetic{
    public class ArithmeticEncoding{
        public DataFile EncodingArithmetic(DataFile input, List<ContextTable> ppmTables){
            
            return null;
        }

        public Dictionary<byte, int> ProcessPpmTables(List<ContextTable> ppmTables){
            return null;
        }
        
        

        public Dictionary<double[], byte> SetIntervals(List<ContextTable> ppmTables){
            var byteIntervals = new Dictionary<double[], byte>();
            double resTag, low = 0, high = 1;
            int cumCount;

            foreach (ContextTable table in ppmTables){
                table.UpdateCumulativeCount();
                
                foreach(var content in table.ContextDict) {   
                   foreach(var symbol in content.Value){
                       if(symbol.Key is Letter letter){
                          double tempLow = low, tempHigh = high;
                          double[] interval = CalcInterval(low, high, symbol.Value.Count , symbol.Value.CumulativeCount, table.TotalCount);

                          low = interval[0];
                          high = interval[1];
                        
                          byteIntervals.Add(interval, letter.Data);
                       }
                   }

                }
            }

            return byteIntervals;
        }

        public int CalcTag(Dictionary<double[,], byte> byteIntervals){
            var uniqueFinalTag = byteIntervals.Keys.Last();

            return 0;
        }

        public double[] CalcInterval(double prevLow, double prevHigh, int count, int cumCount, int totalCount){
            double lowInterval = prevLow + (double) count * (prevHigh - prevLow) / totalCount;
            double highInterval = prevLow + (double) cumCount * (prevHigh - prevLow) / totalCount;
            return new double[2] {lowInterval, highInterval};
        }
    }
}