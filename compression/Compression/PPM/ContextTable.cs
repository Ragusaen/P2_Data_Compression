using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.SqlServer.Server;

namespace Compression.PPM{
    public class ContextTable : IEnumerable<Dictionary<byte, SymbolInfo>> {
        public readonly Dictionary<byte[], SymbolDictionary> ContextDict = new Dictionary<byte[], SymbolDictionary>(new ByteArrayComparer());
        private readonly int _defaultEscaping;
        
        public ContextTable(int defaultEscaping = 0) {
            _defaultEscaping = defaultEscaping;
        }

        public bool UpdateContext(byte[] context, byte symbol) {
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

        public IEnumerator<Dictionary<byte, SymbolInfo>> GetEnumerator() {
            return ContextDict.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}