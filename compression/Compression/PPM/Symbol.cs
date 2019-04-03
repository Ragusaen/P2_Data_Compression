namespace Compression.PPM{
    public class Symbol{
        public byte Content;
        public int Count;
        public int CumulativeCount;

        public Symbol(byte content) {
            Content = content;
            Count = 1;
            CumulativeCount = 1;
        }
    }
}