using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Security.Cryptography;

namespace Compression.PPM{
    public class ContextTable{
        public List<Context> ContextList; 
        public int TotalCount { get; set; }

        public void UpdateContext(byte[] context, byte symbol) {
            int i = ContextAlreadyExist(context);
            
            if (i >= 0) { // execute if context was found in table
                ContextList[i].Update(symbol);
            }
            else { // execute if context was not found in table
                ContextList.Add(new Context(context, symbol));
            }
        }

        private int ContextAlreadyExist(byte[] currentContext) {
            ByteArrayComparer byteArrayComparer = new ByteArrayComparer();
            
            for (int i = 0; i < ContextList.Count; i++) {
                if(byteArrayComparer.Compare(ContextList[i].ContextBytes, currentContext) == 0)
                    return i; 
            }
            return -1;
        }
    }
}