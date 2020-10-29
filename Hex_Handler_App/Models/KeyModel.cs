using System;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.RegularExpressions;

namespace Hex_Handler_App.Models
{
    internal class KeyModel : MainModel
    {
        public KeyModel(ObservableCollection<HexadecimalKeyModel> hexModels) : base(hexModels) { }

        public override string PreparingDataForSaving()
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < _hexModels.Count; i++)
            {
                builder.Append($"{_hexModels[i].Number} - {_hexModels[i].KeyValue}\n");
            }

            return builder.ToString();
        }

        public override void ReadAndParsingData(string fileData)
        {
            if (Regex.IsMatch(fileData, @"\d+ - [A-Z0-9]+\n"))
            {
                var arr = fileData.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < arr.Length; i++)
                {
                    _context.Invoke(() => _hexModels.Add(new HexadecimalKeyModel()
                    {
                        Number = i + 1,
                        KeyValue = arr[i].Substring(arr[i].Length - 10)
                    }));
                }
            }
            else
            {
                fileData = fileData.Replace("\r", "").Replace("\n", "");
                for (int i = 0, n = 1; i < fileData.Length; i += 12, n++)
                {
                    _context.Invoke(() => _hexModels.Add(new HexadecimalKeyModel()
                    {
                        Number = n,
                        KeyValue = fileData.Substring(i, 10)
                    }));
                }
            }
        }
    }
}
