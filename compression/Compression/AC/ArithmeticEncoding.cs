using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web.Security.AntiXss;
using Compression.AC;
using Compression.PPM;

namespace Compression.Arithmetic{
    public class ArithmeticEncoding : DataFileIterator {
        public int totalCount = 0;
        public ArithmeticEncoding(DataFile file) : base(file) {
            this.file = file; 
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
                 freqTable.Add(t.Key, t.Value / (double) totalCount);
                 //Console.WriteLine(t.Value/ (double) totalCount + " for: " + t.Key);
                 //Console.WriteLine(t.Value);
             }
             
             return freqTable;
        }
        
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

        public Dictionary<Interval, byte> CalcTag(Dictionary<byte, Interval> intervalDict, Dictionary<byte, double> freqTable) {
            var byteArray = file.GetAllBytes(); 
            var tagDict = new Dictionary<Interval, byte>();
            double low = 0, high = 0;
            
                foreach (var b in byteArray) {
                    foreach (var t in intervalDict) {
                        if (b.Equals(t.Key)) {
                            var currentRange = t.Value.high - t.Value.low;
                            low = low + (t.Value.low * currentRange); 
                            high = low + (t.Value.high * currentRange);
                            
                            var it = new Compression.AC.Interval(low,high);
                            tagDict.Add(it, b);
                        }
                    }
                }
            return tagDict; 
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
    }
}