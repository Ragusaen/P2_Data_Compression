namespace Compression.PPM{
    public class SymbolInfo{
        public uint Count;
        public uint CumulativeCount;

        public SymbolInfo(uint count = 1, uint cumulative = 0) {
            Count = count;
            CumulativeCount = cumulative;
        }
    }
}