using System;
using System.Linq;
using Compression.ByteStructures;

namespace Compression.PPM{
    public class ContextTablePrinter {
        public void ConsolePrint(ContextTable CTP) {
            CTP.UpdateCumulativeCount();
            CTP.CalculateTotalCount();
            Console.WriteLine("Context | Symbol | Count | Cum_Count");
            foreach (var t in CTP.ContextDict) {
                foreach (var u in t.Value) {
                    Console.Write(t.Key.ToString());
                    int extraSymbolChars = 0;
                    
                    if (u.Key is Letter letter)
                        Console.Write("".PadLeft(10 - t.Key.ToString().Length, ' ') + (char) letter.Data);
                    else {
                        Console.Write("".PadLeft(10 - t.Key.ToString().Length, ' ') + "<esc>");
                        extraSymbolChars = 4;
                    }

                    Console.WriteLine("".PadLeft(8-extraSymbolChars, ' ') + u.Value.Count +
                        "".PadLeft(8-u.Value.Count.ToString().Length, ' ') + u.Value.CumulativeCount
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