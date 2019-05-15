using System.Collections.Generic;

namespace Compression.PPM{
    public class SymbolDictionary : Dictionary<byte, SymbolInfo>{
        public SymbolDictionary() { }

        public readonly SymbolInfo EscapeInfo = new SymbolInfo(count: 0);
        
        public int TotalCount { get; set; }

        public SymbolDictionary(byte symbol) {
            Add(symbol, new SymbolInfo());
        }
        
        public void AddNew(byte symbol) {
            Add(symbol, new SymbolInfo());
        }

        public void CalculateCounts() {
            int cumCount = 0;
            
            foreach (var t in this) {
                t.Value.CumulativeCount = cumCount += t.Value.Count;
            }

            TotalCount = EscapeInfo.CumulativeCount = cumCount + EscapeInfo.Count;
        }

        public void CalculateCumulativeCounts() {
            int cumCount = 0;
            
            foreach (var t in this) {
                t.Value.CumulativeCount = cumCount += t.Value.Count;
            }
        }

        public void CalculateTotalCount() {
            foreach (var t in this) {
                TotalCount += t.Value.Count;
            }
        }
    }
}