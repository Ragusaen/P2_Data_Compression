using System;
using System.Transactions;
using Compression.ByteStructures;

namespace compression.AC_R {
    public class ArithmeticCoder {
        private const long MAX_INTERVAL = (1 << 20) - 1;
        private BitString _bitString = new BitString();
        
        private int _followBits = 0;
        private Interval _interval = new Interval(0, MAX_INTERVAL, MAX_INTERVAL);

        public void Encode(int count, int cumulativeCount, int totalCount) {
            var prevCount = cumulativeCount - count;
            _interval.Narrow(prevCount, cumulativeCount, totalCount);
            
            if(count == 0)
                throw new ArgumentException("Arithmetic encoder: Count is zero");
            if(cumulativeCount == 0)
                throw new ArgumentException("Arithmetic encoder: Cumulative count is zero");
            if(totalCount == 0)
                throw new ArgumentException("Arithmetic encoder: Total count is zero");

            //Console.WriteLine("Encoding -> Count: {0}, CC: {1}, TC: {2}", count, cumulativeCount, totalCount);
            
            ExpansionType et;
            while ((et = _interval.Expand()) != ExpansionType.NONE) {
                if (et == ExpansionType.MIDDLE) {
//                    Console.WriteLine("Expand Middle");
                    ++_followBits;
                }
                else {
                    UnevenByte toEncode;
                    if (et == ExpansionType.LEFT) {
                        //Console.WriteLine("Expand Left");
                        toEncode = UnevenByte.One;
                    }
                    else { // et == ExpansionType.Right
//                        Console.WriteLine("Expand Right");
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
//            Console.WriteLine($"Final interval: {_interval}");
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