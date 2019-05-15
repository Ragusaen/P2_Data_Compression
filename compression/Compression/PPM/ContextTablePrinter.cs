using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Compression.ByteStructures;

namespace Compression.PPM{
    public class ContextTablePrinter {
        public void ConsolePrint(ContextTable CTP) {
            foreach (var t in CTP.ContextDict) {
                t.Value.CalculateCounts();
            }
            
            Console.WriteLine("Context | Symbol | Count | Cum_Count");
            foreach (var t in CTP.ContextDict) {
                string context = t.Key.ToString();

                foreach (var u in t.Value) {
                    PrintLine(context, u.Key.ToString(), u.Value.Count, u.Value.CumulativeCount);
                }

                PrintLine(context, "<esc>", t.Value.EscapeInfo.Count, t.Value.EscapeInfo.CumulativeCount);
                Console.WriteLine("".PadLeft(14, '-') + " Total Count " + t.Value.TotalCount);
            }
        }

        private void PrintLine(string context, string symbol, int count, int cumCount) {
            Console.Write(context);
            Console.Write("".PadLeft(10 - context.Length, ' ') + symbol);
            Console.Write("".PadLeft(9 - symbol.Length, ' ') + count);
            Console.Write("".PadLeft(8 - count.ToString().Length, ' ') + cumCount + "\n");
        }
    }
}