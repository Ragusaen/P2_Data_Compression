using Compression.ByteStructures;

namespace compression.AC {
    public class BinaryFractionInIntervalFinder {
        public UnevenByte GetBinaryFraction(UnevenByte lower, UnevenByte upper) {
            UnevenByte ub = new UnevenByte();

            while (!(FractionCompare(ub + UnevenByte.OneOne, upper) < 0 &&
                     FractionCompare(ub, lower) >= 0)) {
                
            }

            return default(UnevenByte);
        }

        public UnevenByte FractionToUnevenByte(double d) {
            UnevenByte ub = new UnevenByte();
            double accountedFor = 0f;
            
            for (int i = 1; i < 10 && accountedFor != d; ++i) {
                double addition = (double)1 / (1 << i);
                if (accountedFor + addition <= d) {
                    accountedFor += addition;
                    ub += UnevenByte.One;
                }
                else {
                    ub += UnevenByte.Zero;
                }
            }

            return ub;
        }

        public int FractionCompare(UnevenByte a, UnevenByte b) {
            for (int ai = a.Length, bi = b.Length; ai >= 0 && bi >= 0; --ai, --bi) {
                int dif = a[0] - b[0];
                if (dif != 0)
                    return dif;
            }

            return 0;
        }
    }
}