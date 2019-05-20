
using System;
using Compression.ByteStructures;

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
            Upper = upper - 1; // Subtract 1 to make it inclusive
            Max = max;
        }

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

        public void Narrow(int prevCount, int count, int totalCount) {
            int tempLower = Lower;
            Lower = Lower + (prevCount * (Upper - Lower)) / totalCount;
            Upper = tempLower + (count * (Upper - tempLower)) / totalCount - 1;
        }

        public ExpansionType ExpandBest() {
            if (Max - Lower > Upper) {
                Lower *= 2;
                Upper = Max;
                return ExpansionType.RIGHT;
            }
            else {
                Lower = 0;
                Upper = (Upper - Max / 2) * 2;
                return ExpansionType.LEFT;
            }
        }

        public override string ToString() {
            return $"[{(double)Lower / Max}, {(double)Upper / Max})";
        }
    }
}