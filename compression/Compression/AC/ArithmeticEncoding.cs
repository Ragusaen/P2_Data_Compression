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
                /*foreach(ContextInTable content in table) {
                 double tempLow = low, tempHigh = high;
                 
                 double[,] interval = calcInterval(low, high, count, cumCount);

                 low = interval[0][0];
                 high = interval[0][1];
                 foreach(Symbol s in content){
                 
                 byteIntervals.Add(interval, s);
                 }
                 }*/
            }

            return byteIntervals;
        }

        public int CalcTag(Dictionary<double[,], byte> byteIntervals){
            var uniqueFinalTag = byteIntervals.Keys.Last();

            return 0;
        }

        public double[] CalcInterval(double prevLow, double prevHigh, int count, int cumCount){
            double totalCount = 0; // Skal fixes;
            double lowInterval = prevLow + (double) count * (prevHigh - prevLow) / totalCount;
            double highInterval = prevLow + (double) cumCount * (prevHigh - prevLow) / totalCount;
            return new double[2] {lowInterval, highInterval};;
        }
    }
}