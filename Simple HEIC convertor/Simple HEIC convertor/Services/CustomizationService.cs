using MaterialDesignThemes.Wpf;
using Simple_HEIC_convertor.Enums;
using Simple_HEIC_convertor.Models;
using Simple_HEIC_convertor.Services.Interfaces;
using Simple_HEIC_convertor.ViewModels;
using System;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Simple_HEIC_convertor.Services
{
    public class CustomizationService : ICustomizationService
    {
        private readonly IWindowService _windowService;

        public CustomizationService(IWindowService windowService)
        {
            _windowService = windowService;
        }

        public void Set_Theme(bool IsDarkTheme)
        {
            var paletteHelper = new PaletteHelper();
            ITheme theme = paletteHelper.GetTheme();
            if (IsDarkTheme)
                theme.SetBaseTheme(Theme.Dark);
            else
                theme.SetBaseTheme(Theme.Light);
            paletteHelper.SetTheme(theme);
        }

        public void ChangeColorScheme(ColorSchemes selectedScheme)
        {
            var paletteHelper = new PaletteHelper();
            ITheme theme = paletteHelper.GetTheme();
            System.Windows.Media.Color primaryColor;
            ColorRGB colorRGB;

            switch (selectedScheme)
            {
                case ColorSchemes.Green:
                    primaryColor = System.Windows.Media.Color.FromRgb(124, 179, 66);
                    colorRGB = new ColorRGB() { R = 124,  G = 179, B = 66 };
                    break;
                case ColorSchemes.Purple:
                    primaryColor = System.Windows.Media.Color.FromRgb(170, 0, 255);
                    colorRGB = new ColorRGB() { R = 170, G = 0, B = 255 };
                    break;
                case ColorSchemes.Orange:
                    primaryColor = System.Windows.Media.Color.FromRgb(255, 195, 0);
                    colorRGB = new ColorRGB() { R = 255, G = 195, B = 0 };
                    break;
                case ColorSchemes.Custom:
                    _windowService.CreateWindowAndShow(new SettingsViewModel(this), "Simple_HEIC_convertor.ColorSchemeSettings");
                    return;
                default:
                    primaryColor = System.Windows.Media.Color.FromRgb(124, 179, 66);
                    colorRGB = new ColorRGB() { R = 124, G = 179, B = 66 };
                    break;
            }

            theme.SetPrimaryColor(primaryColor);
            paletteHelper.SetTheme(theme);

            //SaveCurrentSettings(colorRGB);
            Thread thread = new Thread(() => SaveCurrentSettings(colorRGB));
            thread.Start();
        }

        public void SetCustomRGBTheme(ColorRGB colorRGB, bool IsNeedSaveSettings = false)
        {
            var paletteHelper = new PaletteHelper();
            ITheme theme = paletteHelper.GetTheme();
            theme.SetPrimaryColor(System.Windows.Media.Color.FromRgb(colorRGB.R, colorRGB.G, colorRGB.B));
            paletteHelper.SetTheme(theme);
            if(IsNeedSaveSettings)
                SaveCurrentSettings(colorRGB);
        }

        private void SaveCurrentSettings(ColorRGB colorRGB)
        {
            using (FileStream fs = new FileStream("themeSettings.json", FileMode.OpenOrCreate))
            {
                fs.SetLength(0);
                JsonSerializer.SerializeAsync(fs, colorRGB);
            }
        }

        public async Task<Models.ColorRGB> ReadColorSettingsFromJsonAsync()
        {
            Models.ColorRGB colorRGB = null;
            try
            {
                using (FileStream fs = new FileStream("themeSettings.json", FileMode.OpenOrCreate))
                {
                    if (fs.Length > 0)
                        colorRGB = await JsonSerializer.DeserializeAsync<Models.ColorRGB>(fs);
                    else
                        colorRGB = new ColorRGB() { R = 124, B = 179, G = 66 };
                    
                }
                return colorRGB;
            }
            catch (Exception exc)
            {
                MessageBox.Show($"При считывании конфигурации произошла ошибка! Использована тема по умолчанию: {exc.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }
    }
}
