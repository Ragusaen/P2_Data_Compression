namespace Compression.PPM{
    public class Symbol {
        public byte Letter;
        public uint Count;
        public uint CumulativeCount;

        public Symbol(byte letter) {
            Letter = letter;
            Count = 1;
        }
    }
}