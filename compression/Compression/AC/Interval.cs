using System.Web.Security.AntiXss;

namespace Compression.AC {
    public class Interval {
        public double high, low;

        public Interval(double low, double high) {
            this.high = high;
            this.low = low; 
        }
    }
}