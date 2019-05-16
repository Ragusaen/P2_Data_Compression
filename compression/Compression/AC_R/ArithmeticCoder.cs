using System;
using System.Collections.Generic;
using System.IO;
using Compression.ByteStructures;
using Compression.PPM;

namespace compression.AC_R {
    public class ArithmeticCoder {
        private const int MAX_INTERVAL = 1 << 16;
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

            Console.WriteLine("Encoding -> Count: {0}, CC: {1}, TC: {2}", count, cumulativeCount, totalCount);
            
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
                    while (_followBits > 0) {
                        _bitString.Append(!toEncode); // Encode follow bits as the complement
                        --_followBits;
                    }
                }
            }
        }

        public BitString GetEncodedBitString() {
            _bitString.Append(UnevenByte.Zero);
            return _bitString;
        }
    }
}