using System;
using System.Transactions;
using Compression.ByteStructures;

namespace Compression.AC {
    /// <summary>
    /// This class implement arithmetic encoding. When created a BitString is instantiated, and you then encode
    /// symbols one by one, by specifying a count, cumululative count and a total count. The intervals will be
    /// narrow upon each encoding. When done encoding call the finalize method to find that last bits.
    /// </summary>
    public class ArithmeticCoder {
        private const long MAX_INTERVAL = (1 << 20) - 1;
        private BitString _bitString = new BitString();
        
        private int _followBits = 0;
        private Interval _interval = new Interval(0, MAX_INTERVAL, MAX_INTERVAL);
        
        /// <summary>
        /// This method encodes a single symbol by narrowing the interval.
        /// </summary>
        /// <param name="count"> The count of the symbol. </param>
        /// <param name="cumulativeCount"> The count of the symbol, and all of the symbols below it. </param>
        /// <param name="totalCount"> The total count of all the symbols. </param>
        public void Encode(int count, int cumulativeCount, int totalCount) {
            // Calculate cumulative count of previous symbol
            var prevCount = cumulativeCount - count;
            
            // Narrow the interval
            _interval.Narrow(prevCount, cumulativeCount, totalCount);
            
            // Expand the interval as long as possible.
            ExpansionType et;
            while ((et = _interval.Expand()) != ExpansionType.NONE) {
                // If middle expansion, increment follow bits, to account for intervals around the middle
                if (et == ExpansionType.MIDDLE) {
                    ++_followBits;
                }
                else {
                    // Encode 0 or 1 depending on left or right expansion
                    UnevenByte toEncode;
                    if (et == ExpansionType.LEFT) {
                        toEncode = UnevenByte.One;
                    }
                    else { // et == ExpansionType.Right
                        toEncode = UnevenByte.Zero;
                    }
                    _bitString.Append(toEncode);
                    
                    // If there are any follow bits, encode the complement of the bit follow bit times
                    for (;_followBits > 0; --_followBits) {
                        _bitString.Append(!toEncode); // Encode follow bits as the complement
                    }
                }
            }
        }
        
        /// <summary>
        /// This method finalizes the interval by finding the bits that make the interval within the interval
        /// of the last symbol encoded.
        /// </summary>
        public void FinalizeInterval() {
            // Chooses the bit which has the most of the interval within it.
            UnevenByte toEncode = _interval.Upper > _interval.Max - _interval.Lower ? UnevenByte.One : UnevenByte.Zero;
            _bitString.Append(toEncode);
            
            // Encode the complement follow bits times
            for (; _followBits > 0; --_followBits) {
                _bitString.Append(!toEncode);
            }
            _bitString.Append(!toEncode); // Encode a final complement
        }
        
        /// <summary>
        /// Get the BitString that contains the encoding.
        /// </summary>
        /// <returns> BitString that contains the encoding. </returns>
        public BitString GetEncodedBitString() {
            return _bitString;
        }
    }
}