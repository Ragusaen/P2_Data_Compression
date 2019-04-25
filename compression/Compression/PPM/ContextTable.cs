using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.SqlServer.Server;

namespace Compression.PPM{
    public class ContextTable : IEnumerable<Dictionary<ISymbol, SymbolInfo>> {
        public Dictionary<SymbolList, SymbolDictionary> ContextDict = new Dictionary<SymbolList, SymbolDictionary>();
        public int TotalCount;
        private readonly int _defaultEscaping;
        
        public ContextTable(int defaultEscaping = 0) {
            _defaultEscaping = defaultEscaping;
        }

        public bool UpdateContext(byte[] inputContext, byte inputSymbol) {
            SymbolList context = new SymbolList(inputContext);
            ISymbol symbol = ByteToISymbol.ConvertSingle(inputSymbol);
            ISymbol escape = new EscapeSymbol();
            
            if (!ContextDict.ContainsKey(context)) {
                ContextDict.Add(context, new SymbolDictionary());
                ContextDict[context].Add(escape, new SymbolInfo(count:_defaultEscaping));
                ContextDict[context].AddNew(symbol);
                return false;
            }

            if (ContextDict[context].ContainsKey(symbol)) {
                ContextDict[context][symbol].Count++;
                return true;
            }
            
            ContextDict[context].AddNew(symbol);
            return false;
        }

        public void CalculateTotalCount() {
            TotalCount = ContextDict.Values.Sum(p => p.Sum(q => q.Value.Count));
        }

        public void UpdateCumulativeCount() {
            int cumCount = 0;

            foreach (var t in ContextDict.Values) {
                foreach (var t1 in t) {
                    t1.Value.CumulativeCount = cumCount += t1.Value.Count;
                }
            }
        }

        public IEnumerator<Dictionary<ISymbol, SymbolInfo>> GetEnumerator() {
            return ContextDict.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}