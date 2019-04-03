using System.Collections.Generic;

namespace Compression.PPM{
    public class ContextTableContent{
        public byte[] Context;
        public List<Symbol> SymbolList;

        public ContextTableContent(byte[] context, Symbol firstSymbol) {
            Context = context;
            SymbolList.Add(firstSymbol);
        }
        
        public void NewSymbol(Symbol newSymbol) {
            SymbolList.Add(newSymbol);
        }
    }
}