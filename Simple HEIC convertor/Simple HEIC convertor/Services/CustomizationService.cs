using MaterialDesignThemes.Wpf;
using Simple_HEIC_convertor.Enums;
using Simple_HEIC_convertor.Models;
using Simple_HEIC_convertor.ViewModels;
using System;
using System.IO;
using System.Text.Json;
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

            switch (selectedScheme)
            {
                case ColorSchemes.Green:
                    primaryColor = System.Windows.Media.Color.FromRgb(124, 179, 66);
                    break;
                case ColorSchemes.Purple:
                    primaryColor = System.Windows.Media.Color.FromRgb(170, 0, 255);
                    break;
                case ColorSchemes.Orange:
                    primaryColor = System.Windows.Media.Color.FromRgb(255, 195, 0);
                    break;
                case ColorSchemes.Custom:
                    _windowService.CreateWindowAndShow(new SettingsViewModel(this), "Simple_HEIC_convertor.ColorSchemeSettings");
                    return;
                default:
                    primaryColor = System.Windows.Media.Color.FromRgb(178, 255, 89);
                    break;
            }

            theme.SetPrimaryColor(primaryColor);
            paletteHelper.SetTheme(theme);
        }

        public void SetCustomRGBTheme(ColorRGB colorRGB)
        {
            var paletteHelper = new PaletteHelper();
            ITheme theme = paletteHelper.GetTheme();
            theme.SetPrimaryColor(System.Windows.Media.Color.FromRgb(colorRGB.R, colorRGB.G, colorRGB.B));
            paletteHelper.SetTheme(theme);
            SaveCurrentSettings(colorRGB);
        }

        private void SaveCurrentSettings(ColorRGB colorRGB)
        {
            using (FileStream fs = new FileStream("themeSettings.json", FileMode.OpenOrCreate))
            {
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
                    colorRGB = await JsonSerializer.DeserializeAsync<Models.ColorRGB>(fs);
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
