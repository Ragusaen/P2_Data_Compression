using System.Collections.Generic;

namespace Compression.PPM{
    public class PPMTables{
        public List<ContextTable> _orderList = new List<ContextTable>();
        private readonly int _maxOrder;

        public PPMTables(int maxOrder = 5) {
            _maxOrder = maxOrder;
            InitializeTables();
        }

        public EncodeInfo LookUpAndUpdate(Entry entry, out ContextTable.ToEncode updateResult) {
            ContextTable ct = _orderList[entry.Context.Length + 1];
            byte symbol = entry.Symbol;
            byte[] context = entry.Context;
            
            updateResult = ct.UpdateContext(entry);
            
            // No matched context or symbol case, encode nothing
            if(updateResult == ContextTable.ToEncode.EncodeNothing)
                return new EncodeInfo(0,0,0);
            
            // Matched context and symbol case
            SymbolDictionary matchedContext = ct.ContextDict[context];
            if (updateResult == ContextTable.ToEncode.EncodeSymbol)
                return new EncodeInfo(matchedContext[symbol].Count, matchedContext[symbol].CumulativeCount, matchedContext.TotalCount);
            
            // Matched context but not symbol in 0. order
            if(updateResult == ContextTable.ToEncode.EncodeMinusFirst)
                return new EncodeInfo(-(matchedContext.EscapeInfo.Count), matchedContext.EscapeInfo.CumulativeCount, matchedContext.TotalCount );
            
            // Matched context, but no symbol case
            if (updateResult == ContextTable.ToEncode.EncodeEscape)
                return new EncodeInfo(matchedContext.EscapeInfo.Count,matchedContext.EscapeInfo.CumulativeCount, matchedContext.TotalCount );
            
            return new EncodeInfo(1, entry.Symbol, byte.MaxValue + 1);
        }

        public EncodeInfo LookUpSymbol(Entry entry) {
            byte symbol = entry.Symbol;
            byte[] context = entry.Context;
            int count = 0;
            int cumulative = 0;
            int totalCount = 0;
            Dictionary<byte[], SymbolDictionary> ct = _orderList[context.Length + 1].ContextDict;
            
            if (ct.ContainsKey(context)) {
                if (ct[context].ContainsKey(symbol)) {
                    count = ct[context][symbol].Count;
                    cumulative = ct[context][symbol].CumulativeCount;
                    totalCount = ct[context].TotalCount;
                }
                else {
                    totalCount = ct[context].TotalCount;
                }
            }

            return new EncodeInfo(count, cumulative, totalCount);
        }

        public EncodeInfo LookUpEscape(byte[] context) {
            SymbolDictionary sd = _orderList[context.Length + 1].ContextDict[context];
            
            return new EncodeInfo(sd.EscapeInfo.Count, sd.EscapeInfo.CumulativeCount, sd.TotalCount);
        }
        
        private void InitializeTables() {
            _orderList = new List<ContextTable>();
            
            for (int i = 0; i <= _maxOrder+1; i++) {
                _orderList.Add(new ContextTable());
            }
        }        
        
        private byte[] GetContextFromFile(DataFile file, int i) {
            if (_maxOrder == 0)
                return new byte[0];
            
            if(i > _maxOrder)
                return file.GetBytes(i - _maxOrder, _maxOrder);
            return file.GetBytes(0, i);
        }
    }
} 