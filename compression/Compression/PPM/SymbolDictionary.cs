using System.Collections.Generic;

namespace Compression.PPM{
    public class SymbolDictionary : Dictionary<byte, SymbolInfo>{
        
        public readonly SymbolInfo EscapeInfo = new SymbolInfo(count: 0);
        public int TotalCount { get; set; }

        public SymbolDictionary() { }
        public SymbolDictionary(byte symbol) {
            Add(symbol, new SymbolInfo());
            UpdateCounts();
        }
        
        public void AddNew(byte symbol) {
            Add(symbol, new SymbolInfo());
            UpdateCounts();
        }

        public void IncrementSymbol(byte symbol) {
            this[symbol].Count++;
            UpdateCounts();
        }

        public void IncrementEscape() {
            EscapeInfo.Count++;
            UpdateCounts();
        }

        public void UpdateCounts() {
            TotalCount++;
            CalculateCumulativeCounts();
        }

        public void CalculateCumulativeCounts() {
            int cumCount = 0;
            
            foreach (var t in this) {
                t.Value.CumulativeCount = cumCount += t.Value.Count;
            }

            EscapeInfo.CumulativeCount = cumCount + EscapeInfo.Count;
        }
    }
}