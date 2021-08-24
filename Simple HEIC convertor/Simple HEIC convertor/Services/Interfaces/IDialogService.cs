using System.Collections.Generic;

namespace Simple_HEIC_convertor.Services.Interfaces
{
    public interface IDialogService
    {
        void ShowMessage(string message);
        public string ConvertPath { get; set; }
        public List<string> OpenPaths { get; set; }
        public List<string> DeletePaths { get; set; } 
        bool OpenFileDialog();
        bool SaveFileDialog(); 
    }
}
