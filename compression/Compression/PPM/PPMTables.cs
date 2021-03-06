using System.Collections.Generic;

namespace Compression.PPM {
    public class PPMTables : List<ContextTable> {
        private readonly int _maxOrder;

        public PPMTables(int maxOrder = 5) {
            _maxOrder = maxOrder;
            InitializeTables();
        }

        public ContextTable.ToEncode LookUpAndUpdate(Entry entry, out EncodeInfo encodeInfo) {
            var ct = this[entry.Context.Length];
            var symbol = entry.Symbol;
            var context = entry.Context;
            ContextTable.ToEncode updateResult;

            // If it is the minus first order, just encode the symbol
            if (entry.IsMinusFirstOrder) {
                encodeInfo = new EncodeInfo(1, entry.Symbol + 1, byte.MaxValue + 1);
                updateResult = ContextTable.ToEncode.EncodeSymbol;
            }
            else {
                updateResult = ct.UpdateContext(entry);
                var matchedContext = ct[context];
                // No matched context or symbol case, encode nothing
                if (updateResult == ContextTable.ToEncode.EncodeNothing)
                    encodeInfo = default(EncodeInfo);

                // Matched context and symbol case
                else if (updateResult == ContextTable.ToEncode.EncodeSymbol)
                    encodeInfo = new EncodeInfo(matchedContext[symbol].Count - 1,
                        matchedContext[symbol].CumulativeCount - 1, matchedContext.TotalCount - 1);

                // Matched context, but no symbol case
                else // (updateResult == ContextTable.ToEncode.EncodeEscape)
                    encodeInfo = new EncodeInfo(matchedContext.EscapeInfo.Count - 1,
                        matchedContext.EscapeInfo.CumulativeCount - 2, matchedContext.TotalCount - 2);
            }

            return updateResult;
        }

        /// <summary>
        ///     This method removes all of the large contexts' data (3. - 5.) to reduce the memory usage.
        ///     It also reduces the counts of the symbols in the 1. and 0. order, to prevent too specific probabilities.
        ///     If total count get too large, smaller counts may not fit within the integer interval in arithmetic coding.
        /// </summary>
        public void CleanUp() {
            // Reduce counts of symbols in 1. and 0. order contexts
            for (var i = 0; i <= 1; ++i)
                foreach (var context in this[i].Values) {
                    var cumCount = 0;

                    foreach (var symbol in context.Values) {
                        symbol.Count /= 8; // Reduce symbol count
                        if (symbol.Count == 0)
                            symbol.Count = 1;

                        cumCount += symbol.Count;
                        symbol.CumulativeCount = cumCount;
                    }

                    // Update escape cumcount and totalcount
                    cumCount += context.EscapeInfo.Count;
                    context.EscapeInfo.CumulativeCount = cumCount;
                    context.TotalCount = cumCount;
                }

            for (var i = 2; i < Count; ++i) this[i] = new ContextTable();
        }
        
        private void InitializeTables() {
            for (var i = 0; i <= _maxOrder; i++) Add(new ContextTable());
        }
    }
}