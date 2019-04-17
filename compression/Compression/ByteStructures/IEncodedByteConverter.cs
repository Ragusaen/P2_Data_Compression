using Compression.ByteStructures;

namespace compression.ByteStructures {
    public interface IEncodedByteConverter<T> where T: EncodedByte {
        UnevenByte ToUnevenByte(T encodedByte);
        uint GetUnevenByteLength(byte firstByte);
        T ToEncodedByte(UnevenByte unevenByte);
    }
}