using System.Collections;
using System.Collections.Generic;

namespace Compression.PPM{
    public class ContextTable : IEnumerable<Dictionary<byte, SymbolInfo>> {
        public enum ToEncode{
            EncodeNothing, EncodeSymbol, EncodeEscape, EncodeMinusFirst
        }
        
        public readonly Dictionary<byte[], SymbolDictionary> ContextDict = new Dictionary<byte[], SymbolDictionary>(new ByteArrayComparer());
        public ToEncode UpdateContext(Entry entry) {
            byte symbol = entry.Symbol;
            byte[] context = entry.Context;
            
            if (!ContextDict.ContainsKey(context)) { // did not match context, do not encode anything
                ContextDict.Add(context, new SymbolDictionary());
                ContextDict[context].AddNew(symbol);
                ContextDict[context].IncrementEscape();
                return ToEncode.EncodeNothing;
            }

            if (ContextDict[context].ContainsKey(symbol)) { // matched context and symbol, encode symbol
                ContextDict[context].IncrementSymbol(symbol);
                return ToEncode.EncodeSymbol;
            }
            
            // Matched context but not symbol, encode an escape symbol
            if (context.Length == 0) {
                ContextDict[context].AddNew(symbol);
                ContextDict[context].IncrementEscape();
                return ToEncode.EncodeMinusFirst;
            }
            
            ContextDict[context].AddNew(symbol);
            ContextDict[context].IncrementEscape();
            return ToEncode.EncodeEscape;
        }

        public IEnumerator<Dictionary<byte, SymbolInfo>> GetEnumerator() {
            return ContextDict.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}