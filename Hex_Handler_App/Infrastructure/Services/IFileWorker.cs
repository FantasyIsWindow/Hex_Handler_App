namespace Hex_Handler_App.Infrastructure.Services
{
    public interface IFileWorker
    {
        /// <summary>
        /// Open file dialog
        /// </summary>
        /// <returns>Path to file</returns>
        public string OpenFileDialog();

        /// <summary>
        /// Read file data
        /// </summary>
        /// <param name="filePath">Path to file</param>
        /// <returns>File data</returns>
        public string GetFileData(string filePath);

        /// <summary>
        /// Open save dialog and save data to file
        /// </summary>
        /// <param name="data">Data to save</param>
        /// <returns>Operation success report</returns>
        public bool SaveData(string data);
    }
}
