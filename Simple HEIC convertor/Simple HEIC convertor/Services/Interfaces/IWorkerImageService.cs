using Simple_HEIC_convertor.Enums;
using Simple_HEIC_convertor.Models;
using System.Collections.ObjectModel;

namespace Simple_HEIC_convertor.Services.Interfaces
{
    interface IWorkerImageService
    {
        void ConvertToHEIC(ObservableCollection<ImageFile> imageFiles, ImageFormat imageFormat, string convertPath = null);
        void OpenHEICImage(string imagePath);
    }
}
