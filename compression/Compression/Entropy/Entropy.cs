using System;
using System.Collections.Generic;
using System.Linq;

/*
 * The Entropy Math Class
 * Contain math methods for calculating entropy of any given string
 */

namespace Compression.Entropy
{
    public class Entropy
    {        
        /*
        * Calculating log_b(a)
        * @param double a: number to be calculated & double b: base of log
        * @returns the logarithm of a specified number a with a specified base b.
        */
        public double logCalc(double a, double b)
        {
            return Math.Log(a) / Math.Log(b);
        }

        public double calcEntropy()
        {
            Dictionary<char, double> table = new Dictionary<char, double>();
            double result = 0; 
            freq = symbol.Value / totalCount; 
            
            return result; 
        }

    }
}