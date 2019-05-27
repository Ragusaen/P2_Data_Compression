using System;
using System.ComponentModel;
using Compression;
using Eto.Forms;
using System.ComponentModel;
using System.Threading.Tasks;
using Compression.LZ;
using Application = Eto.Forms.Application;
using FileInfo = System.IO.FileInfo;
using Thread = System.Threading.Thread;
using System.Diagnostics;
using System.IO;
using Compression;
using Eto.Forms;
using Eto.Threading;


namespace Gui {
    public partial class Form1 : Form {
        private FileInfo _fileInfo;
        private string _fileName;
        private bool _isRunning;
        private int _maxProcentageReached = 0;
        private Label _filetextArea = new Label();
        private Label _compressionSpeedlabel = new Label();
        private Label _compressionRatioLabel = new Label();
        private Label _typeOfCompressionLabel = new Label();
        private Thread _thread2 = null;
        private string _typeOfCompression = "";
        private BackgroundWorker _worker1;
        private readonly Label _filenameTableLabel = new Label();
        private ICompressor _compressor;
        private readonly Label filetextArea = new Label();

        public string Path = string.Empty;
        private readonly Label typeOfCompressionLabel = new Label();

       
         public Form1() {
            InitializeComponent();
         }
        private void Compress(object sender, EventArgs e){
            
            var timer = System.Diagnostics.Stopwatch.StartNew();
            DataFile input_file = new DataFile(Path);
            string tempDocPath = Path + ".Compress";
            
            var compressedFile = _compressor.Compress(input_file); 
            var compressionTime = timer.ElapsedMilliseconds;
            compressedFile.WriteToFile(tempDocPath);
            _typeOfCompressionLabel.Text = _typeOfCompression;
            _filenameTableLabel.Text = _fileName + ".Compress";
            _compressionSpeedlabel.Text = compressionTime.ToString() + " ms";
            _compressionRatioLabel.Text = ((double) compressedFile.Length / input_file.Length).ToString();
        }

        private void Decompress(object sender, EventArgs e){
            Console.WriteLine("We're here");
            DataFile input_file = new DataFile(Path);
            string tempDocPath = Path;
            var compressedFile = _compressor.Decompress(input_file);
        }

        private void OpenFileClick(object sender, EventArgs e) {
            var s = new OpenFileDialog();
            if (s.ShowDialog(Application.Instance.MainForm) == DialogResult.Ok) {
                Path = s.FileName;

                _fileInfo = new FileInfo(Path);
                _fileName = _fileInfo.Name;
                _filetextArea.Text = "Filename: " + _fileInfo.Name; 
            }
        }

        private delegate void SafeCallDelegate(DataFile file);
    }
}