namespace Compression.PPM{
    public struct EncodeInfo{
        public int Count;
        public int CumulativeCount;
        public int TotalCount;

        public EncodeInfo(int count, int cumulative, int totalCount) {
            Count = count;
            CumulativeCount = cumulative;
            TotalCount = totalCount;
        }

        public override string ToString() {
            return "Count: " + Count.ToString() + "  CumCount: " + CumulativeCount.ToString() + "  TotalCount: " + TotalCount.ToString();
        }
    }
}