using Ookii.Dialogs.Wpf;
using System.IO;
using System.Text;

namespace Hex_Handler_App.Infrastructure.Services
{
    internal class FileWorker : IFileWorker
    {
        private const string FILE_PATTERN = "*.hex|*.hex";

        public string OpenFileDialog()
        {
            VistaOpenFileDialog fileDialog = new VistaOpenFileDialog
            {
                Filter = FILE_PATTERN,
                FilterIndex = 2
            };
            if (fileDialog.ShowDialog() == true)
            {
                return fileDialog.FileName;
            }
            return null;
        }

        public string GetFileData(string filePath)
        {
            using (FileStream fsStream = File.OpenRead(filePath))
            {
                using (StreamReader reader = new StreamReader(fsStream))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        public bool SaveData(string data)
        {
            VistaSaveFileDialog saveDialog = new VistaSaveFileDialog
            {
                Filter = FILE_PATTERN,
                FilterIndex = 2
            };
            if (saveDialog.ShowDialog() == true)
            {
                if(!Path.HasExtension(saveDialog.FileName))
                {
                    saveDialog.FileName += ".hex";
                }
                using (FileStream stream = new FileStream(saveDialog.FileName, FileMode.OpenOrCreate))
                {
                    byte[] arr = Encoding.Default.GetBytes(data);
                    stream.Write(arr, 0, arr.Length);
                }
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
