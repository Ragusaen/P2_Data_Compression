using Compression.ByteStructures;

namespace compression.ByteStructures {
    public interface IEncodedByteConverter<T> where T: EncodedByte {
        UnevenByte ToUnevenByte(T encodedByte);
        int GetUnevenByteLength(UnevenByte controlBit);
        T ToEncodedByte(UnevenByte unevenByte);
    }
}