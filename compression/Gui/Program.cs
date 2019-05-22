using System;
using Eto; 
using Eto.Forms; 
using Eto.Drawing;

namespace GUI {
    internal class Program {
        [STAThread]
        public static void Main(string[] args) {
            new Application().Run(new MainForm());
        }
    }
}