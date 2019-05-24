using System;
using System.ComponentModel;
using System.IO;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows.Input;
using Compression;
using Compression.ByteStructures;
using Compression.Huffman;
using Compression.LZ;
using Compression.PPM;
using Eto.Forms; 
using Eto.Drawing;
using Eto;

namespace Gui {
    public partial class Form1 : Form {
        private readonly ICompressor _ppmCompressor = new PredictionByPartialMatching();
        private readonly ICompressor _lzCompressor = new LZSS();
        private readonly ICompressor _huffCompressor = new HuffmanCompressor();
        
        bool ppmClicked = false;
        bool huffClicked = false;
        bool lzClicked = false; 

        private bool _isOn;

        Stopwatch stopwatch = new Stopwatch();
        // UITimer timer;

        public string path = "";
        public const string DocPath = "../../res/";
        private String fileName;
        private System.IO.FileInfo fileInfo;

        public Form1() {
            InitializeComponent();
        }


        private void OpenFileClick(object sender, EventArgs e) {
            OpenFileDialog s = new OpenFileDialog();
            if (s.ShowDialog(Application.Instance.MainForm) == DialogResult.Ok) {
                path = s.FileName;
                
                fileInfo = new FileInfo(path);
                fileName = fileInfo.Name;
            }
        }

        private void RunHuffmanCompressButton(Object sender, EventArgs e) {
            if (huffClicked == true) {
                DataFile input_file = new DataFile(path);
                String tempDocPath = DocPath + fileName + ".HM";
                var compressed_huffman = _huffCompressor.Compress(input_file);
                compressed_huffman.WriteToFile(tempDocPath);
            }
           
        }

        private void RunLZCompressButton(Object sender, EventArgs e) {
            if (lzClicked == true) {
                DataFile input_file = new DataFile(path);
                String tempDocPath = DocPath + fileName + ".LZ";
                var compressed_lz = _lzCompressor.Compress(input_file);
                compressed_lz.WriteToFile(tempDocPath);
            }
        }

        private void RunPPMCompressButton(Object sender, EventArgs e) {
            if (ppmClicked == true) {
                DataFile input_file = new DataFile(path);
                String tempDocPath = DocPath + fileName + ".PPM";
                var compressed_PPM = _ppmCompressor.Compress(input_file);

                compressed_PPM.WriteToFile(tempDocPath);
            }
        }
    }
}