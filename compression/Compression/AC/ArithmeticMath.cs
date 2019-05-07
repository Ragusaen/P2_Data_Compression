using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using compression.AC;
using Compression.ByteStructures;

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
        public Dictionary<Interval, byte> SetIntervals(Dictionary<byte, double> freqTable) {
                    double highBound = 0;

                    Dictionary<Interval, byte> intervalDict = new Dictionary<Interval, byte>();
                    foreach (var t in freqTable) {
                        var lowBound = highBound;
                        highBound = lowBound + t.Value;
                       //Console.WriteLine(highBound + " " + lowBound);
                       UnevenByte ubHigh = new BinaryFractionInIntervalFinder().FractionToUnevenByte(highBound);
                       UnevenByte ubLow = new BinaryFractionInIntervalFinder().FractionToUnevenByte(lowBound);
                       
                       var it = new Interval(ubHigh, ubLow);
                       intervalDict.Add(it, t.Key); 
                       //Console.WriteLine(intervalArr[1]);
                    }
                    return intervalDict; 
                }
        #endregion
        
        #region Calculating tag method
        public Dictionary<Interval, byte> CalcTag() {
           /* var byteArray = file.GetAllBytes();
            Dictionary<byte, double> freqTable = CalcFreq(); 
            Dictionary<Interval, byte> intervalDict = SetIntervals(freqTable); 
            var tagDict = new Dictionary<Interval, byte>();
            double low = 0; 
            foreach (var b in byteArray) {
                foreach (var t in intervalDict) {
                    if (b.Equals(t.Value)) {
                        var currentRange = t.Key.high - t.Key.low;

                        var high = t.Key.low + (t.Key.low + (t.Key.high * currentRange));
                        low = t.Key.low + (t.Key.low + (t.Key.low * currentRange));

                        //low = t.Key.low; 
                        
                        var it = new Interval(low,high);     
                        tagDict.Add(it, b);
                    }
                }
            }*/
            return null; 
        } 
        #endregion
        
        #region Representing bounds with a binary representation
           public Dictionary<UnevenByte, byte> UniqueBinaryTag(Dictionary<Interval, byte> intervalDict) {
               Dictionary<UnevenByte,byte> encodedBytes = new Dictionary<UnevenByte, byte>();
               foreach (var t in intervalDict) {
                   UnevenByte ub = new BinaryFractionInIntervalFinder().GetBinaryFraction(t.Key.low,t.Key.high);
                   
                   encodedBytes.Add(ub, t.Value);
               }

               return encodedBytes;
           }
        #endregion
        #endregion
    }
}