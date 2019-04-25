using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace Compression.PPM{
    public class PredictionByPartialMatching : ICompressor{
        private readonly uint _maxOrder;
        private readonly uint _defaultEscaping;
        public List<ContextTable> OrderList = new List<ContextTable>();

        public PredictionByPartialMatching(uint maxOrder = 5, uint defaultEscaping = 0) {
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
            
            for (uint i = 0; i < file.Length; i++) {
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
        
        private byte[] GetContextFromFile(DataFile file, uint i) {
            if (_maxOrder == 0)
                return new byte[0];
            
            if(i > _maxOrder)
                return file.GetBytes(i - _maxOrder, _maxOrder);
            return file.GetBytes(0, i);
        }

        private void CreateMinusFirstOrder() {
            OrderList[0].ContextDict.Add(new SymbolList(), new SymbolDictionary());
            ISymbol[] zeroOrderSymbols = OrderList[1].ContextDict[new SymbolList()].Keys.ToArray();
            foreach(var u in zeroOrderSymbols)
                Console.WriteLine(u);
            int len = zeroOrderSymbols.Length;
            
            SymbolList empty = new SymbolList();
            
            for (int i = 0; i < len; i++) {
                if(zeroOrderSymbols[i] is Letter)
                    OrderList[0].ContextDict[empty].Add(zeroOrderSymbols[i], new SymbolInfo());
            }
        }

        public void EscapeToEnd() {
            KeyValuePair<SymbolList, SymbolInfo> keyValuePair = new KeyValuePair<SymbolList, SymbolInfo>();
            EscapeSymbol esc = new EscapeSymbol();
            SymbolList escContext = new SymbolList {esc};
            keyValuePair.Key.Add(esc);
            int count = 0;
            
            
            foreach (var t in OrderList) {
                foreach (var context in t.ContextDict) {
                    count = (int)context.Value[esc].Count;
                }
            }
        }
    }
}