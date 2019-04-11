using System.Collections.Generic;
using Compression.PPM;
using System.Linq;

namespace Compression.AC {
    public class ArithmeticEncoding {
        public DataFile encodingArithmetic(DataFile input, List<ContextTable> ppmTables) {
            return null;
        }

        public Dictionary<double[,], byte> setIntervals(List<ContextTable> ppmTables) {
            Dictionary<double[,], byte> byteIntervals = new Dictionary<double[,], byte>();
            double resTag, low = 0, high = 1;
            int cumCount;

            foreach (ContextTable table in ppmTables) {
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

        public int calcTag(Dictionary<double[,], byte> byteIntervals) {
            double[,] uniqueTag = byteIntervals.Keys;
            double[,] uniqueFinalTag = byteIntervals.Keys.Last;

            return null;
        }

        public double[,] calcInterval(double prevLow, double prevHigh, int count, int cumCount) {
            return new double[prevLow + (count * (prevHigh - prevLow)) / totalCount,
                prevLow + (cumCount * (prevHigh - prevLow)) / totalCount];
        }

        public double calcEntropy(List<ContextTable> ppmTables) {
            double entropy = 0;
            
            foreach (ContextTable table in ppmTables) {
                foreach (Context context in table) {
                    foreach (Symbol s in context) {
                    }
                }
            }

            return entropy;
        }
    }
}   