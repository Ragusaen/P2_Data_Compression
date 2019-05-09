using System;

namespace Compression.PPM{
    public interface ISymbol : IEquatable<ISymbol>{
        
    }

    public struct Letter : ISymbol{
        public readonly byte Data;

        public Letter(byte data) {
            Data = data;
        }

        public bool Equals(ISymbol other) {
            if (other is Letter l)
                return Data == l.Data;
            return false;
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Letter other && Equals(other);
        }

        public override int GetHashCode() {
            return Data.GetHashCode();
        }

        public override string ToString() {
            return ((char)Data).ToString();
        }
    }

    public struct EscapeSymbol : ISymbol {
        public bool Equals(ISymbol other) {
            return other.GetType() == typeof(EscapeSymbol);
        }

        public override bool Equals(object obj) {
            if (obj == null)
                return false;
            return obj.GetType() == typeof(EscapeSymbol);
        }

        public override int GetHashCode() {
            return 0;
        }

        public override string ToString() {
            return "<esc>";
        }
    }
}