using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;

namespace Compression.PPM{
    public class PPMTables{
        public List<ContextTable> _orderList = new List<ContextTable>();
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
            }
            else {
                updateResult = ct.UpdateContext(entry);
                SymbolDictionary matchedContext = ct.ContextDict[context];
                // No matched context or symbol case, encode nothing
                if(updateResult == ContextTable.ToEncode.EncodeNothing)
                    encodeInfo = new EncodeInfo(0,0,0);
            
                // Matched context and symbol case
                else if (updateResult == ContextTable.ToEncode.EncodeSymbol) 
                    encodeInfo = new EncodeInfo(matchedContext[symbol].Count - 1, matchedContext[symbol].CumulativeCount - 1, matchedContext.TotalCount - 1);
            
                // Matched context, but no symbol case
                else // (updateResult == ContextTable.ToEncode.EncodeEscape)
                    encodeInfo = new EncodeInfo(matchedContext.EscapeInfo.Count - 1, matchedContext.EscapeInfo.CumulativeCount - 2, matchedContext.TotalCount - 2);
            }
            
            return updateResult;
        }
        
        private void InitializeTables() {
            _orderList = new List<ContextTable>();
            
            for (int i = 0; i <= _maxOrder; i++) {
                _orderList.Add(new ContextTable());
            }
        }
    }
} 