namespace Compression.PPM{
    public class Symbol {
        public SymbolData Data;
        public int Count;
        public int CumulativeCount;

        public Symbol(byte letter) {
            Data = new Letter(letter);
            Count = 1;
        }

        public Symbol() {
            Data = new EscapeSymbol();
            Count = 1;
        }
    }
}