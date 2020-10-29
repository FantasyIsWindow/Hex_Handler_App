using System;

namespace Hex_Handler_App.Infrastructure.EventsArgs
{
    internal class PackageEventArgs : EventArgs
    {
        public object Message { get; set; }
    }
}
