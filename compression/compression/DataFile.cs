using System;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace compression {
    public class DataFile{
        private byte[] _byteArray;
        public uint Length {
            get { return (uint) _byteArray.Length; }
        }

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