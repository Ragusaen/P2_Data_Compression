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
       * Calculating the entropy of any given string s
       * @param string s: given string
       * @returns result: entropy of string
       */
        public double CalcEntropy(string s) {
            //check if string is not empty
            if (s != null) {
                //Convert string to byte[]
                byte[] b = new byte[s.Length];
                int num = 0;
                foreach (char item in s.ToCharArray()) {
                    b[num++] = (byte) item;
                }
                
                //count byte frequencies into dictionary
                var table = new Dictionary<byte, int>();
                foreach (byte bt in b) {
                    if (!table.ContainsKey(bt))
                        table.Add(bt, 1);
                    else
                        table[bt] += 1;
                }
                
                //calculate entropy
                int len = b.Length;
                double result = 0.0;
                foreach (var item in table) {
                    double freq = (double) item.Value / len;
                    result += -(freq * Math.Log(freq, 2));
                }

                return result;
            }

            throw new ArgumentNullException(nameof(s));
        }
    }  
}