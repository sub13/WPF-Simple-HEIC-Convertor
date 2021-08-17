using Simple_HEIC_convertor.Commands;
using Simple_HEIC_convertor.Enums;
using Simple_HEIC_convertor.Models;
using Simple_HEIC_convertor.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;

namespace Simple_HEIC_convertor.ViewModels
{
    class ImageConvertorViewModel : INotifyPropertyChanged
    {
        private string _convertPath;

        private ImageFormat  _imageFormat = ImageFormat.Jpeg;
        private ObservableCollection<ImageFile> _allFilePaths = new ObservableCollection<ImageFile>();
        private ImageFile _selectedImage;
        private ColorSchemes _selectedColorScheme = ColorSchemes.Green;

        private bool _isConvertingStart = false;
        private bool _isDarkTheme;
        private bool _isCleanButtonVisible = false;

        private readonly IDialogService _dialogService;
        private readonly IWorkerImageService _workerImageService;
        private readonly ICustomizationService _customizationService;

        public ImageConvertorViewModel(IDialogService dialogService, 
            IWorkerImageService workerImageService, 
            ICustomizationService customizationService,
            bool isDarkTheme = false)
        {
            _dialogService = dialogService;
            _workerImageService = workerImageService;
            IsDarkTheme = isDarkTheme;
            _customizationService = customizationService;
        }

        public ColorSchemes SelectedColorScheme 
        { 
            get 
            { 
                return _selectedColorScheme; 
            } 
            set 
            {
                _selectedColorScheme = value;
                OnPropertyChanged("SelectedColorScheme");
            } 
        }

        public bool IsDarkTheme
        {
            get
            {
                return _isDarkTheme;
            }
            set
            {
                _isDarkTheme = value;
                OnPropertyChanged("IsDarkTheme");
            }
        }

        public bool IsCleanButtonVisible
        {
            get
            {
                return _isCleanButtonVisible;
            }
            set
            {
                if (_allFilePaths.Count > 0)
                    _isCleanButtonVisible = true;
                else
                    _isCleanButtonVisible = false;
                OnPropertyChanged("IsCleanButtonVisible");
            }
        }

        public bool IsConvertingStart
        {
            get
            {
                return _isConvertingStart;
            }
            set
            {
                _isConvertingStart = value;
                OnPropertyChanged("IsConvertingStart");
            }
        }

        public ImageFormat SelectedImageFormat
        {
            get 
            { 
                return _imageFormat; 
            }
            set 
            {
                if (_imageFormat == value)
                    return;
                _imageFormat = value;
                OnPropertyChanged("SelectedImageFormat");
                OnPropertyChanged("IsJpgFormat");
                OnPropertyChanged("IsPngFormat");
            }
        }

        public ImageFile SelectedImage
        {
            get
            {
                return _selectedImage;
            }
            set
            {
                if (value == _selectedImage)
                    return;
                _selectedImage = value;
                OnPropertyChanged("SelectedImage");
            }
        }

        public ObservableCollection<ImageFile> AllFilePaths 
        {
            get { return _allFilePaths; }
            set 
            {
                if (value == _allFilePaths)
                    return;
                _allFilePaths = value;
                OnPropertyChanged("AllFilePaths");
            } 
        }        

        public bool IsJpgFormat
        {
            get
            {
                return SelectedImageFormat == ImageFormat.Jpeg;
            }
            set
            {
                SelectedImageFormat = value ? ImageFormat.Jpeg : SelectedImageFormat;
            }
        }

        public bool IsPngFormat
        {
            get
            {
                return SelectedImageFormat == ImageFormat.Png;
            }
            set
            {
                SelectedImageFormat = value ? ImageFormat.Png : SelectedImageFormat;
            }
        }

        public string SelectedConvertPath
        {
            get { return _convertPath; }
            set
            {
                _convertPath = value;
                OnPropertyChanged("SelectedConvertPath");
            }
        }

        private FileImagesCommand _changeColorSchemeCommand;
        public FileImagesCommand ChangeColorSchemeCommand
        {
            get
            {
                return _changeColorSchemeCommand ??
                    (_changeColorSchemeCommand = new FileImagesCommand(obj =>
                    {
                        _customizationService.ChangeColorScheme(SelectedColorScheme);
                    }));
            }
        }

        private FileImagesCommand _setDarkThemeCommand;
        public FileImagesCommand SetDarkThemeCommand
        {
            get
            {
                return _setDarkThemeCommand ??
                    (_setDarkThemeCommand = new FileImagesCommand(obj =>
                    {
                        _customizationService.Set_Theme(IsDarkTheme);
                    }));
            }
        }

        private FileImagesCommand _openImagePreviewCommand;
        public FileImagesCommand OpenImagePreviewCommand
        {
            get
            {
                return _openImagePreviewCommand ??
                    (_openImagePreviewCommand = new FileImagesCommand(obj =>
                    {
                        if (!String.IsNullOrEmpty(SelectedImage.Path))
                        {
                            Thread thread = new Thread(() => _workerImageService.OpenHEICImage(SelectedImage.Path));
                            thread.Start();
                        }
                    }));
            }
        }

        private FileImagesCommand _openImagesCommand;
        public FileImagesCommand OpenImagesCommand
        {
            get
            {
                return _openImagesCommand ??
                    (_openImagesCommand = new FileImagesCommand(obj =>
                    {
                        if (_dialogService.OpenFileDialog() == true)
                        {
                            _allFilePaths.Clear();
                            _dialogService.OpenPaths.ForEach(p =>
                            _allFilePaths.Add(new ImageFile()
                            {
                                Path = p
                            }
                            ));
                            IsCleanButtonVisible = true;
                        }
                    }));
            }
        }

        private FileImagesCommand _dranAndDropFilesCommand;
        public FileImagesCommand DranAndDropFilesCommand
        {
            get
            {
                return _dranAndDropFilesCommand ??
                    (_dranAndDropFilesCommand = new FileImagesCommand(obj =>
                    {
                        var eventArgs = (DragEventArgs)obj;
                        string[] pathsFromDragEventArgs = (string[])eventArgs.Data.GetData(DataFormats.FileDrop);
                        CheckDragAndDropPathsAndAddToAllPaths(pathsFromDragEventArgs);
                    }));
            }
        }

        private void CheckDragAndDropPathsAndAddToAllPaths(string[] pathsFromDragEventArgs)
        {
            try
            {
                AllFilePaths.Clear();
                bool isWasWrongFiles = false;
                int i = -1;
                List<int> delCounter = new List<int> { };
                foreach (string dragPath in pathsFromDragEventArgs)
                {
                    i++;
                    int pos = dragPath.LastIndexOf(@".");
                    if (!dragPath.Substring(pos).Equals(".heic"))
                    {
                        isWasWrongFiles = true;
                        delCounter.Add(i);
                        i--;
                    }
                    else
                    {
                        AllFilePaths.Add(new ImageFile()
                        {
                            Path = dragPath
                        });
                    }
                }
                if (isWasWrongFiles)
                {
                    if (AllFilePaths.Count > 0)
                    {
                        IsCleanButtonVisible = true;
                        MessageBox.Show("При перетаскивании были обнаружены файлы имеющие расширении не .heic! Были добавлены только .heic файлы", "Обратите внимание!", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Неправильный файл. Поддерживется только .heic!", "Обратите внимание!", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                IsCleanButtonVisible = true;
            }
            catch (Exception exc)
            {
                MessageBox.Show($"При перетаскивании произошла ошибка! {exc.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private FileImagesCommand _openHelpCommand;
        public FileImagesCommand OpenHelpCommand
        {
            get
            {
                return _openHelpCommand ??
                    (_openHelpCommand = new FileImagesCommand(obj =>
                    {
                        MessageBox.Show("Разработал: isergei151@gmail.com\nGitHub: https://github.com/sub13/WPF-Simple-HEIC-Convertor", "Контакты!", MessageBoxButton.OK, MessageBoxImage.Information);
                    }));
            }
        }

        private FileImagesCommand _clearFilePathsCommand;
        public FileImagesCommand ClearFilePathsCommand
        {
            get
            {
                return _clearFilePathsCommand ??
                    (_clearFilePathsCommand = new FileImagesCommand(obj =>
                    {
                        AllFilePaths.Clear();
                        IsCleanButtonVisible = false;
                    }));
            }
        }

        private FileImagesCommand _setConvertPathCommand;
        public FileImagesCommand SetConvertPathCommand
        {
            get
            {
                return _setConvertPathCommand ??
                    (_setConvertPathCommand = new FileImagesCommand(obj =>
                    {

                        if (_dialogService.SaveFileDialog() == true)
                        {
                            SelectedConvertPath = _dialogService.ConvertPath;
                        }
                    }));
            }
        }

        private FileImagesCommand _convertImageCommand;
        public FileImagesCommand ConvertImageCommand
        {
            get
            {
                return _convertImageCommand ??
                    (_convertImageCommand = new FileImagesCommand(obj =>
                    {
                        IsConvertingStart = true;
                        Thread thread = new Thread(() => {
                            _workerImageService.ConvertToHEIC(AllFilePaths, _imageFormat, _convertPath);
                            IsConvertingStart = false;
                            });
                        thread.Start();
                    }));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

    }
}
