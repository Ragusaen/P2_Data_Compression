using System;
using System.ComponentModel;
using System.IO;
using System.Threading;
using Compression;
using Eto.Forms;
using Thread = Eto.Threading.Thread;
using System.ComponentModel; 


namespace Gui {
    public partial class Form1 : Form {
        private bool _isRunning;
        private BackgroundWorker _worker1; 
        private int _maxProcentageReached = 0;
        private Label filetextArea = new Label();
        private Label compressionSpeedlabel = new Label();
        private Label compressionRatioLabel = new Label();
        private Label filenameTableLabel = new Label();
        private Label typeOfCompression = new Label();
        private Thread _thread2 = null;

        private delegate void SafeCallDelegate(DataFile file);
        private ICompressor _compressor;
        //private BackgroundWorker worker1;

        public string Path = string.Empty;
        private String _fileName;
        private FileInfo _fileInfo;
        private String _typeOfCompression = "";

        public Form1() {
            InitializeComponent();
        }

        private void Compress(object sender, EventArgs e) {
            var timer = System.Diagnostics.Stopwatch.StartNew();
            DataFile input_file = new DataFile(Path);
            string tempDocPath = Path + _fileName + ".Compress";
            var compressedFile = _compressor.Compress(input_file); 
            var compressionTime = timer.ElapsedMilliseconds;
            compressedFile.WriteToFile(tempDocPath);

            filenameTableLabel.Text = _fileName;
            compressionSpeedlabel.Text = compressionTime.ToString() + " ms";
            compressionRatioLabel.Text = ((double) compressedFile.Length / input_file.Length).ToString();
            typeOfCompression.Text = _typeOfCompression;

        }

        private void Decompress(object sender, EventArgs e) {
            DataFile input_file = new DataFile(Path);
            string tempDocPath = Path + _fileName + ".Compress";
            var compressedFile = _compressor.Decompress(input_file); 
            compressedFile.WriteToFile(tempDocPath);
        }
        
        private void OpenFileClick(object sender, EventArgs e) {
            OpenFileDialog s = new OpenFileDialog();
            if (s.ShowDialog(Application.Instance.MainForm) == DialogResult.Ok) {
                Path = s.FileName;
                
                _fileInfo = new FileInfo(Path);
                _fileName = _fileInfo.Name;
                filetextArea.Text = "Filename: " + _fileInfo.Name; 
            }
        }

    }
}