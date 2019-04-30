namespace Compression.PPM{
    public class SymbolInfo{
        public int Count;
        public int CumulativeCount;

        public SymbolInfo(int count = 1, int cumulative = 0) {
            Count = count;
            CumulativeCount = cumulative;
        }
    }
}