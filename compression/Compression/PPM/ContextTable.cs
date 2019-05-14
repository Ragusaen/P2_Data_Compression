using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.SqlServer.Server;

namespace Compression.PPM{
    public class ContextTable : IEnumerable<Dictionary<ISymbol, SymbolInfo>> {
        public Dictionary<SymbolList, SymbolDictionary> ContextDict = new Dictionary<SymbolList, SymbolDictionary>();
        private readonly int _defaultEscaping;
        
        public ContextTable(int defaultEscaping = 0) {
            _defaultEscaping = defaultEscaping;
        }

        public bool UpdateContext(byte[] inputContext, byte inputSymbol) {
            SymbolList context = new SymbolList(inputContext);
            ISymbol symbol = ByteToISymbol.ConvertSingle(inputSymbol);
            
            if (!ContextDict.ContainsKey(context)) {
                ContextDict.Add(context, new SymbolDictionary());
                ContextDict[context].EscapeInfo.Count = _defaultEscaping+1;
                ContextDict[context].AddNew(symbol);
                return false;
            }

            if (ContextDict[context].ContainsKey(symbol)) {
                ContextDict[context][symbol].Count++;
                return true;
            }
            
            ContextDict[context].AddNew(symbol);
            ContextDict[context].EscapeInfo.Count++;
            return false;
        }

        public void CalculateAllCounts() {
            foreach (var t in ContextDict) {
                t.Value.CalculateCounts();
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