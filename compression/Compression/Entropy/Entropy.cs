using System;
using System.Collections.Generic;
using Compression.AC;

/*
 * The Entropy Math Class
 * Contain method for calculating entropy of any given file
 */

namespace Compression.Entropy {
    public class Entropy {
       /*
       * Calculating the entropy of any given file
       * @param file: datafile file is the given file
       * @returns result: entropy of the given file
       */
        public double CalcEntropy(DataFile file) {
            ArithmeticMath am = new ArithmeticMath(file);
            Dictionary<byte, double> freqtable = am.CalcFreq(); 
                //calculate entropy
                double result = 0.0;
                foreach (var item in freqtable) {
                    result += -(item.Value * Math.Log(item.Value, 2));
                }

                return result;
            }
        }
}  