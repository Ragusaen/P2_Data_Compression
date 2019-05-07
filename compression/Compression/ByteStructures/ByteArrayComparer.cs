using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Compression {
    public class ByteArrayComparer : IComparer<byte[]> {
        public int Compare(byte[] x, byte[] y) {
            
            int length = (x.Length < y.Length)? x.Length: y.Length;
            for (int i = 0; i < length; i++) {
                int v = x[i].CompareTo(y[i]);
                if (v != 0)
                    return v;
            }
            return x.Length.CompareTo(y.Length);
        }
    }
}