using System;
using System.Collections.Generic;

namespace Compression {
    
    /// <summary>
    /// This class is used to compare arrays of bytes, and therefore implements both the IComparer and
    /// IEqualityComparer interfaces.
    /// </summary>
    public class ByteArrayComparer : IComparer<byte[]>, IEqualityComparer<byte[]> {
        /// <summary>
        /// Simply compare each element in the array to check if they are equal.
        /// </summary>
        /// <param name="x"> The first array. </param>
        /// <param name="y"> The second array. </param>
        /// <returns> 1 if first arrays is larger on first not matching byte, 0 if they are equal
        /// and -1 if second array is larger </returns>
        public int Compare(byte[] x, byte[] y) {
            int length = (x.Length < y.Length)? x.Length: y.Length;
            for (int i = 0; i < length; i++) {
                int v = x[i].CompareTo(y[i]);
                if (v != 0)
                    return v;
            }
            return x.Length.CompareTo(y.Length);
        }
        
        public bool Equals(byte[] x, byte[] y) {
            if ( x == null || y == null ) {
                return x == y;
            }
            if ( x.Length != y.Length ) {
                return false;
            }
            for ( int i= 0; i < y.Length; i++) {
                if ( x[i] != y[i] ) {
                    return false;
                }
            }
            
            return true;
        }

        public int GetHashCode(byte[] key) {
            if (key == null)
                throw new ArgumentNullException("key");
            int sum = 0;
            foreach ( byte cur in key ) {
                sum += cur;
            }
            return sum;
        }
    }
}