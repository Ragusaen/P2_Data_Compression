using System;
using System.Collections.Generic;

namespace Compression.PPM{
    public class PPMTables{
        private List<ContextTable> _orderList = new List<ContextTable>();
        private readonly int _maxOrder;

        public PPMTables(int maxOrder = 5) {
            _maxOrder = maxOrder;
            InitializeTables();
        }

        public ContextTable.ToEncode LookUpAndUpdate(Entry entry, out EncodeInfo encodeInfo) {
            ContextTable ct = _orderList[entry.Context.Length];
            byte symbol = entry.Symbol;
            byte[] context = entry.Context;
            ContextTable.ToEncode updateResult;
            
            // If it is the minus first order, just encode the symbol
            if (entry.IsMinusFirstOrder) {
                encodeInfo = new EncodeInfo(1, entry.Symbol + 1, byte.MaxValue + 1);
                updateResult = ContextTable.ToEncode.EncodeSymbol;
            } else {
                updateResult = ct.UpdateContext(entry);
                SymbolDictionary matchedContext = ct[context];
                // No matched context or symbol case, encode nothing
                if(updateResult == ContextTable.ToEncode.EncodeNothing)
                    encodeInfo = default(EncodeInfo);
            
                // Matched context and symbol case
                else if (updateResult == ContextTable.ToEncode.EncodeSymbol) 
                    encodeInfo = new EncodeInfo(matchedContext[symbol].Count - 1, matchedContext[symbol].CumulativeCount - 1, matchedContext.TotalCount - 1);
            
                // Matched context, but no symbol case
                else // (updateResult == ContextTable.ToEncode.EncodeEscape)
                    encodeInfo = new EncodeInfo(matchedContext.EscapeInfo.Count - 1, matchedContext.EscapeInfo.CumulativeCount - 2, matchedContext.TotalCount - 2);
            }
            
            return updateResult;
        }
        
        /// <summary>
        /// This method removes all of the large contexts' data (3. - 5.) to reduce the memory usage.
        /// It also reduces the counts of the symbols in the 1. and 0. order, to prevent too specific probabilities.
        /// If total count get too large, smaller counts may not fit within the integer interval in arithmetic coding.
        /// </summary>
        public void CleanUp() {
            
            // Reduce counts of symbols in 1. and 0. order contexts
            for (int i = 0; i <= 1; ++i) {
                foreach (var context in _orderList[i].Values) {
                    int cumCount = 0;
                    foreach (var entry in context) {
                        entry.Value.Count /= 128; // Reduce symbol count
                        if (entry.Value.Count == 0)
                            entry.Value.Count = 1;
                        
                        cumCount += entry.Value.Count;
                        entry.Value.CumulativeCount = cumCount;
                    }
                    
                    // Update escape cumcount and totalcount
                    cumCount += context.EscapeInfo.Count;
                    context.EscapeInfo.CumulativeCount = cumCount;
                    context.TotalCount = cumCount;
                }
            }
            
            for (int i = 2; i < _orderList.Count; ++i) {
                _orderList[i] = new ContextTable();
            }
        }
        
        private void InitializeTables() {
            _orderList = new List<ContextTable>();
            
            for (int i = 0; i <= _maxOrder; i++) {
                _orderList.Add(new ContextTable());
            }
        }
    }
} 