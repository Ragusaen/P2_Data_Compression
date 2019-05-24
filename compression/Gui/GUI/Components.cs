using System;
using System.Net.Mime;
using Eto.Drawing;
using Eto.Forms;

namespace Gui {
    partial class Form1 {
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
                    path = s.FileName;
                };
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
            
            var openFileButton = new Button{Text = "Open"};
            openFileButton.Click += (OpenFileClick);

            var runButton = new Button{Text = "Run"};
            runButton.BindDataContext(c => c.BackgroundColor, (ButtonColor m) => m.ButtonBackgroundColor);
            runButton.BindDataContext(c => c.Command, (ButtonColor m) => m.ChangeColorCommand);

            Control fileTextAreaChooseFile() {
                var FiletextArea = new TextBox();
                FiletextArea.TextInput += (sender, args) => {
                    openFileButton.Click += OpenFileClick; 
                    FiletextArea.Text = fileName;
                    Console.WriteLine(fileName);
                }; 
                return FiletextArea; 
            }

            Control Selectbutton() {
                var selecButton = new DropDown();
                selecButton.Items.Add("Prediction by Partial Matching", "a");
                selecButton.Items.Add("Huffman encoder", "b");
                selecButton.Items.Add("LzSS compression", "c");
                selecButton.SelectedIndex = 0; 
                selecButton.SelectedIndexChanged += (sender, args) => {
                    if (selecButton.SelectedKey == "a") {
                        ppmClicked = true;
                        huffClicked = false;
                        lzClicked = false;
                        runButton.Click += RunPPMCompressButton; 
                        Console.WriteLine("ppm selected");
                    }
                    else if (selecButton.SelectedKey == "b") {
                        huffClicked = true;
                        ppmClicked = false;
                        lzClicked = false;
                        runButton.Click += RunHuffmanCompressButton; 
                        Console.WriteLine("huffman selected");
                    }
                    else if (selecButton.SelectedKey == "c") {
                        lzClicked = true;
                        ppmClicked = false;
                        huffClicked = false;
                        runButton.Click += RunPPMCompressButton; 
                        Console.WriteLine("Lz Selected");
                    }
                };
                return selecButton; 
            }
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
                        openFileButton,
                        Selectbutton(),
                        runButton,
                        new Progressbar()

                    ),
                    new TableRow(
                        new TableCell(fileTextAreaChooseFile())
                    ),
                    new TableRow{ScaleHeight = true}
                }
            };
            
            this.DataContext = new ButtonColor();
            #endregion

        }


    }
}