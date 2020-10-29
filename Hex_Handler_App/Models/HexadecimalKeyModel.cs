using Hex_Handler_App.Infrastructure.Services;

namespace Hex_Handler_App.Models
{
    public class HexadecimalKeyModel : ModelBase
    {
        private int _number;

        private string _keyValue;

        private string _hashValue;

        public int Number 
        {
            get => _number;
            set => SetProperty(ref _number, value, "Number");
        }

        public string KeyValue
        {
            get => _keyValue;
            set => SetProperty(ref _keyValue, value, "KeyValue");
        }

        public string HashValue
        {
            get => _hashValue;
            set => SetProperty(ref _hashValue, value, "HashValue");
        }
    }
}
