using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace Compression {
    public class DataFile{
        private byte[] ByteArray;

        public byte[] GetBytes(uint start, uint len) {
            byte[] result = new byte[len];
            
            Array.Copy(ByteArray, start, result, 0, len);
            
            return result;
        }

        public void LoadFromFile(string path) {
            ByteArray = File.ReadAllBytes(path);
        }
    }
}