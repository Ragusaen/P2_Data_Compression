
using System;
using System.Collections.Generic;
using System.Linq;

namespace Compression.AC {
    public class ArithmeticMath : DataFileIterator {
        #region Field variables
        private int _totalCount = 0;
        #endregion

        #region Constructor
        public ArithmeticMath(DataFile file) : base(file) {
            this.file = file; 
        }
        #endregion

        #region Methods
        /*
         * Contains methods necessary for ArithmeticEncoding, to encode.  
         */
        #region Calculating probalility method
        public Dictionary<byte,double> CalcFreq() {
            var gb = file.GetAllBytes();
            var table = new Dictionary<byte, int>();
            foreach (byte bt in gb) {
                _totalCount += 1;
                if (!table.ContainsKey(bt)) {
                    table.Add(bt, 1);
                }
                else
                    table[bt] += 1;
            }
             
            var freqTable = new Dictionary<byte, double>();
            foreach (var t in table) {
                freqTable.Add(t.Key, t.Value / (double) _totalCount);
                //Console.WriteLine(t.Value/ (double) totalCount + " for: " + t.Key);
                //Console.WriteLine(t.Value);
            }
             
            return freqTable;
        }
        #endregion

        #region Calculating initial intervals method
        public Dictionary<byte, Interval> SetIntervals(Dictionary<byte, double> freqTable) {
                    double highBound = 0;
                    
                    Dictionary<byte, Interval> intervalDict = new Dictionary<byte, Interval>();
                    foreach (var t in freqTable) {
                        var lowBound = highBound;
                        highBound = lowBound + t.Value; 
                       //Console.WriteLine(highBound + " " + lowBound);
        
        
                       var it = new Compression.AC.Interval(lowBound, highBound);
                       intervalDict.Add(t.Key, it); 
                       //Console.WriteLine(intervalArr[1]);
                    }
                    return intervalDict; 
                }
        #endregion

        #region Representing bounds with a binary representation
           public Dictionary<byte, byte> ConvertIntervalToBinary(Dictionary<byte, Interval> intervalDict) {
                
                Dictionary<byte, byte> binaryDict = new Dictionary<byte, byte>();
                double encodedByte; 
                foreach (var t in intervalDict) {
                    
                    //binaryDict.Add(); 
                    //Console.WriteLine(intervalArr[1]);
                }
                return binaryDict; 
            }
        #endregion
        #endregion
 
    }
}