using System;
using System.Collections.Generic;
using Compression.ByteStructures;
using Compression.PPM;

namespace compression.AC_R {
    public class ArithmeticCoder {
        private const int MAX_INTERVAL = 1 << 16;
        private BitString _bitString = new BitString();
        
        private int _followBits = 0;
        private Interval _interval = new Interval(0, MAX_INTERVAL, MAX_INTERVAL);

        public void Encode(SymbolInfo symbolInfo, int contextTotalCount) {
            var prevCount = symbolInfo.CumulativeCount - symbolInfo.Count;
            _interval.Narrow(prevCount, symbolInfo.CumulativeCount, contextTotalCount);
            Console.Write($"Interval: {_interval} ");

            ExpansionType et;
            while ((et = _interval.Expand()) != ExpansionType.NONE) {
                if (et == ExpansionType.MIDDLE) {
                    Console.WriteLine("Expand Middle");
                    ++_followBits;
                }
                else {
                    UnevenByte toEncode;
                    if (et == ExpansionType.LEFT) {
                        Console.WriteLine("Expand Left");
                        toEncode = UnevenByte.One;
                    }
                    else { // et == ExpansionType.Right
                        Console.WriteLine("Expand Right");
                        toEncode = UnevenByte.Zero;
                    }

                    _bitString.Append(toEncode);
                    for (;_followBits > 0; --_followBits) {
                        _bitString.Append(!toEncode); // Encode follow bits as the complement
                    }
                }
            }
        }

        public void Finalize() {
            Console.WriteLine($"Final interval: {_interval}");
            UnevenByte toEncode = _interval.Upper > _interval.Max - _interval.Lower ? UnevenByte.One : UnevenByte.Zero;
            
            _bitString.Append(toEncode);
            for (; _followBits > 0; --_followBits) {
                _bitString.Append(!toEncode);
            }
            _bitString.Append(!toEncode);
        }

        public BitString GetEncodedBitString() {
            return _bitString;
        }

        public byte[] GetBytes() {
            return _bitString.ToArray();
        }
    }
}