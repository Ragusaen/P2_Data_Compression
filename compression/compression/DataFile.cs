using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace Compression {
    public class DataFile{
        private byte[] _byteArray;

        public byte[] GetBytes(uint start, uint len) {
            if (len == 0)
                return new byte[0];
            
            if(start + len > _byteArray.Length)
                throw new IndexOutOfRangeException();
            
            byte[] result = new byte[len];
            
            Array.Copy(_byteArray, start, result, 0, len);
            
            return result;
        }

        public void LoadFromFile(string path) {
            _byteArray = File.ReadAllBytes(path);
        }
    }
}