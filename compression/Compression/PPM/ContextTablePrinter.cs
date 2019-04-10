using System;
using Compression.ByteStructures;

namespace Compression.PPM{
    public class ContextTablePrinter {
        public void ConsolePrint(ContextTable CTP) {
            CTP.UpdateCumulativeCount();
            CTP.CalculateTotalCount();
            Console.WriteLine("Context | Symbol | Count | Cum_Count");
            foreach (var t in CTP.ContextList) {
                foreach (var u in t.SymbolList) {
                    ByteArrayPrinter.PrintToString(t.ContextBytes);
                    int extraSymbolChars = 0;
                    
                    if (u.Data is Letter letter)
                        Console.Write("".PadLeft(10 - t.ContextBytes.Length, ' ') + (char) letter.Data);
                    else {
                        Console.Write("".PadLeft(10 - t.ContextBytes.Length, ' ') + "<esc>");
                        extraSymbolChars = 4;
                    }

                    Console.WriteLine("".PadLeft(8-extraSymbolChars, ' ') + u.Count +
                        "".PadLeft(8-u.Count.ToString().Length, ' ') + u.CumulativeCount
                    );
                }
            }
            Console.WriteLine("TC: ".PadLeft(27, ' ') + "{0}", CTP.TotalCount);
        }

        public void ConsolePrintAll(PredictionByPartialMatching PPM) {
            foreach (var t in PPM.OrderList) {
                ConsolePrint(t);
            }
        }
    }
}