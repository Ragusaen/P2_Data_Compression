using System.Net.Configuration;

namespace Compression.PPM{
    public abstract class SymbolData{}

    public class Letter : SymbolData{
        public byte Data;

        public Letter(byte data) {
            Data = data;
        }
    }

    public class EscapeSymbol : SymbolData{}
}