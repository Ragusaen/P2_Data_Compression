using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web.Hosting;

/*
 * The Entropy Math Class
 * Contain math methods for calculating entropy of any given string
 */

namespace Compression.Entropy {
    public class Entropy {
        /*
        * Calculating log_b(a)
        * @param double a: number to be calculated & double b: base of log
        * @returns the logarithm of a specified number a with a specified base b.
        */
        public double LogCalc(double a, double b) {
            return Math.Log(a) / Math.Log(b);
        }
        
       /*
       * Calculating the entropy of any given string s
       * @param string s: given string
       * @returns result: entropy of string
       */
        public double CalcEntropy(string s) {
           var table = new Dictionary<char, int>();
            foreach (char c in s) {
                if (!table.ContainsKey(c))
                    table.Add(c,1);
                else
                    table[c] += 1;
            }

            int len = s.Length;
            double result = 0.0;
            foreach (var item in table) {
                double freq = (double) item.Value / len;
                result += -(freq * LogCalc(freq,2));
            }
            return result;
        }
    }  
}