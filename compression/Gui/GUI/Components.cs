using System;
using Compression.Huffman;
using Compression.LZ;
using Compression.PPM;
using Eto.Drawing;
using Eto.Forms;

namespace Gui {
    partial class Form1 {
        private ProgressBar _progressBar;

        private void InitializeComponent() {
            #region Client
            Title = "Compression";
            ClientSize = new Size(600, 400);
            #endregion

            #region Commands
            var openFileCommand = new Command { MenuText = "Open"};
            var aboutCommand = new Command { MenuText = "About..." };
            var quitCommand = new Command { MenuText = "Quit", 
                Shortcut = Application.Instance.CommonModifier | Keys.Q };
            
            openFileCommand.Executed += (sender, e) => {
                OpenFileDialog s = new OpenFileDialog();
                if (s.ShowDialog(Application.Instance.MainForm) == DialogResult.Ok) {
                    Path = s.FileName;
                }
            };
            
            quitCommand.Executed += (sender, e) => Application.Instance.Quit();
				
            // About dialog executor
            aboutCommand.Executed += (sender, e) => {
                new Dialog {
                    Content = new Label {Text = "Visit https://github.com/Ragusaen/P2_Data_Compression/blob/master/README.md"},
                    ClientSize = new Size(200, 50)
                }.ShowModal(this);
            };
            #endregion

            #region Buttons
            
            var openfileButton = new Button {Text = "Open"};
            openfileButton.Click += OpenFileClick;

            var _runButton = new Button {Text = "Run"};
            _runButton.BindDataContext(c => c.BackgroundColor, (ButtonColor m) => m.ButtonBackgroundColor);
            _runButton.BindDataContext(c => c.Command, (ButtonColor m) => m.ChangeColorCommand);
            
            #endregion

            #region Control

            Control Selectbutton() {
                var selecButton = new DropDown();
                selecButton.Items.Add("Prediction by Partial Matching", "a");
                selecButton.Items.Add("Huffman encoder", "b");
                selecButton.Items.Add("LzSS compression", "c");
                selecButton.SelectedIndex = 0; 
                selecButton.SelectedIndexChanged += (sender, args) => {
                    if (selecButton.SelectedKey == "a") {
                        _compressor =  new PredictionByPartialMatching();
                        _typeOfCompression = "PPM";
                        _runButton.Click += Compress; 
                    }
                    else if (selecButton.SelectedKey == "b") {
                        _compressor = new HuffmanCompressor();
                        _typeOfCompression = "HM";
                        _runButton.Click += Compress;
                    }
                    else if (selecButton.SelectedKey == "c") {
                        _compressor = new LZSS();
                        _typeOfCompression = "LZSS";
                        _runButton.Click += Compress;
                    }
                };
                return selecButton; 
            }
            
            Control Indeterminate() {
                var control = new ProgressBar {
                    Indeterminate = _isRunning
                };
                return control;
            }

            var textlabel = new Label();
            textlabel.Text = "Compression Name";
            
            
            #endregion

            #region Menu
            // create menu
            Menu = new MenuBar
            {
                Items = {
                    // File submenu
                    new ButtonMenuItem { Text = "&File", Items = {openFileCommand} }
                    // new ButtonMenuItem { Text = "&Edit", Items = { /* commands/items */ } },
                    // new ButtonMenuItem { Text = "&View", Items = { /* commands/items */ } },
                },
                ApplicationItems = {
                    // application (OS X) or file menu (others)
                    new ButtonMenuItem { Text = "&Preferences..." }
                },
                QuitItem = quitCommand,
                AboutItem = aboutCommand
            };
            #endregion

            #region Content
            Content = new TableLayout
            {
                Spacing = new Size(5,5),
                Padding = new Padding(10,10,10,2),
                Rows = {
                    new TableRow(
                        new TableCell(new Label{TextAlignment = TextAlignment.Center, Text = "Please select file"}, true),
                        new TableCell(new Label{TextAlignment = TextAlignment.Center, Text = "Please select compression method"}, true),
                        new Label{TextAlignment = TextAlignment.Center, Text = "Run"},
                        new TableCell()
                    ),
                    new TableRow(
                        openfileButton,
                        Selectbutton(),
                        _runButton,
                        _progressBar

                    ),
                    new TableRow(
                        new TableCell(filetextArea)
                    ),
                    new TableRow(
                        new TableCell()
                        ),
                    new TableRow(
                        new TableCell()
                    ),
                    new TableRow(
                        new TableCell(
                            new Label{Text = "Compression type"}, true),
                            new TableCell(new Label{Text = "Filename"}, true),
                        new TableCell(new Label{Text = "Compression Ratio"}, true), 
                        new TableCell(new Label{Text = "Compression Speed"}, true),
                        new TableCell()
                    ),new TableRow(
                        new TableCell(typeOfCompressionLabel),
                        new TableCell(filenameTableLabel),
                        new TableCell(compressionRatioLabel),
                        new TableCell(compressionSpeedlabel),
                        new TableCell()
                    ),
                    new TableRow{ScaleHeight = true}
                    
                }
            };
            
            this.DataContext = new ButtonColor();
            #endregion

        }


    }
}