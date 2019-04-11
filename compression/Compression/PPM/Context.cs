using System.Collections;
using System.Collections.Generic;

namespace Compression.PPM{
    public class Context : IEnumerable<Symbol> {
        public byte[] ContextBytes;
        public List<Symbol> SymbolList = new List<Symbol>();
        
        public Context(byte[] contextBytes) {
            ContextBytes = contextBytes;
        }
        
        public bool Update(byte a) { // Returns true if a is a new symbol in the given context
            for(int i = 0; i < SymbolList.Count; i++) {
                if (SymbolList[i].Data is Letter && ((Letter) SymbolList[i].Data).Data == a) {
                    SymbolList[i].Count++;
                    return false;
                }
            }
            SymbolList[0].Count++; // Increments EscapeSymbol if a new symbol is added to that context
            SymbolList.Add(new Symbol(a));
            return true;
        }

        public IEnumerator<Symbol> GetEnumerator() {
            return SymbolList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}