namespace Compression.ByteStructures {
    public abstract class EncodedByte {
        public abstract UnevenByte ToUnevenByte();
        public abstract EncodedByte UnevenByteToEncodedByte(UnevenByte unevenByte);
        public abstract uint GetUnevenByteLength(byte firstByte);
    }
}