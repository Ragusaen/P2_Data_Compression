using System;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using PDC;

namespace Compression.PPM{
    public class Entry{
        public byte Symbol;
        public byte[] Context;
        public bool IsMinusFirstOrder = false;
        
        public Entry() { }

        public Entry(byte symbol, byte[] context) {
            Symbol = symbol;
            Context = context;
        }
        
        public void NextContext() {
            if (Context.Length == 0) {
                IsMinusFirstOrder = true;
                return;
            }
            if (Context.Length <= 1) {
                Context = new byte[0];
                return;
            }
            
            byte[] res = new byte[Context.Length - 1];
            Array.Copy(Context, 1, res, 0, res.Length);

            Context = res;
        }

        public override string ToString() {
            return "Symbol: " + (char)Symbol + "  Context: " + new string(Context.Select(p => (char)p).ToArray()) + " -1?: " + IsMinusFirstOrder;
        }
    }
}