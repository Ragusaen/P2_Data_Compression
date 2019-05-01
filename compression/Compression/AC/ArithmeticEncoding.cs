using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Compression.PPM;

namespace Compression.Arithmetic{
    public class ArithmeticEncoding : DataFileIterator {
        public int totalCount = 0;
        private double _low = 0, _high = 100; 
        public ArithmeticEncoding(DataFile file, double low, double high) : base(file) {
            _low = low;
            _high = high;
        }

        public Dictionary<byte,double> CalcFreq() {
             var gb = file.GetAllBytes();
             var table = new Dictionary<byte, int>();
             foreach (byte bt in gb) {
                 totalCount += 1;
                 if (!table.ContainsKey(bt)) {
                     table.Add(bt, 1);
                 }
                 else
                     table[bt] += 1;
             }
             
             var freqTable = new Dictionary<byte, double>();
             foreach (var t in table) {
                 freqTable.Add(t.Key, t.Value / totalCount);
             }
             
             return freqTable;
        }
        
        public Dictionary<double[], byte> SetIntervals(List<ContextTable> ppmTables){
            var byteIntervals = new Dictionary<double[], byte>();
            double resTag;
            int cumCount;

            foreach (ContextTable table in ppmTables){
                table.UpdateCumulativeCount();
                
                foreach(var content in table.ContextDict) {   
                   foreach(var symbol in content.Value){
                       if(symbol.Key is Letter letter){
                          double tempLow = _low, tempHigh = _high;
                          double[] interval = CalcInterval(_low, _high, symbol.Value.Count , symbol.Value.CumulativeCount, table.TotalCount);

                          _low = interval[0];
                          _high = interval[1];
                        
                          byteIntervals.Add(interval, letter.Data);
                       }
                   }

                }
            }

            return byteIntervals;
        }

        public ContextTable EvalTable(ContextTable ppmTable) {
            foreach (var t in ppmTable) {
                foreach (var symbol in t) {
                    if (symbol.Key is Letter letter) {
                        
                    }
                }
            }
            return ppmTable; 
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