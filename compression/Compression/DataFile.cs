using System;
using System.IO;
using System.Linq;

namespace Compression {
    /// <summary>
    ///     This class is used to store the data of a file.
    /// </summary>
    public class DataFile {
        private byte[] _byteArray;

        public DataFile(string path) {
            LoadFromFile(path);
        }

        public DataFile(byte[] data) {
            LoadBytes(data);
        }

        public DataFile() {
            _byteArray = new byte[0];
        }

        public int Length => _byteArray.Length;

        public byte[] GetBytes(int start, int len) {
            if (len == 0)
                return new byte[0];

            if (start + len > _byteArray.Length)
                throw new IndexOutOfRangeException();

            var result = new byte[len];

            Array.Copy(_byteArray, start, result, 0, len);

            return result;
        }

        public byte GetByte(int i) {
            if (i >= _byteArray.Length)
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

        public static bool Compare(DataFile a, DataFile b) {
            var aArray = a.GetBytes(0, a.Length);
            var bArray = b.GetBytes(0, b.Length);

            return aArray.SequenceEqual(bArray);
        }
    }
}