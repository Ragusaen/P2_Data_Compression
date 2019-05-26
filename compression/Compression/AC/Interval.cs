using System;
namespace Compression.AC {
    public enum ExpansionType {
        LEFT,
        RIGHT,
        MIDDLE,
        NONE
    };
    
    /// <summary>
    /// This class handles integer intervals. It contains a Lower and Upper bound, as well as a max. Instead
    /// of interval of real numbers [0;1) there is an interval [0; Max). Therefore the conversion is simple,
    /// this interval is just scaled by Max and truncated to an int. The interval can be used to both narrow
    /// and expand.
    /// </summary>
    public class Interval {
        public long Lower;
        public long Upper;
        public long Max;

        public Interval(long lower, long upper, long max) {
            Lower = lower;
            Upper = upper - 1; // Subtract 1 to make it inclusive
            Max = max;
        }
        
        /// <summary>
        /// This method expands the interval and returns the type of expansion done. The expansion is
        /// done when the interval lies strictly within one of the following intervals: [0; 0,5), [0,5; 1)
        /// and [0,25; 0,75).
        /// </summary>
        /// <returns> Type of expansion that was done. </returns>
        public ExpansionType Expand() {
            if (Lower >= Max / 2) {
                Lower = 2 * Lower - Max;
                Upper = 2 * Upper - Max;
                return ExpansionType.LEFT;
            }
            if (Upper <= Max / 2) {
                Lower *= 2;
                Upper *= 2;
                return ExpansionType.RIGHT;
            }
            if (Lower >= Max / 4 && Upper <= 3 * Max / 4) {
                Lower = 2 * Lower - Max / 2;
                Upper = 2 * Upper - Max / 2;
                return ExpansionType.MIDDLE;
            }

            return ExpansionType.NONE;
        }
        
        /// <summary>
        /// Narrow the interval to within the given counts given.
        /// </summary>
        /// <param name="count"> The count of the symbol. </param>
        /// <param name="cumulativeCount"> The count of the symbol, and all of the symbols below it. </param>
        /// <param name="totalCount"> The total count of all the symbols. </param>
        /// <exception cref="ArithmeticException"> If parameters are too large, the integer arithmetic can
        /// get too large, therefore an exception is thrown if this is detected. </exception>
        public void Narrow(long prevCount, long cumCount, long totalCount) {
            long tempLower = Lower;
            Lower = Lower +  (prevCount * (Upper - Lower)) / totalCount;
            Upper = tempLower + (cumCount * (Upper - tempLower)) / totalCount - 1;
            
            if (Upper <= Lower) {
                throw new ArithmeticException("Arithmetic was not precise enough");
            }
        }

        public override string ToString() {
            return $"[{(double)Lower / Max}, {(double)Upper / Max})";
        }
    }
}