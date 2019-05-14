
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

        public ExpansionType Expand() {
            if (Upper < Max / 2) {
                Lower *= 2;
                Upper *= 2;
                return ExpansionType.LEFT;
            }
            if (Lower > Max / 2) {
                Lower = (Lower - Max / 2) * 2;
                Upper = (Upper - Max / 2) * 2;
                return ExpansionType.RIGHT;
            }
            if (Lower > Max / 4 && Upper < 3 * Max / 4) {
                Lower = 2 * Lower - Max / 2;
                Upper = 2 * Upper - Max / 2;
                return ExpansionType.MIDDLE;
            }

            return ExpansionType.NONE;
        }
    }
}