using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Windows;

namespace Simple_HEIC_convertor.Services
{
    public class DialogService : IDialogService
    {
        public string ConvertPath { get; set; }
        public List<string> OpenPaths { get; set; }
        public List<string> DeletePaths { get; set; }

        public bool OpenFileDialog()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image files(*.heic)|*.heic";
            ofd.CheckFileExists = true;
            ofd.Multiselect = true;
            try
            {
                if (ofd.ShowDialog() == true)
                {
                    OpenPaths = new List<string> { };
                    foreach (var k in ofd.FileNames)
                    {
                        OpenPaths.Add(k);
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                MessageBox.Show("При выборе файлов произошла не предвиденная ошибка", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        public bool SaveFileDialog()
        {
            CommonOpenFileDialog dfd = new CommonOpenFileDialog();
            dfd.Title = "Путь для конвертирования...";
            dfd.IsFolderPicker = true;
            dfd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            try
            {
                if (dfd.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    ConvertPath = dfd.FileName;
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch
            {
                MessageBox.Show("При выборе пути произошла не предвиденная ошибка!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                MessageBox.Show("Выбран путь по умолчанию! (Рабочий стол пользователя)", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                ConvertPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                return false;
            }
        }

        public void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }
    }
}
