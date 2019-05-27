using System;
using System.Collections.Generic;

namespace Compression.PPM {
    public class ContextTablePrinter {
        public void ConsolePrint(ContextTable CTP) {
            Console.WriteLine("Context | Symbol | Count | Cum_Count");
            foreach (var t in CTP) {
                var cArr = new char[t.Key.Length];

                for (var i = 0; i < t.Key.Length; i++) cArr[i] = (char) t.Key[i];

                var context = new string(cArr);

                foreach (var u in t.Value)
                    PrintLine(context, ((char) u.Key).ToString(), u.Value.Count, u.Value.CumulativeCount);

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

        public void PrintAll(List<ContextTable> ppmTables) {
            var i = 0;

            foreach (var t in ppmTables) {
                Console.WriteLine("Order is: " + (i++ - 1));
                ConsolePrint(t);
            }
        }
    }
}