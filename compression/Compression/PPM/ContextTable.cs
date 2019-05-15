using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.SqlServer.Server;

namespace Compression.PPM{
    public class ContextTable : IEnumerable<Dictionary<byte, SymbolInfo>> {
        public readonly Dictionary<byte[], SymbolDictionary> ContextDict = new Dictionary<byte[], SymbolDictionary>(new ByteArrayComparer());
        public bool UpdateContext(byte[] context, byte symbol) {
            if (!ContextDict.ContainsKey(context)) {
                ContextDict.Add(context, new SymbolDictionary());
                ContextDict[context].AddNew(symbol);
                ContextDict[context].EscapeInfo.Count = 1;
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
            foreach (var t in ContextDict.Values) {
                t.CalculateCounts();
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