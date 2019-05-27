using System.Collections.Generic;

namespace Compression.PPM{
    public class ContextTable : Dictionary<byte[], SymbolDictionary> {
        public enum ToEncode{
            EncodeNothing, EncodeSymbol, EncodeEscape
        }

        public ContextTable() : base(new ByteArrayComparer()) { }
        
        public ToEncode UpdateContext(Entry entry) {
            byte symbol = entry.Symbol;
            byte[] context = entry.Context;
            ToEncode toEncode;
            
            if (ContainsKey(context)) { // did not match context, do not encode anything
                if (this[context].ContainsKey(symbol)) { // matched context and symbol, encode symbol
                    this[context].IncrementSymbol(symbol);
                    toEncode = ToEncode.EncodeSymbol;
                } else {
                    // Matched context but not symbol, encode an escape symbol
                    this[context].AddNew(symbol);
                    this[context].IncrementEscape();
                    toEncode = ToEncode.EncodeEscape;
                }
            } else {
                Add(context, new SymbolDictionary());
                this[context].AddNew(symbol);
                    this[context].IncrementEscape();
                toEncode = ToEncode.EncodeNothing;
            }
            
            return toEncode;
        }
    }
}