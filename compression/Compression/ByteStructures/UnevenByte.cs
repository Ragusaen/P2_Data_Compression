using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web.Configuration;
using PDC;

namespace Compression.ByteStructures{
    public struct UnevenByte {
        public readonly uint Data;
        public readonly int Length;

        public UnevenByte(uint data, int length) {
            Data = data;
            Length = length;
        }

        public int this[int index] => ((int) Data >> (Length - index - 1)) % 2;

        public static UnevenByte operator +(UnevenByte a, UnevenByte b) {
            return new UnevenByte((a.Data << b.Length) + b.Data, a.Length + b.Length);
        }

        public static UnevenByte operator -(UnevenByte ub, int l) {
            return new UnevenByte((uint) (ub.Data % (1 << (ub.Length - l))), ub.Length - l);
        }

        public static readonly UnevenByte One = new UnevenByte(1,1);
        public static readonly UnevenByte OneOne = new UnevenByte(0b11,2);
        public static readonly UnevenByte Zero = new UnevenByte(0,1);


        public byte GetBits(int count) {
            return (byte)((Data >> (Length - count)) % (1 << Length));
        }

        public override string ToString() {
            uint d = (uint) (Data % (1 << (Length)));
            string s = Convert.ToString(d, 2);
            while (s.Length < Length)
                s = "0" + s;
            return s;
        }
    }
}