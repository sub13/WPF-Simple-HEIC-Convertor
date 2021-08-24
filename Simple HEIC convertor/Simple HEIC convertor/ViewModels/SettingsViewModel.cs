using MaterialDesignThemes.Wpf;
using Simple_HEIC_convertor.Commands;
using Simple_HEIC_convertor.Models;
using Simple_HEIC_convertor.Services;
using Simple_HEIC_convertor.Services.Interfaces;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Input;
using System.Windows.Media;

namespace Simple_HEIC_convertor.ViewModels
{
    class SettingsViewModel : INotifyPropertyChanged
    {
        private readonly ICustomizationService _customizationService;
        
        public SettingsViewModel(ICustomizationService customizationService)
        {
            _customizationService = customizationService;
            _selectedColorRGB = new ColorRGB()
            {
                R = 1,
                G = 1,
                B = 1
            };
        }

        private FileImagesCommand _setThemeWithColorPicker;
        public FileImagesCommand SetThemeWithColorPicker
        {
            get
            {
                return _setThemeWithColorPicker ??
                    (_setThemeWithColorPicker = new FileImagesCommand(obj =>
                    {
                        var eventArgs = (MouseButtonEventArgs)obj;
                        ColorPicker colorPicker = (ColorPicker)eventArgs.Source;
                        ColorRGB newColorRGB = new ColorRGB()
                        {
                            R = colorPicker.Color.R,
                            G = colorPicker.Color.G,
                            B = colorPicker.Color.B
                        };

                        SelectedColorRGB = newColorRGB;
                        //Color color;
                        //ColorPickerWindow.ShowDialog(out color);
                    }));
            }
        }

        public ColorRGB _selectedColorRGB;

        public ColorRGB SelectedColorRGB
        {
            get
            {
                return _selectedColorRGB;
            }
            set
            {
                _selectedColorRGB = value;
                OnPropertyChanged("SelectedColorRGB");
            }
        }

        private FileImagesCommand _setThemeByRGB;
        public FileImagesCommand SetThemeByRGB
        {
            get
            {
                return _setThemeByRGB ??
                    (_setThemeByRGB = new FileImagesCommand(obj =>
                    {
                        Thread thread = new Thread(() => _customizationService.SetCustomRGBTheme(SelectedColorRGB, true));
                        thread.Start();
                    }));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
