using System;
using Eto.Drawing;
using Eto.Forms;


namespace GUI 
{
	public sealed class MainForm : Form {
		public MainForm() {
			string path;

			#region Client
			Title = "Compression";
			ClientSize = new Size(600, 400);
			#endregion

			#region Commands
			// Open file command
			var openFile = new Command { MenuText = "Open"};
			openFile.Executed += (sender, e) => {
				OpenFileDialog s = new OpenFileDialog();
				if (s.ShowDialog(Application.Instance.MainForm) == DialogResult.Ok) {
					path = s.FileName; 
				};
			};
			// Quit program command
			var quitCommand = new Command { MenuText = "Quit", Shortcut = Application.Instance.CommonModifier | Keys.Q };
			quitCommand.Executed += (sender, e) => Application.Instance.Quit();
			
			// link command 
			var aboutCommand = new Command { MenuText = "About..." };
			aboutCommand.Executed += (sender, e) => {
				new Dialog {
					Content = new Label {Text = "Visit <Insert URL to git.readme>"},
					ClientSize = new Size(200, 200)
				}.ShowModal(this);
			};
			#endregion

			#region Content
			Content = new TableLayout
			{
				Spacing = new Size(5,5),
				Padding = new Padding(10,10,10,10),
				Rows = {
					new TableRow(
						new TableCell(new Label{Text = "Open file"}, true),
						new TableCell(new Label{Text = "Select method"}, true),
						new Label{Text = "Process"}
					),
					new TableRow(
						new Button((sender, e) => openFile.Execute()),
						new DropDown { Items = { "Item 1", "Item 2", "Item 3" } },
							
						new CheckBox { Text = "A checkbox" }
					),
					new TableRow{ScaleHeight = true}
				}
			};
			#endregion
			
			#region Menu
			// create menu
			Menu = new MenuBar
			{
				Items = {
					// File submenu
					new ButtonMenuItem { Text = "&File", Items = { openFile } }
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
		}
	}
}