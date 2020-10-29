using System;

namespace Hex_Handler_App.Infrastructure.Services
{
    public interface IContext
    {
        void Invoke(Action action);

        void BeginInvoke(Action action);
    }
}
