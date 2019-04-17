using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using Compression.ByteStructures;
using Compression.RLE;

namespace Compression.PPM{
    public class SymbolList : IEquatable<SymbolList>, IList<ISymbol>{
        private readonly List<ISymbol> _symbolList = new List<ISymbol>();

        public SymbolList() { }

        public SymbolList(ISymbol[] input) {
            _symbolList = input.ToList();

            for (int i = 0; i < input.Length; i++) {
                _symbolList[i] = input[i];
            }
        }

        public SymbolList(byte[] input) {
            _symbolList = input.Select(p => new Letter(p)).Cast<ISymbol>().ToList();
        }

        public bool Equals(SymbolList other) {
            if (other == null || other.Count != _symbolList.Count)
                return false;

            for (int i = 0; i < _symbolList.Count; i++) {
                if (!_symbolList[i].Equals(other[i]))
                    return false;
            }

            return true;
        }

        public override int GetHashCode() {
            return _symbolList.ToString().GetHashCode();
        }

        public override string ToString() {
            string res = "";

            foreach (var t in _symbolList) {
                res += t.ToString();
            }

            return res;
        }

        public IEnumerator<ISymbol> GetEnumerator() {
            return _symbolList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public void Add(ISymbol item) {
            _symbolList.Add(item);
        }

        public void Clear() {
            _symbolList.Clear();
        }

        public bool Contains(ISymbol item) {
            return _symbolList.Contains(item);
        }

        public void CopyTo(ISymbol[] array, int arrayIndex) {
            _symbolList.CopyTo(array, arrayIndex);
        }

        public bool Remove(ISymbol item) {
            return _symbolList.Remove(item);
        }

        public int Count => _symbolList.Count;

        public bool IsReadOnly => false;

        public int IndexOf(ISymbol item) {
            return _symbolList.IndexOf(item);
        }

        public void Insert(int index, ISymbol item) {
            _symbolList.Insert(index, item);
        }

        public void RemoveAt(int index) {
            _symbolList.RemoveAt(index);
        }

        public ISymbol this[int index] {
            get => _symbolList[index];
            set => _symbolList[index] = value;
        }
    }
}