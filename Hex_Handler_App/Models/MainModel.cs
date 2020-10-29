using Hex_Handler_App.Infrastructure.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Hex_Handler_App.Models
{
    abstract internal class MainModel
    {
        protected ObservableCollection<HexadecimalKeyModel> _hexModels;

        protected IContext _context;

        public MainModel(ObservableCollection<HexadecimalKeyModel> hexModels)
        {
            _hexModels = hexModels;
            _context = new WpfDispatcherContext();
        }

        public abstract string PreparingDataForSaving();

        public abstract void ReadAndParsingData(string fileData);

        public HashSet<HexadecimalKeyModel> GetDuplicates()
        {
            HashSet<HexadecimalKeyModel> duplicatesArr = new HashSet<HexadecimalKeyModel>();
            for (int i = 0; i < _hexModels.Count; i++)
            {
                for (int f = i + 1; f < _hexModels.Count; f++)
                {
                    if (_hexModels[i].KeyValue == _hexModels[f].KeyValue)
                    {
                        duplicatesArr.Add(_hexModels[f]);
                    }
                }
            }
            return duplicatesArr;
        }

        public void DeleteDuplicates(HashSet<HexadecimalKeyModel> duplicatesCol)
        {
            foreach (var item in duplicatesCol)
            {
                _context.Invoke(() => _hexModels.Remove(item));
            }

            int newNumber = 1;
            foreach (var item in _hexModels)
            {
                item.Number = newNumber++;
            }
        }
    }
}
