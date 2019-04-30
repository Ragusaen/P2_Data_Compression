using System.Collections.Generic;
using System.Linq;
using Compression.PPM;

namespace Compression.Arithmetic{
    public class ArithmeticEncoding : DataFileIterator {
        private double _low, _high; 
        public ArithmeticEncoding(DataFile file, double low, double high) : base(file) {
            this._low = low;
            this._high = high;
        }  

        public Dictionary<double[], byte> SetIntervals(List<ContextTable> ppmTables){
            var byteIntervals = new Dictionary<double[], byte>();
            double resTag, low = 0, high = 1;
            int cumCount;

            foreach (ContextTable table in ppmTables){
                
                foreach(var content in table.ContextDict) {
                   
                   foreach(var symbol in content.Value){
                       double tempLow = low, tempHigh = high;
                       double[] interval = CalcInterval(low, high, symbol.Value.Count , symbol.Value.CumulativeCount, table.TotalCount);

                       low = interval[0];
                       high = interval[1];
                       byteIntervals.Add(interval, ((Letter) symbol.Key).Data);
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
            while (!AtEnd()) { }

            double lowInterval = prevLow + (double) count * (prevHigh - prevLow) / totalCount;
            double highInterval = prevLow + (double) cumCount * (prevHigh - prevLow) / totalCount;
            return new double[2] {lowInterval, highInterval};
        }
    }
}