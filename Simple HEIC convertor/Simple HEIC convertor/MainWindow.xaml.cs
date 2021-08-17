using Microsoft.Win32;
using Simple_HEIC_convertor.Services;
using Simple_HEIC_convertor.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace Simple_HEIC_convertor
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<string> openPaths { get; set; }
        private List<string> deletePaths = new List<string> { };
        private readonly ICustomizationService _customizationService;

        public MainWindow()
        {
            bool isDarkTheme = false;
            string recivedTheme = Get_Current_Windows_Theme();
            
            if (recivedTheme == "Dark")
                isDarkTheme = true;
            _customizationService = new CustomizationService(new WindowService());

            DataContext = new ImageConvertorViewModel(new DialogService(), 
                new WorkerImageService(),
                _customizationService,
                isDarkTheme);

            AppDomain currentDomain;
            currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += CurrentDomain_UnhandledException;
            Closed += ClearWindows_Temp;
            Set_Start_Theme(recivedTheme);
            InitializeComponent();
        }

        private async void Set_Start_Theme(string recivedTheme)
        {
            if (recivedTheme == "Dark")
            {
                ThemeCheckBox.RaiseEvent(new RoutedEventArgs(CheckBox.CheckedEvent));
                ThemeCheckBox.IsChecked = true;
            }
            var colorRGB = await _customizationService.ReadColorSettingsFromJsonAsync();
            if (colorRGB != null)
                _customizationService.SetCustomRGBTheme(colorRGB);
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
    }
}
