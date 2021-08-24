using Simple_HEIC_convertor.Enums;
using Simple_HEIC_convertor.Models;
using System.Threading.Tasks;

namespace Simple_HEIC_convertor.Services.Interfaces
{
    interface ICustomizationService
    {
        void Set_Theme(bool IsDarkTheme);
        void SetCustomRGBTheme(ColorRGB colorRGB, bool IsNeedSaveSettings = false);
        void ChangeColorScheme(ColorSchemes selectedScheme);
        Task<Models.ColorRGB> ReadColorSettingsFromJsonAsync();
    }
}
