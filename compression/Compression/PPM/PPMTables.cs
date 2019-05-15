using System.Collections.Generic;
using System.Linq;

namespace Compression.PPM{
    public class PPMTables{
        private List<ContextTable> _orderList = new List<ContextTable>();
        private readonly int _maxOrder;
        private readonly int _defaultEscaping;

        public PPMTables(int maxOrder = 5, int defaultEscaping = 0) {
            _maxOrder = maxOrder;
            _defaultEscaping = defaultEscaping;
            InitializeTables();
        }

        public void FillTables(DataFile file) {
            if (file.Length == 0) // return if file is empty
                return;
            
            for (int i = 0; i < file.Length; i++) {
                AddEntryToTable(new Entry(GetContextFromFile(file, i), file.GetByte(i)));
            }
            CreateMinusFirstOrder();
        }

        public SymbolInfo LookUp(byte[] context, byte symbol) {
            return _orderList[context.Length + 1].ContextDict[context][symbol];
        }

        public SymbolInfo LookUpMinusFirstOrder(byte symbol) {
            return _orderList[0].ContextDict[new byte[0]][symbol];
        }
        
        private void InitializeTables() {
            _orderList = new List<ContextTable>();
            
            for (int i = 0; i <= _maxOrder+1; i++) {
                _orderList.Add(new ContextTable(_defaultEscaping));
            }
        }        
        
        private void AddEntryToTable(Entry entry) {
            for (int i = entry.Context.Length + 1; i >= 1; i--) { // Stops when it has added to 0. order
                if (_orderList[i].UpdateContext(entry.Context, entry.Letter))
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
            byte[] empty = new byte[0];
            
            _orderList[0].ContextDict.Add(empty, new SymbolDictionary());
            byte[] zeroOrderSymbols = _orderList[1].ContextDict[new byte[0]].Keys.ToArray();
            int len = zeroOrderSymbols.Length;
            
            for (int i = 0; i < len; i++) {
                _orderList[0].ContextDict[empty].Add(zeroOrderSymbols[i], new SymbolInfo());
            }
        }
    }
}