using ImageMagick;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Windows.Controls;
using System.Windows.Input;
using System.Diagnostics;
using System.IO;

namespace Simple_HEIC_convertor
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MagickImage Image;
        private double progressBarValue;
        private int countPhoto;
        private string convertPath { get; set; }
        private List<string> openPaths { get; set; }
        private List<string> deletePaths = new List<string> { };
        public MainWindow()
        {
            AppDomain currentDomain;
            currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += CurrentDomain_UnhandledException;
            Closed += ClearWindows_Temp;
            InitializeComponent();
        }


        private void ClearWindows_Temp(object sender, EventArgs e)
        {
            try
            {
                if (deletePaths.Count != 0)
                {
                    foreach (var tempPath in deletePaths)
                    {
                        File.Delete(tempPath);
                    }
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show($"При удалении временных файлов произошла ошибка! Пожалуйста очистите файлы изображений {@"в C:\Windows\Temp"} ошибка: {exc.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void get_files_from_explorer(object sender, DragEventArgs e)
        {
            try
            {
                openPaths = new List<string> { };
                string[] tempPaths = (string[])e.Data.GetData(DataFormats.FileDrop);
                openPaths.AddRange(tempPaths);
                bool isWasWrongFiles = false;
                int i = -1;
                List<int> delIndex = new List<int> { };
                foreach (string temp in openPaths)
                {
                    i++;
                    int pos = temp.LastIndexOf(@".");
                    if (!temp.Substring(pos).Equals(".heic"))
                    {
                        isWasWrongFiles = true;
                        delIndex.Add(i);
                        i--;
                    }
                }
                if (isWasWrongFiles)
                {
                    if (delIndex.Count == openPaths.Count)
                    {
                        MessageBox.Show("При перетаскивании были обнаружены неправельные файлы! Используйте .heic файлы!", "Обратите внимание!", MessageBoxButton.OK, MessageBoxImage.Information);
                        openPaths.Clear();
                        return;
                    }
                    foreach (int k in delIndex)
                    {
                        openPaths.RemoveAt(k);
                    }
                    MessageBox.Show("При перетаскивании были обнаружены файлы имеющие расширении не .heic! Были добавлены только .heic файлы", "Обратите внимание!", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                add_files_to_stackPannel();
            }
            catch (Exception exc)
            {
                MessageBox.Show($"При конвертировании произошла ошибка! {exc.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Clean_Click(object sender, RoutedEventArgs e)
        {
            FilesPanel.Children.Clear();
            openPaths.Clear();
            CleanFilesButton.Visibility = Visibility.Hidden;
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
            add_files_to_stackPannel();
        }

        void add_files_to_stackPannel()
        {
            if (openPaths != null)
            {
                if (openPaths[0] != "")
                {
                    foreach (var k in openPaths)
                    {
                        TextBox elem = new TextBox();
                        elem.Text = k;
                        elem.IsReadOnly = true;
                        elem.IsReadOnlyCaretVisible = true;
                        elem.Cursor = Cursors.Arrow;
                        elem.MouseDoubleClick += files_click_handler;
                        FilesPanel.Children.Add(elem);
                    }
                }
                CleanFilesButton.Visibility = Visibility.Visible;
            }
        }


        private async void files_click_handler(object sender, RoutedEventArgs e)
        {
            var current = (TextBox)e.Source;
            await openPicturesWindow(current.Text);
        }

        private async Task openPicturesWindow(string path)
        {
            try
            {
                using (MagickImage image = new MagickImage(path))
                {
                    image.Format = MagickFormat.Jpeg;
                    int pos = path.LastIndexOf(@"\");
                    StringBuilder reviewPath = new StringBuilder($@"C:\Windows\Temp{path.Substring(pos)}");
                    reviewPath.Remove(reviewPath.ToString().LastIndexOf("."), 5);
                    reviewPath.Append(".jpeg");
                    image.Write(reviewPath.ToString());
                    Process proc = Process.Start(reviewPath.ToString());
                    deletePaths.Add(reviewPath.ToString());
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show($"При предварительном просмотре произошла ошибка! {exc.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            await Task.Delay(5);
            // Через свое окно
            //PictureWindow pictureWindow = new PictureWindow();
            //pictureWindow.Show();
            //pictureWindow.load_picture(path);
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
            if (openPaths == null)
            {
                MessageBox.Show("Вы не выбрали файлы для конвертирования! Пожалуйста выберите файлы!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            else if (openPaths.Count == 0)
            {
                MessageBox.Show("Вы не выбрали файлы для конвертирования! Пожалуйста выберите файлы!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            else if (openPaths[0] == "")
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


        private async Task progressBarIncrease(double progressBarValue)
        {
            progressBar1.Value += progressBarValue;
            await Task.Delay(5);
        }

        private async Task converter()
        {
            bool IsPathChose = true;
            if (!(bool)RadioButton1.IsChecked && !(bool)RadioButton2.IsChecked)
            {
                MessageBox.Show("Вы не выбрали формат! Пожалуйста выберите формат!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            try
            {
                countPhoto = openPaths.Count;
                progressBarValue = 100 / countPhoto;
                if(convertPath == null || convertPath == "")
                {
                    convertPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                    IsPathChose = false;
                }
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
                if(!IsPathChose)
                    MessageBox.Show("Конвертация успешно завершена!Поскольку вы ранее не выбрали путь, все файлы были сохранены на рабочем столе!", "Успех!", MessageBoxButton.OK, MessageBoxImage.Information);
                else
                    MessageBox.Show("Конвертация успешно завершена!", "Успех!", MessageBoxButton.OK, MessageBoxImage.Information);
                progressBar1.Value = 0;
            }
            catch (Exception exc)
            {
                MessageBox.Show($"При конвертировании произошла ошибка! {exc.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            await Task.Delay(1);
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Разработал: isergei151@gmail.com\nGitHub: https://github.com/sub13/WPF-Simple-HEIC-Convertor", "Контакты!", MessageBoxButton.OK, MessageBoxImage.Information);
        }


        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception Ex = (Exception)e.ExceptionObject;
            MessageBox.Show($"При запуске произошла ошибка! {Ex.TargetSite} {Ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

    }
}
