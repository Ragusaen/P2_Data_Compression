using System.Collections.Generic;
using Compression.AC;

/*
 * Arithmetic encoding class
 * Contains all methods for calculating unique tag
 */
namespace Compression.Arithmetic {
    public class ArithmeticEncoding : DataFileIterator {

        public ArithmeticEncoding(DataFile file) : base(file) {
            this.file = file; 
        }
        
        
        #region Methods
        /*
         * Contains Encoding methods 
         */

        #region Calculating tag method
        /*
        public Dictionary<Interval, byte> CalcTag(Dictionary<byte, Interval> intervalDict, Dictionary<byte, double> freqTable) {
            var byteArray = file.GetAllBytes(); 
            var tagDict = new Dictionary<Interval, byte>();
            double low = 0;

            foreach (var b in byteArray) {
                    foreach (var t in intervalDict) {
                        if (b.Equals(t.Key)) {
                            var currentRange = t.Value.high - t.Value.low;
                            //Console.WrinteLine(t.Value.high + " " + t.Value.low);
                            
                            //Console.WriteLine("low: " + low);
                            var high = low + (t.Value.high * currentRange);
                            low =  low + (t.Value.low * currentRange); 
                            
                            //Console.WriteLine(low + " = " + t.Value.low);
                            //Console.WriteLine("high: " + high);
                            var it = new Compression.AC.Interval(low,high);
                            
                            tagDict.Add(it, b);
                        }
                    }
                }
            return tagDict; 
        } */
        #endregion

        #region Encoding file method

        public DataFile EncodeFIle() {
            ArithmeticMath am = new ArithmeticMath(file);
            Dictionary<byte, byte> byteFile = am.ConvertIntervalToBinary
                (am.SetIntervals(am.CalcFreq()));
            
            return null; 
        }

        #endregion
       
        #endregion
    }
}