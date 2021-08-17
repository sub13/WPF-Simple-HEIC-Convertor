using Simple_HEIC_convertor.Commands;
using Simple_HEIC_convertor.Models;
using Simple_HEIC_convertor.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;

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
                        Thread thread = new Thread(() => _customizationService.SetCustomRGBTheme(SelectedColorRGB));
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
