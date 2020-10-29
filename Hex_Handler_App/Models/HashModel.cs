using System;
using System.Collections.ObjectModel;
using System.Text;

namespace Hex_Handler_App.Models
{
    internal class HashModel : MainModel
    {
        public HashModel(ObservableCollection<HexadecimalKeyModel> hexModels) : base(hexModels) { }

        public override string PreparingDataForSaving()
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < _hexModels.Count; i++)
            {
                builder.Append($":{_hexModels[i].KeyValue}{_hexModels[i].HashValue}\n");
            }

            return builder.ToString();
        }

        public override void ReadAndParsingData(string fileData)
        {
            var arr = fileData.Split(new char[] { ':', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < arr.Length; i++)
            {
                _context.Invoke(() => _hexModels.Add(new HexadecimalKeyModel()
                {
                    Number = i + 1,
                    KeyValue = arr[i].Remove(arr[i].Length - 2, 2),
                    HashValue = arr[i].Substring(arr[i].Length - 2, 2)
                }));
            }
        }
    }
}
