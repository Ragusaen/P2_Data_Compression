using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using Compression;

namespace compression {
    
   
    
    public class FileSystemContainer {
        #region Nodes
        private abstract class FileTreeNode {
            public string path;
            public string name;

            public FileTreeNode(string path) {
                path = path;
                name = Path.GetFileName(path);
            }
        }
        private class FileTreeDataFileNode : FileTreeNode {
            public DataFile DataFile;
            public FileTreeDataFileNode(string path) : base(path) {}
        }
        private class FileTreeDirectoryNode : FileTreeNode {
            public List<FileTreeDirectoryNode> DirectoryNodes = new List<FileTreeDirectoryNode>();
            public List<FileTreeDataFileNode> DataFileNodes = new List<FileTreeDataFileNode>();
            public FileTreeDirectoryNode(string path) : base(path) {}
        }
        #endregion

        private FileTreeDirectoryNode rootNode;
        private int FileCount = 0;
        private int TotalFileSize = 0;
        
        
        private const int DIRECTORY_ENCODING_SIZE = 3;
        private const int FILE_ENCODING_SIZE = 9;

        public FileSystemContainer(string directory) {
            if (!Directory.Exists(directory))
                throw new DirectoryNotFoundException();
            
            rootNode = new FileTreeDirectoryNode(directory);
            CreateDirectoryNode(rootNode);
        }

        private void CreateDataFileNode(FileTreeDataFileNode node) {
            node.DataFile = new DataFile(node.path);
            FileCount++;
            TotalFileSize += node.name.Length + node.DataFile.Length + FILE_ENCODING_SIZE;
        }

        private void CreateDirectoryNode(FileTreeDirectoryNode node) {
            string[] directories = Directory.GetDirectories(node.path);
            foreach (string dir in directories) {
                var newDir = new FileTreeDirectoryNode(dir);
                node.DirectoryNodes.Add(newDir);
                CreateDirectoryNode(newDir);
            }

            string[] files = Directory.GetFiles(node.path);
            foreach (string fil in files) {
                var newFil = new FileTreeDataFileNode(fil);
                node.DataFileNodes.Add(newFil);
                CreateDataFileNode(newFil);
            }

            TotalFileSize += DIRECTORY_ENCODING_SIZE;
        }

        public DataFile EncodeAsDataFile() {
            byte[] bytes = new byte[TotalFileSize];
            int index = 0;
            EncodeDirectoryNode(rootNode, ref bytes, ref index);
            return new DataFile(bytes);
        }

        private void EncodeDataFileNode(FileTreeDataFileNode node, ref byte[] bytes, ref int index) {
            // Encode length of directory name
            bytes[index] = (byte)node.name.Length;
            index++;

            // Encode the file name
            for (int si = 0; si < (byte) node.name.Length; ++si, ++index) {
                bytes[index] = (byte)node.name[si];
            }

            // Encode length (Little-endian)
            int l = node.DataFile.Length;
            bytes[index++] = (byte)l;
            bytes[index++] = (byte) (l >> 8);
            bytes[index++] = (byte) (l >> 16);
            bytes[index++] = (byte) (l >> 24);
            
            // Encode data from file
            for (int i = 0; i < node.DataFile.Length; ++i, index++) {
                bytes[index] = node.DataFile.GetByte(i);
            }
        }

        private void EncodeDirectoryNode(FileTreeDirectoryNode node, ref byte[] bytes, ref int index) {
            // Encode length of directory name
            bytes[index] = (byte)node.name.Length;
            index++;

            // Encode the directory name
            for (int si = 0; si < (byte) node.name.Length; ++si, ++index) {
                bytes[index] = (byte)node.name[si];
            }
            
            // Encode amount of directories
            bytes[index] = (byte)node.DirectoryNodes.Count;
            ++index;
            
            // Encode amount of files
            bytes[index] = (byte)node.DataFileNodes.Count;
            ++index;
            
            // Encode subdirectories
            foreach (var dir in node.DirectoryNodes) {
                EncodeDirectoryNode(dir, ref bytes, ref index);
            }
            
            // Encode files
            foreach (var fil in node.DataFileNodes) {
                EncodeDataFileNode(fil, ref bytes, ref index);
            }
        }
        
    }
}