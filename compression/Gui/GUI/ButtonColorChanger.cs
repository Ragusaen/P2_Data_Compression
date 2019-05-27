using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Eto.Drawing;
using Eto.Forms;

namespace Gui {
    public class ButtonColor : INotifyPropertyChanged {
        private static readonly Color DefaultButtonBackgroundColor = new Button().BackgroundColor;
        private Color _buttonBackgroundColor = DefaultButtonBackgroundColor;
        private bool _buttonIsGreen;

        private Command _changeColorCommand;

        public Color ButtonBackgroundColor {
            get => _buttonBackgroundColor;
            private set {
                _buttonBackgroundColor = value;
                TriggerPropertyChanged();
            }
        }

        public ICommand ChangeColorCommand {
            get {
                var command = _changeColorCommand;
                if (command != null) return command;

                return _changeColorCommand = new Command(ChangeColor);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void ChangeColor(object sender, EventArgs e) {
            _buttonIsGreen = !_buttonIsGreen;
            if (!_buttonIsGreen)
                ButtonBackgroundColor = DefaultButtonBackgroundColor;
            else
                ButtonBackgroundColor = new Color(Colors.Green, 0.2f);
        }

        private void TriggerPropertyChanged([CallerMemberName] string memberName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
        }
    }
}