using System.Windows;
using System.Windows.Input;

namespace Simple_HEIC_convertor
{
    /// <summary>
    /// Логика взаимодействия для PictureWindow.xaml
    /// </summary>
    public partial class PictureWindow : Window
    {
        public PictureWindow()
        {
            InitializeComponent();
            KeyDown += exit_picture_window;
        }
        private void exit_picture_window(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Close();
            }
        }
        //public void load_picture(string picturePath)
        //{
        //    try
        //    {
        //        using (MagickImage image = new MagickImage(picturePath))
        //        { 
        //            image.Format = MagickFormat.Jpeg;
        //            byte[] picture = image.ToByteArray();
        //            var ms = new MemoryStream(picture);
        //            var bitmap = new BitmapImage();
        //            bitmap.BeginInit();
        //            bitmap.CacheOption = BitmapCacheOption.OnLoad; // here
        //            bitmap.StreamSource = ms;
        //            bitmap.EndInit();
        //            WindowState = WindowState.Maximized;
        //            WindowStyle = WindowStyle.None;
        //            PictureContainer.Source = bitmap;
        //            PictureContainer.Height = Height;
        //            PictureContainer.Width = Width - 10;
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        MessageBox.Show($"Не удалось открыть изображение! Ошибка: {e.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        //    }
        //}
    }
}
