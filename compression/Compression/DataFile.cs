using System;
using System.IO;
using System.Linq;
using Compression.ByteStructures;

namespace Compression {
    /// <summary>
    /// This class is used to store the data of a file.
    /// </summary>
    public class DataFile{
        private byte[] _byteArray;
        public int Length {
            get { return _byteArray.Length; }
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

        public byte[] GetBytes(int start, int len) {
            if (len == 0)
                return new byte[0];
            
            if(start + len > _byteArray.Length)
                throw new IndexOutOfRangeException();
            
            byte[] result = new byte[len];
            
            Array.Copy(_byteArray, start, result, 0, len);
            
            return result;
        }
        
        public byte GetByte(int i) {
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