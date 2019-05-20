using System.Linq;

namespace Compression.PPM{
    public class Entry{
        public byte Symbol;
        public byte[] Context;
        
        public Entry() { }

        public Entry(byte symbol, byte[] context) {
            Symbol = symbol;
            Context = context;
        }
        
        public void NextContext() {
            if (Context.Length <= 1) {
                Context = new byte[0];
                return;
            }

            int newLength = Context.Length - 1;
            byte[] res = new byte[newLength];
            
            for (int i = 0; i < newLength; i++) {
                res[i] = Context[i+1];
            }

            Context = res;
        }

        public override string ToString() {
            return "Symbol: " + (char)Symbol + "  Context: " + new string(Context.Select(p => (char)p).ToArray());
        }
    }
}