namespace Compression.PPM{
    public class Entry{
        public byte[] Context;
        public byte Letter;

        public Entry(byte[] context, byte letter) {
            Context = context;
            Letter = letter;
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
    }
}