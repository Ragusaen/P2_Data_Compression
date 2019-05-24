using Eto.Drawing;
using Eto.Forms;

namespace Gui {
    public class Progressbar : Panel {
        public Progressbar() {
            var layout = new DynamicLayout();
            layout.AddRow(Indeterminate());

            layout.Add(null, null, true);

            Content = layout;
        }

         Control Indeterminate() {
            var control = new ProgressBar {
                Indeterminate = false
            };
            return control;
        }
    }
}