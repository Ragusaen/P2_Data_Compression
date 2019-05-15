
using System;

namespace compression.AC_R {

    public enum ExpansionType {
        LEFT,
        RIGHT,
        MIDDLE,
        NONE
    };
    
    public class Interval {
        public int Lower;
        public int Upper;
        public int Max;

        public Interval(int lower, int upper, int max) {
            Lower = lower;
            Upper = upper;
            Max = max - 1; // Makes max inclusive
        }

        public ExpansionType Expand() {
            if (Lower >= Max / 2) {
                Lower = (Lower - Max / 2) * 2;
                Upper = (Upper - Max / 2) * 2;
                return ExpansionType.LEFT;
            }
            if (Upper < Max / 2) {
                Lower *= 2;
                Upper *= 2;
                return ExpansionType.RIGHT;
            }
            if (Lower > Max / 4 && Upper < 3 * Max / 4) {
                Lower = 2 * Lower - Max / 2;
                Upper = 2 * Upper - Max / 2;
                return ExpansionType.MIDDLE;
            }

            return ExpansionType.NONE;
        }

        public void Narrow(int prevCount, int count, int totalCount) {
            int tempLower = Lower;
            Lower = Lower + (prevCount * (Upper - Lower)) / totalCount;
            Upper = tempLower + (count * (Upper - tempLower)) / totalCount;
        }


        public override string ToString() {
            return $"[{(double)Lower / Max}, {(double)Upper / Max})";
        }
    }
}