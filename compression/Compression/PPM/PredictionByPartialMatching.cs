using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace Compression.PPM{
    public class PredictionByPartialMatching : ICompressor{
        private readonly int _maxOrder;
        private readonly int _defaultEscaping;
        public List<ContextTable> OrderList = new List<ContextTable>();

        public PredictionByPartialMatching(int maxOrder = 5, int defaultEscaping = 0) {
            _maxOrder = maxOrder;
            _defaultEscaping = defaultEscaping;
        }
        
        public DataFile Compress(DataFile toCompress) {
            InitializeTables();
            FillTables(toCompress);
            return toCompress;
        }

        public DataFile Decompress(DataFile toDecompress) {
            throw new System.NotImplementedException();
        }

        private void InitializeTables() {
            OrderList = new List<ContextTable>();
            
            for (int i = 0; i <= _maxOrder+1; i++) {
                OrderList.Add(new ContextTable(_defaultEscaping));
            }
        }

        private void FillTables(DataFile file) {
            if (file.Length == 0) // return if file is empty
                return;
            
            for (int i = 0; i < file.Length; i++) {
                AddEntryToTable(new Entry(GetContextFromFile(file, i), file.GetByte(i)));
            }
            CreateMinusFirstOrder();
        }

        private void AddEntryToTable(Entry entry) {
            for (int i = entry.Context.Length + 1; i >= 1; i--) { // Stops when it has added to 0. order
                if (OrderList[i].UpdateContext(entry.Context, entry.Letter))
                    return; // Done if a match was found in one of the tables
                entry.NextContext();
            }
        }
        
        private byte[] GetContextFromFile(DataFile file, int i) {
            if (_maxOrder == 0)
                return new byte[0];
            
            if(i > _maxOrder)
                return file.GetBytes(i - _maxOrder, _maxOrder);
            return file.GetBytes(0, i);
        }

        private void CreateMinusFirstOrder() {
            SymbolList empty = new SymbolList();
            OrderList[0].ContextDict.Add(empty, new SymbolDictionary());
            ISymbol[] zeroOrderSymbols = OrderList[1].ContextDict[new SymbolList()].Keys.ToArray();
            int len = zeroOrderSymbols.Length;
            
            for (int i = 0; i < len; i++) {
                if(zeroOrderSymbols[i] is Letter)
                    OrderList[0].ContextDict[empty].Add(zeroOrderSymbols[i], new SymbolInfo());
            }
        }
    }
}
    