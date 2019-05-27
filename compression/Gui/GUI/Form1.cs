using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using Compression;
using Eto.Forms;
using Eto.Threading;

namespace Gui {
    public partial class Form1 : Form {
        private ICompressor _compressor;
        private FileInfo _fileInfo;
        private string _fileName;
        private bool _isRunning;
        private int _maxProcentageReached = 0;
        private Thread _thread2 = null;
        private string _typeOfCompression = "";
        private BackgroundWorker _worker1;
        private readonly Label compressionRatioLabel = new Label();
        private readonly Label compressionSpeedlabel = new Label();
        private readonly Label filenameTableLabel = new Label();

        private readonly Label filetextArea = new Label();
        //private BackgroundWorker worker1;

        public string Path = string.Empty;
        private readonly Label typeOfCompressionLabel = new Label();

        public Form1() {
            InitializeComponent();
        }

        private void Compress(object sender, EventArgs e) {
            var timer = Stopwatch.StartNew();
            var input_file = new DataFile(Path);
            var tempDocPath = Path + _fileName + ".Compress";
            var compressedFile = _compressor.Compress(input_file);
            var compressionTime = timer.ElapsedMilliseconds;
            compressedFile.WriteToFile(tempDocPath);

            typeOfCompressionLabel.Text = _typeOfCompression;
            filenameTableLabel.Text = _fileName;
            compressionSpeedlabel.Text = compressionTime + " ms";
            compressionRatioLabel.Text = ((double) compressedFile.Length / input_file.Length).ToString();
        }

        private void Decompress(object sender, EventArgs e) {
            var input_file = new DataFile(Path);
            var tempDocPath = Path + _fileName + ".Compress";
            var compressedFile = _compressor.Decompress(input_file);
            compressedFile.WriteToFile(tempDocPath);
        }

        private void OpenFileClick(object sender, EventArgs e) {
            var s = new OpenFileDialog();
            if (s.ShowDialog(Application.Instance.MainForm) == DialogResult.Ok) {
                Path = s.FileName;

                _fileInfo = new FileInfo(Path);
                _fileName = _fileInfo.Name;
                filetextArea.Text = "Filename: " + _fileInfo.Name;
            }
        }

        private delegate void SafeCallDelegate(DataFile file);
    }
}