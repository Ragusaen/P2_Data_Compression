using System;
using System.ComponentModel;
using System.Windows.Input;
using System.Runtime.CompilerServices;
using Eto.Forms;
using Eto.Drawing;

namespace Gui
{
    public partial class ButtonColor : INotifyPropertyChanged{
            public event PropertyChangedEventHandler PropertyChanged;

            static Color DefaultButtonBackgroundColor = new Button().BackgroundColor;
            bool _buttonIsGreen;
            Color _buttonBackgroundColor = DefaultButtonBackgroundColor;

            public Color ButtonBackgroundColor {
                get => _buttonBackgroundColor;
                private set {
                    _buttonBackgroundColor = value;
                    TriggerPropertyChanged();
                }
            }

            Command _changeColorCommand;
            
            public ICommand ChangeColorCommand {
                get {
                    var command = _changeColorCommand;
                    if (command != null) {
                        return command;
                    }

                    return (_changeColorCommand = new Command(ChangeColor));
                }
            }

            void ChangeColor(object sender, EventArgs e) {
                _buttonIsGreen = !_buttonIsGreen;
                if (!_buttonIsGreen)
                    ButtonBackgroundColor = DefaultButtonBackgroundColor;
                else {
                    ButtonBackgroundColor = new Color(Colors.Green, 0.2f);
                }
            }

            private void TriggerPropertyChanged([CallerMemberName] string memberName = null) {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
            }
    }
}