using System.Web.Security.AntiXss;
using Compression.ByteStructures;

namespace Compression.AC {
    public class Interval {
        public UnevenByte high;
        public UnevenByte low;

        public Interval(UnevenByte low, UnevenByte high) {
            this.high = high;
            this.low = low; 
        }
    }
}