using ImageMagick;
using Microsoft.Win32;
using Simple_HEIC_convertor.Services;
using Simple_HEIC_convertor.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Simple_HEIC_convertor
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<string> openPaths { get; set; }
        private List<string> deletePaths = new List<string> { };
        public MainWindow()
        {
            bool isDarkTheme = false;
            string recivedTheme = Get_Current_Windows_Theme();
            
            if (recivedTheme == "Dark")
                isDarkTheme = true;

            DataContext = new ImageConvertorViewModel(new DialogService(), new WorkerImageService(), isDarkTheme);
            AppDomain currentDomain;
            currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += CurrentDomain_UnhandledException;
            Closed += ClearWindows_Temp;
            InitializeComponent();
            Set_Start_Theme(recivedTheme);
        }

        private void Set_Start_Theme(string recivedTheme)
        {
            if (recivedTheme == "Dark")
            {
                ThemeCheckBox.RaiseEvent(new RoutedEventArgs(CheckBox.CheckedEvent));
                ThemeCheckBox.IsChecked = true;
            }
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

        //private void set_active_files_drop_menu(object sender, DragEventArgs e)
        //{
        //    FilesPanel.Background = new SolidColorBrush(Color.FromArgb(90, 0xF0, 0x00, 0xFF));
        //    new SolidColorBrush(Colors.AliceBlue);
        //    SystemColors.ActiveBorderBrush;
        //}

        //private void set_desabled_files_drop_menu(object sender, DragEventArgs e)
        //{
        //    FilesPanel.Background = new SolidColorBrush(Colors.GhostWhite);
        //}

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
            //FilesPanel.Children.Clear();
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
                        //FilesPanel.Children.Add(elem);
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
                using (MagickImage image = await Task.Run(() => new MagickImage(path)))
                {
                    image.Format = await Task.Run(() => MagickFormat.Jpeg);
                    int pos = path.LastIndexOf(@"\");
                    StringBuilder reviewPath = new StringBuilder($@"C:\Windows\Temp{path.Substring(pos)}");
                    reviewPath.Remove(reviewPath.ToString().LastIndexOf("."), 5);
                    reviewPath.Append(".jpeg");
                    await Task.Run(() => image.Write(reviewPath.ToString()));
                    Process proc = await Task.Run(() => Process.Start(reviewPath.ToString()));
                    deletePaths.Add(reviewPath.ToString());
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show($"При предварительном просмотре произошла ошибка! {exc.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            // Через свое окно
            //PictureWindow pictureWindow = new PictureWindow();
            //pictureWindow.Show();
            //pictureWindow.load_picture(path);
        }


        private void Button_Click_2(object sender, RoutedEventArgs e)
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
        }

        // Отслеживание темы
        private string Get_Current_Windows_Theme()
        {
            
            // Получение темы
            string RegistryKeyPath = @"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize";

            string RegistryValueName = "AppsUseLightTheme";

            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(RegistryKeyPath))
            {
                object registryValueObject = key?.GetValue(RegistryValueName);
                if (registryValueObject == null)
                {
                    return "Light";
                }

                int registryValue = (int)registryValueObject;

                return registryValue > 0 ? "Light" : "Dark";
            }

            // отслеживание темы
            //try
            //{
            //    var watcher = new ManagementEventWatcher(query);
            //    watcher.EventArrived += (sender, args) =>
            //    {
            //        string newWindowsTheme = Get_Current_Windows_Theme(RegistryKeyPath, RegistryValueName);
            //        if (newWindowsTheme == "Dark")
            //        {

            //            ThemeCheckBox.RaiseEvent(new RoutedEventArgs(CheckBox.CheckedEvent));
            //        }
            //        else
            //        {
            //            ThemeCheckBox.RaiseEvent(new RoutedEventArgs(CheckBox.UncheckedEvent));
            //        }
            //        // React to new theme
            //    };

            //    // Start listening for events
            //    watcher.Start();
            //}
            //catch (Exception)
            //{
            //    This can fail on Windows 7
            //    MessageBox.Show("Что-то произошло не так! Не удалось установить тему автоматически под систему");
            //}
        }


        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception Ex = (Exception)e.ExceptionObject;
            MessageBox.Show($"При запуске произошла ошибка! {Ex.TargetSite} {Ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void progressBar1_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }
    }
}
