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

        /// <summary>
        /// Preparing data for saving
        /// </summary>
        /// <returns>Processed data</returns>
        public abstract string PreparingDataForSaving();

        /// <summary>
        /// Read and parsing data
        /// </summary>
        /// <param name="fileData">Processed data</param>
        public abstract void ReadAndParsingData(string fileData);

        /// <summary>
        /// Search for duplicates in the collection
        /// </summary>
        /// <returns>Collection of duplicates</returns>
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

        /// <summary>
        /// Removing duplicates from the original collection
        /// </summary>
        /// <param name="duplicatesCol">Collection of duplicates to delete</param>
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
