using System.Collections.Generic;
using Compression.ByteStructures;
using Compression.PPM;

namespace compression.AC_R {
    public class ArithmeticCoder {

        private List<ContextTable> _contextTables;
        
        public ArithmeticCoder(List<ContextTable> contextTables) {
            _contextTables = contextTables;
        }

        public List<UnevenByte> Encode(byte[] input) {
            SymbolList noContext = new SymbolList();
            var zeroOrder = _contextTables[1].ContextDict[noContext];

            for (int i = 0; i < input.Length; ++i) {
                var k = zeroOrder[new Letter(input[i])];
                
            }

            return null;
        }
    }
}