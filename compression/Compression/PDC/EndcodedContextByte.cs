using Compression.ByteStructures;

namespace PDC {
    public class EndcodedContextByte : EncodedByte {
        public override UnevenByte ToUnevenByte() {
            throw new System.NotImplementedException();
        }

        public override uint GetUnevenByteLength(byte firstByte) {
            throw new System.NotImplementedException();
        }

        public override EncodedByte UnevenByteToEncodedByte(UnevenByte unevenByte) {
            throw new System.NotImplementedException();
        }
    }
}