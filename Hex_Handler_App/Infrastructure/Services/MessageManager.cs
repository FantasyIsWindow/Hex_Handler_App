using System.Windows;

namespace Hex_Handler_App.Infrastructure.Services
{
    internal class MessageManager
    {
        /// <summary>
        /// Show message box with OK button
        /// </summary>
        /// <param name="message">Message to display</param>
        public void ShowOkMessage(string message) =>
            MessageBox.Show(message, "Message", MessageBoxButton.OK);

        /// <summary>
        /// Show message box with Yes and No buttons
        /// </summary>
        /// <param name="message"></param>
        /// <returns>Message to display</returns>
        public bool ShowYesNoMessage(string message) =>
            MessageBox.Show(message, "Message", MessageBoxButton.YesNo) == MessageBoxResult.Yes;
    }
}
