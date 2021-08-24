using Simple_HEIC_convertor.Services.Interfaces;
using System;
using System.Windows;

namespace Simple_HEIC_convertor.Services
{
    public class WindowService : IWindowService 
    {
        public void CreateWindowAndShow(object viewModel, string classNameWindow)
        {
            Type typeWindow = Type.GetType(classNameWindow);
            Window window = (Window)Activator.CreateInstance(typeWindow);
            window.DataContext = viewModel;
            window.Show();
        }
    }
}
