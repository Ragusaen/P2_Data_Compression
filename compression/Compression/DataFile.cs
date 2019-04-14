using System;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using compression.ByteStructures;

namespace Compression {
    public class DataFile{
        private byte[] _byteArray;
        public uint Length {
            get { return (uint) _byteArray.Length; }
        }

        public DataFile(string path) {
            LoadFromFile(path);
        }

        public DataFile(byte[] data) {
            LoadBytes(data);
        }

        public DataFile() {
            _byteArray = new byte[0];
        }

        public byte[] GetBytes(uint start, uint len) {
            if (len == 0)
                return new byte[0];
            
            if(start + len > _byteArray.Length)
                throw new IndexOutOfRangeException();
            
            byte[] result = new byte[len];
            
            Array.Copy(_byteArray, start, result, 0, len);
            
            return result;
        }

        public ArrayIndexer<byte> GetArrayIndexer(int start, int len) {
            return new ArrayIndexer<byte>(_byteArray, start, len);
        }
        
        public byte GetByte(uint i) {
            if(i >= _byteArray.Length)
                throw new IndexOutOfRangeException();
            
            return _byteArray[i];
        }

        public byte[] GetAllBytes() {
            return _byteArray;
        }    

        public void LoadBytes(byte[] array) {
            _byteArray = array;
        }
        
        public void LoadFromFile(string path) {
            _byteArray = File.ReadAllBytes(path);
        }
        
        public void WriteToFile(string path) {
            File.WriteAllBytes(path, _byteArray);
        }
        
        public static Boolean Compare(DataFile a, DataFile b) {
            byte[] aArray = a.GetBytes(0, a.Length);
            byte[] bArray = b.GetBytes(0, b.Length);

            return aArray.SequenceEqual(bArray);
        }
    }
}