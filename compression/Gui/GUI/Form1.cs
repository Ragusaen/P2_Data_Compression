using System;
using System.ComponentModel;
using System.IO;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Compression;
using Compression.ByteStructures;
using Compression.Huffman;
using Compression.AC_R;
using Compression.LZ;
using Compression.PPM;
using Eto.Forms; 
using Eto.Drawing;
using Eto;

namespace Gui {
    public partial class Form1 : Form {
        private readonly ICompressor _ppmCompressor = new PredictionByPartialMatching();
        private readonly ICompressor _lzCompressor = new LZ77();
        private readonly ICompressor _huffCompressor = new HuffmanCompressor();

        Stopwatch stopwatch = new Stopwatch();
        // UITimer timer;

        public string path = "";
        public const string DocPath = "../../res/";


        public Form1() {
            InitializeComponent();
        }
        
        
        private void OpenFileClick(object sender, EventArgs e) {
            OpenFileDialog s = new OpenFileDialog();
            if (s.ShowDialog(Application.Instance.MainForm) == DialogResult.Ok) {
                path = s.FileName;
            }
        }

        private void RunHuffmanCompressButton(Object sender, EventArgs e) {
            DataFile input_file = new DataFile(path);
            var compressed_huffman = _huffCompressor.Compress(input_file);
            compressed_huffman.WriteToFile(DocPath + input_file);
        }

        private void RunLZCompressButton(Object sender, EventArgs e) {
            DataFile input_file = new DataFile(path);
            var compressed_lz = _lzCompressor.Compress(input_file);
            compressed_lz.WriteToFile(DocPath + input_file);
        }

        private void RunPPMCompressButton(Object sender, EventArgs e) {
            DataFile input_file = new DataFile(path);
            var compressed_PPM = _ppmCompressor.Compress(input_file);
            compressed_PPM.WriteToFile(DocPath + input_file);
        }
    }
}