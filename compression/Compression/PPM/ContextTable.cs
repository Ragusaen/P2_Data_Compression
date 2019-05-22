using System.Collections;
using System.Collections.Generic;

namespace Compression.PPM{
    public class ContextTable : IEnumerable<Dictionary<byte, SymbolInfo>> {
        public enum ToEncode{
            EncodeNothing, EncodeSymbol, EncodeEscape
        }
        public readonly Dictionary<byte[], SymbolDictionary> ContextDict = new Dictionary<byte[], SymbolDictionary>(new ByteArrayComparer());
        
        public ToEncode UpdateContext(Entry entry) {
            byte symbol = entry.Symbol;
            byte[] context = entry.Context;
            ToEncode toEncode;
            
            if (ContextDict.ContainsKey(context)) { // did not match context, do not encode anything
                if (ContextDict[context].ContainsKey(symbol)) { // matched context and symbol, encode symbol
                    ContextDict[context].IncrementSymbol(symbol);
                    toEncode = ToEncode.EncodeSymbol;
                } else {
                    // Matched context but not symbol, encode an escape symbol
                    ContextDict[context].AddNew(symbol);
                    ContextDict[context].IncrementEscape();
                    toEncode = ToEncode.EncodeEscape;
                }
            } else {
                ContextDict.Add(context, new SymbolDictionary());
                ContextDict[context].AddNew(symbol);
                ContextDict[context].IncrementEscape();
                toEncode = ToEncode.EncodeNothing;
            }

            return toEncode;
        }

        public IEnumerator<Dictionary<byte, SymbolInfo>> GetEnumerator() {
            return ContextDict.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}