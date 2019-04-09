using System;
using System.Collections.Generic;
using Compression.PPM;
using System.Linq;

namespace Compression.Arithmetic
{
    public class ArithmeticEncoding
    {

        public DataFile encodingArithmetic(DataFile input, List<ContextTable> ppmTables)
        {
            return null;
        }

        public Dictionary<byte, int> ZeroOrder(DataFile input)
        {
            byte[] byteArray = input.GetAllBytes();
            Dictionary<byte, int> countList = new Dictionary<byte, int>();

            foreach (byte sym in byteArray)
            {
                
            }
            
            return null;
        }
    }
}