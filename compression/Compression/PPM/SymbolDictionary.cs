using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Compression.PPM{
    public class SymbolDictionary : Dictionary<ISymbol, SymbolInfo>{
        public SymbolDictionary() { }

        public SymbolDictionary(ISymbol symbol, int symbolCount = 0) {
            Add(symbol, new SymbolInfo(count: symbolCount));
        }

        public void AddNew(ISymbol symbol) {
            Add(symbol, new SymbolInfo());
        }
    }
}