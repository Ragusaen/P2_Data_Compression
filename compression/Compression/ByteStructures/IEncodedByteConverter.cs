namespace Compression.ByteStructures {
    public interface IEncodedByteConverter<T> where T: EncodedByte {
        UnevenByte ToUnevenByte(T encodedByte);
        int GetUnevenByteLength(UnevenByte controlBit);
        T ToEncodedByte(UnevenByte unevenByte);
    }
}