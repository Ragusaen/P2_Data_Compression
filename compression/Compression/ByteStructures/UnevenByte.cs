using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Configuration;
using PDC;

namespace Compression.ByteStructures{
    public struct UnevenByte{
        public uint Data;
        public int Length;
        
        public UnevenByte(uint data, int length) {
            Data = data;
            Length = length;
        }

        // Appends length bits from data to the uneven byte
        public void Append(uint data, int length) {
            Length += length;
            Data = (uint) ((Data << length) + (data % (1 << length)));
        }

        public byte GetBits(int count) {
            return (byte)((Data >> ((int)Length - count)) % (1 << (int)Length));
        }

        public override string ToString() {
            uint d = (uint) (Data % (1 << (int) (Length)));
            string s = Convert.ToString(d, 2);
            while (s.Length < Length)
                s = "0" + s;
            return s;
        }
    }
}