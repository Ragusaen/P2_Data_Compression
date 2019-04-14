using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Security.Cryptography;

namespace Compression.PPM{
    public class ContextTable : IEnumerable<Context> {
        public List<Context> ContextList = new List<Context>();
        public Dictionary<Context, List<Symbol>> ContextDict = new Dictionary<Context, List<Symbol>>();
        public uint TotalCount;
        private readonly uint _defaultEscaping;
        
        
        public ContextTable(uint defaultEscaping) {
            _defaultEscaping = defaultEscaping;
        }

        public bool UpdateContext(byte[] context, byte symbol) {
            int i = ContextAlreadyExist(context);
            
            if (i >= 0) { // execute if context was found in table
                bool newSymbol = ContextList[i].Update(symbol);
                if (newSymbol)
                    return false;
                return true;
            }
            
            // execute if context was not found in table
            ContextList.Add(new Context(context));
            ContextList[ContextList.Count-1].SymbolList.Add(new Symbol()); // Adds instance of Default Escaping Symbol
            ContextList[ContextList.Count-1].SymbolList[0].Count = _defaultEscaping; // Sets count of <esc> to the desired Default Escaping
            ContextList[ContextList.Count-1].Update(symbol);
            return false;
        } 

        private int ContextAlreadyExist(byte[] currentContext) {
            ByteArrayComparer byteArrayComparer = new ByteArrayComparer();
            
            // Looks for context in order table, if found returns the index value
            for (int i = 0; i < ContextList.Count; i++) {
                if(byteArrayComparer.Compare(ContextList[i].ContextBytes, currentContext) == 0)
                    return i; 
            }
            return -1;
        }

        public void CalculateTotalCount() {
            TotalCount = 0;
            
            for (int i = 0; i < ContextList.Count; i++) {
                for (int j = 0; j < ContextList[i].SymbolList.Count; j++) {
                    TotalCount += ContextList[i].SymbolList[j].Count;
                }
            }
        }

        public void UpdateCumulativeCount() {
            uint cumCount = 0;
            
            for (int i = 0; i < ContextList.Count; i++) {
                for (int j = 0; j < ContextList[i].SymbolList.Count; j++) {
                    cumCount += ContextList[i].SymbolList[j].Count;
                    ContextList[i].SymbolList[j].CumulativeCount = cumCount;
                }
            }
        }

        public IEnumerator<Context> GetEnumerator() {
            return ContextList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}