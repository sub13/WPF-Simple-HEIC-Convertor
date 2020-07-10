using ImageMagick;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace Simple_HEIC_convertor
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MagickImage Image;
        private int progressBarValue;
        private int countPhoto;
        private string convertPath { get; set; }
        private List<string> openPaths { get; set; }
        public MainWindow()
        {
            AppDomain currentDomain;
            currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += CurrentDomain_UnhandledException;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image files(*.heic)|*.heic";
            ofd.CheckFileExists = true;
            ofd.Multiselect = true;
            if (ofd.ShowDialog() == true)
            {
                try
                {
                    openPaths = new List<string> { };
                    foreach(var k in ofd.FileNames)
                    {
                        openPaths.Add(k);
                    }
                }
                catch
                {
                    MessageBox.Show("При выборе файлов произошла не предвиденная ошибка", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog dfd = new CommonOpenFileDialog();
            dfd.Title = "Путь для конвертирования...";
            dfd.IsFolderPicker = true;
            dfd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            if (dfd.ShowDialog() == CommonFileDialogResult.Ok)
            {
                try
                {
                    convertPath = dfd.FileName;
                }
                catch
                {
                    MessageBox.Show("При выборе пути произошла не предвиденная ошибка!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    MessageBox.Show("Выбран путь по умолчанию! (Рабочий стол пользователя)", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    convertPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                }
            }
        }

        private async void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (openPaths == null || openPaths[0] == "")
            {
                MessageBox.Show("Вы не выбрали файлы для конвертирования! Пожалуйста выберите файлы!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            //if (!Application.Current.Dispatcher.CheckAccess())
            //{
                //Application.Current.Dispatcher.Invoke(new Action(() => converter())); // реализация через поток
                //Application.Current.Dispatcher.Invoke(new Action(() => progressBarIncrease()));
            //}
            //Task<Location> locTask = location.GetCurrentLocationAsync(); // реализация через ассинхроность
            //await progressBarIncrease();
            await converter();
        }


        private async Task progressBarIncrease(int progressBarValue)
        {
            progressBar1.Value += progressBarValue;
            await Task.Delay(5);
        }

        private async Task converter()
        {
            if (!(bool)RadioButton1.IsChecked && !(bool)RadioButton2.IsChecked)
            {
                MessageBox.Show("Вы не выбрали формат! Пожалуйста выберите формат!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            try
            {
                countPhoto = openPaths.Count;
                progressBarValue = 100 / countPhoto;
                foreach (string k in openPaths)
                {
                    using (MagickImage image = new MagickImage(k))
                    {
                        int pos = k.LastIndexOf(@"\");
                        StringBuilder path = new StringBuilder(k.Substring(pos));
                        if ((bool)RadioButton1.IsChecked)
                        {
                            image.Format = MagickFormat.Jpeg;
                            Image = image;
                            path.Remove(path.ToString().LastIndexOf("."), 5);
                            path.Append(".jpeg");
                        }
                        else if ((bool)RadioButton2.IsChecked)
                        {
                            image.Format = MagickFormat.Png;
                            Image = image;
                            path.Remove(path.ToString().LastIndexOf("."), 5);
                            path.Append(".png");
                        }
                        image.Write(convertPath + path.ToString());
                    }
                    countPhoto--;
                    if (countPhoto == 0)
                        progressBarValue = 100;
                    await progressBarIncrease(progressBarValue);
                }
                MessageBox.Show("Конвертация успешно завершена!", "Успех!", MessageBoxButton.OK, MessageBoxImage.Information);
                progressBar1.Value = 0;
            }
            catch (Exception exc)
            {
                MessageBox.Show($"При конвертировании произошла ошибка! {exc.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            await Task.Delay(5);
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Разработал: isergei151@gmail.com", "Контакты!", MessageBoxButton.OK, MessageBoxImage.Information);
        }


        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception Ex = (Exception)e.ExceptionObject;
            MessageBox.Show($"При запуске произошла ошибка! {Ex.TargetSite} {Ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
