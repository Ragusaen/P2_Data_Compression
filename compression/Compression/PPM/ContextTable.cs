using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Security.Cryptography;

namespace Compression.PPM{
    public class ContextTable{
        public List<Context> ContextList = new List<Context>();
        public uint TotalCount;

        public void UpdateContext(byte[] context, byte symbol) {
            int i = ContextAlreadyExist(context);
            
            if (i >= 0) { // execute if context was found in table
                ContextList[i].Update(symbol);
            }
            else { // execute if context was not found in table
                ContextList.Add(new Context(context));
                ContextList[0].Update(symbol);
            }
        }

        private int ContextAlreadyExist(byte[] currentContext) {
            ByteArrayComparer byteArrayComparer = new ByteArrayComparer();
            
            // Looks for context in order table, if found returns the index value
            for (int i = 0; i < ContextList.Count; i++) {
                if(byteArrayComparer.Compare(ContextList[i].ContextBytes, currentContext) == 0)
                    return i; 
            }
            return -1;
        }

        public void CalculateTotalCount() {
            TotalCount = 0;
            
            for (int i = 0; i < ContextList.Count; i++) {
                for (int j = 0; j < ContextList[i].SymbolList.Count; j++) {
                    TotalCount += ContextList[i].SymbolList[j].Count;
                }
            }
        }
    }
}