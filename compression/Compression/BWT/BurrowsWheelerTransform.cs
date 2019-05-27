using System.Collections.Generic;
using Compression.ByteStructures;

namespace Compression.BWT {
    /// <summary>
    ///     This class performs the Burrows-Wheeler-Transform.
    /// </summary>
    public class BurrowsWheelerTransform {
        public byte[] Transform(byte[] input) {
            var transformMatrix = new List<byte[]>();

            for (var i = 0; i < input.Length; i++) transformMatrix.Add(ByteMethods.ShiftArray(input, i));

            transformMatrix.Sort(new ByteArrayComparer());

            var result = new byte[input.Length];
            for (var i = 0; i < input.Length; i++)
                result[i] = transformMatrix[i][input.Length - 1];

            return result;
        }

        public byte[] InverseTransform(byte[] input) {
            var itm = new List<byte[]>();

            for (var i = 0; i < input.Length; i++)
                itm.Add(new byte[input.Length]);

            for (var i = 0; i < itm.Count; i++) {
                for (var j = 0; j < input.Length; j++) {
                    itm[j] = ByteMethods.ShiftArray(itm[j], 1);
                    itm[j][0] = input[j];
                }

                itm.Sort(new ByteArrayComparer());
            }

            for (var i = 0; i < itm.Count; i++)
                if (itm[i][0] == (byte) '^')
                    return itm[i];
            return null;
        }
    }
}