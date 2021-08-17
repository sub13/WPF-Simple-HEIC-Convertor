using ImageMagick;
using Simple_HEIC_convertor.Enums;
using Simple_HEIC_convertor.Models;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Windows;

namespace Simple_HEIC_convertor.Services
{
    public class WorkerImageService : IWorkerImageService
    {
        public void ConvertToHEIC(ObservableCollection<ImageFile> imageFiles, ImageFormat imageFormat, string convertPath = null)
        {
            MagickImage Image;

            bool IsPathChose = true;
            try
            {
                int countPhoto = imageFiles.Count;
                if(countPhoto == 0)
                    MessageBox.Show($"Вы не открытыли ни одной фотографии!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);

                if (convertPath == null || convertPath == "")
                {
                    convertPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                    IsPathChose = false;
                }
                foreach (ImageFile imageFile in imageFiles)
                {
                    using (MagickImage image = new MagickImage(imageFile.Path))
                    {
                        int pos = imageFile.Path.LastIndexOf(@"\");
                        StringBuilder path = new StringBuilder(imageFile.Path.Substring(pos));
                        if (ImageFormat.Jpeg == imageFormat)
                        {
                            image.Format = MagickFormat.Jpeg;
                            Image = image;
                            path.Remove(path.ToString().LastIndexOf("."), 5);
                            path.Append(".jpeg");
                        }
                        else if (ImageFormat.Png == imageFormat)
                        {
                            image.Format = MagickFormat.Png;
                            Image = image;
                            path.Remove(path.ToString().LastIndexOf("."), 5);
                            path.Append(".png");
                        }
                        image.Write(convertPath + path.ToString());
                    }
                    countPhoto--;
                }
                if (!IsPathChose)
                    MessageBox.Show("Конвертация успешно завершена!Поскольку вы ранее не выбрали путь, все файлы были сохранены на рабочем столе!", "Успех!", MessageBoxButton.OK, MessageBoxImage.Information);
                else
                    MessageBox.Show("Конвертация успешно завершена!", "Успех!", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception exc)
            {
                MessageBox.Show($"При конвертировании произошла ошибка! {exc.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void OpenHEICImage(string imagePath)
        {
            using (MagickImage image = new MagickImage(imagePath))
            {
                image.Format = MagickFormat.Jpeg;
                int pos = imagePath.LastIndexOf(@"\");
                StringBuilder reviewPath = new StringBuilder($@"C:\Windows\Temp{imagePath.Substring(pos)}");
                reviewPath.Remove(reviewPath.ToString().LastIndexOf("."), 5);
                reviewPath.Append(".jpeg");
                image.Write(reviewPath.ToString());
                Process proc = Process.Start(reviewPath.ToString());
            }
        }
    }
}
