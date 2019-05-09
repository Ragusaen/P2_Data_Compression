using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Compression;

namespace compression {
    
   
    
    public class FileSystemContainer {
        private abstract class FileTreeNode {
            public List<FileTreeNode> nodes;
        }

        private class FileTreeDataFileNode : FileTreeNode {
            public string filename;
            public DataFile DataFile;
        }

    }
}