using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using Compression.ByteStructures;

namespace Compression.PPM{
    public class PredictionByPartialMatching : ICompressor{
        private DataFile file;
        private int _maxOrder;
        private uint _defaultEscaping;
        public List<ContextTable> OrderList { get; private set; }

        public PredictionByPartialMatching(DataFile file, int maxOrder, uint defaultEscaping = 0) {
            _maxOrder = maxOrder;
            _defaultEscaping = defaultEscaping;
            this.file = file;
            InitializeTables();
            FillTables();
        }
        
        public DataFile Compress(DataFile to_compress) {
            throw new System.NotImplementedException();
        }

        public DataFile Decompress(DataFile to_decompress) {
            throw new System.NotImplementedException();
        }

        private void InitializeTables() {
            OrderList = new List<ContextTable>();
            
            for (int i = 0; i <= _maxOrder+1; i++) {
                OrderList.Add(new ContextTable(_defaultEscaping));
            }
        }

        private void FillTables() {
            if (file.Length == 0) // return if file is empty
                return;
            
            for (uint i = 0; i < file.Length; i++) {
                AddEntryToTable(new Entry(GetContextFromFile(i, _maxOrder), file.GetByte(i)));
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
        
        private byte[] GetContextFromFile(uint i, int currentOrder) {
            if (currentOrder > 0) {
                if (i >= _maxOrder)
                    return file.GetBytes(i - (uint) currentOrder, (uint) currentOrder);
                return file.GetBytes(0, i);
            }
            return new byte[0];
        }

        private void CreateMinusFirstOrder() {
            OrderList[0] = new ContextTable(0);
            OrderList[0].ContextList.Add(new Context(new byte[0]));
            
            for (var i = 1; i < OrderList[1].ContextList[0].SymbolList.Count; i++) {
                OrderList[0].ContextList[0].SymbolList.Add(new Symbol(((Letter) OrderList[1].ContextList[0].SymbolList[i].Data).Data));
            }
        }
    }
}