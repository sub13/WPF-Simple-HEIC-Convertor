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
            //CleanFilesButton.Visibility = Visibility.Hidden;

            bool IsPathChose = true;
            //if (!(bool)RadioButton1.IsChecked && !(bool)RadioButton2.IsChecked)
            //{
            //    MessageBox.Show("Вы не выбрали формат! Пожалуйста выберите формат!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            //    CleanFilesButton.Visibility = Visibility.Visible;
            //    return;
            //}
            try
            {
                int countPhoto = imageFiles.Count;
                // Доделать прогресс бар
                //progressBarValue = 100 / countPhoto;
                if(countPhoto == 0)
                    MessageBox.Show($"Вы не открытыли ни одной фотографии!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);

                if (convertPath == null || convertPath == "")
                {
                    // await convertPath = await Task.Run(() => Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory));
                    convertPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                    IsPathChose = false;
                }
                foreach (ImageFile imageFile in imageFiles)
                {
                    // await using (MagickImage image = await Task.Run(() => new MagickImage(imageFile.Path)))
                    using (MagickImage image = new MagickImage(imageFile.Path))
                    {
                        int pos = imageFile.Path.LastIndexOf(@"\");
                        StringBuilder path = new StringBuilder(imageFile.Path.Substring(pos));
                        if (ImageFormat.Jpeg == imageFormat)
                        {
                            // await image.Format = await Task.Run(() => MagickFormat.Jpeg);
                            image.Format = MagickFormat.Jpeg;
                            Image = image;
                            path.Remove(path.ToString().LastIndexOf("."), 5);
                            path.Append(".jpeg");
                        }
                        else if (ImageFormat.Png == imageFormat)
                        {
                            // await await Task.Run(() => MagickFormat.Png);
                            image.Format = MagickFormat.Png;
                            Image = image;
                            path.Remove(path.ToString().LastIndexOf("."), 5);
                            path.Append(".png");
                        }
                        // await  await Task.Run(() => image.Write(convertPath + path.ToString()));
                        image.Write(convertPath + path.ToString());
                    }
                    countPhoto--;
                    //if (countPhoto == 0)
                    //    progressBarValue = 100;
                    //await progressBarIncrease(progressBarValue);
                }
                if (!IsPathChose)
                    MessageBox.Show("Конвертация успешно завершена!Поскольку вы ранее не выбрали путь, все файлы были сохранены на рабочем столе!", "Успех!", MessageBoxButton.OK, MessageBoxImage.Information);
                else
                    MessageBox.Show("Конвертация успешно завершена!", "Успех!", MessageBoxButton.OK, MessageBoxImage.Information);
                //progressBar1.Value = 0;
                //CleanFilesButton.Visibility = Visibility.Visible;
            }
            catch (Exception exc)
            {
                MessageBox.Show($"При конвертировании произошла ошибка! {exc.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                //CleanFilesButton.Visibility = Visibility.Visible;
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
