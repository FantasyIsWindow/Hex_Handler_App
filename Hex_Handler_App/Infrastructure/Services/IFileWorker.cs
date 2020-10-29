namespace Hex_Handler_App.Infrastructure.Services
{
    public interface IFileWorker
    {
        public string OpenFileDialog();

        public string GetFileData(string filePath);

        public bool SaveData(string data);
    }
}
