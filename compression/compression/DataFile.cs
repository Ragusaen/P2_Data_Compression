using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace compression {
    public class DataFile{
        private byte[] _byteArray;

        public DataFile(string path) {
            LoadFromFile(path);
        }       
        
        public DataFile() {}

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

        public uint Length() {
            return (uint) _byteArray.Length;
        }
    }
}