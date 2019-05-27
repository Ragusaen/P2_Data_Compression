using Compression.ByteStructures;

namespace Compression.Huffman
{
    /// <summary>
    /// This class implements Huffman Encoding. It creates a BitString and inserts the encoded
    /// dictionary and all the encoded bytes.
    /// </summary>
    public class HuffmanEncoder {
        public byte[] EncodeAllBytes (HuffmanTree huffmanTree, byte[] data) {
            BitString bitString = new BitString();

            // Calculate filler bits
            UnevenByte fillerBits = CreateFillerUnevenByte(huffmanTree);

            bitString.Append(fillerBits);
            
            // Append the encoded tree
            for (int i = 0; i < huffmanTree.EncodedTreeList.Count; ++i) {
                bitString.Append(huffmanTree.EncodedTreeList[i]);
            }
            
            // Encode all the bytes
            for (int i = 0; i < data.Length; i++) {
                bitString.Append(huffmanTree.CodeDictionary[data[i]]);
            }

            return bitString.ToArray();
        }
        
        /// <summary>
        /// This method creates an UnevenByte so the last byte contains 8 bits.
        /// </summary>
        /// <param name="huffmanTree"> Contains TotalLeafs and TotalLength </param>
        /// <param name="sizeOfTree"> Number of different byte that appeared in input </param>
        /// <param name="TotalLength"> The total size of all the encoded bytes </param>
        /// <returns> An UnevenByte filled with 1s with a length that fills the last byte </returns>
        private UnevenByte CreateFillerUnevenByte(HuffmanTree huffmanTree) {
            int sizeOfTree = huffmanTree.TotalLeafs * 10 - 1;
            int bitsInLastByte = (sizeOfTree + huffmanTree.TotalLength) % 8;
            
            if (bitsInLastByte > 0) { 
                uint fillOnes = (uint)0b11111111 >> bitsInLastByte;

                return new UnevenByte(fillOnes, 8 - bitsInLastByte);
            }
            return default(UnevenByte);
        }
    }
}
