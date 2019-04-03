using System.Collections.Generic;

namespace Compression.PPM{
    public class Context{
        public byte[] ContextBytes;
        public List<Symbol> SymbolList = new List<Symbol>();

        public Context(byte[] contextBytes, byte firstLetter) {
            ContextBytes = contextBytes;
            SymbolList.Add(new Symbol(firstLetter));
        }

        public void Update(byte a) {
            for(int i = 0; i < SymbolList.Count; i++) {
                if (SymbolList[i].Letter == a) {
                    SymbolList[i].Count++;
                    return;
                }
            }
            SymbolList.Add(new Symbol(a));
        }
    }
}