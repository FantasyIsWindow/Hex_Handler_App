using System;
using System.Windows.Threading;

namespace Hex_Handler_App.Infrastructure.Services
{
    internal sealed class WpfDispatcherContext : IContext
    {
        private readonly Dispatcher _dispatcher;

        public WpfDispatcherContext() : this(Dispatcher.CurrentDispatcher) { }

        public WpfDispatcherContext(Dispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }
        
        public void Invoke(Action action)
        {
            try
            {
                _dispatcher.Invoke(action);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void BeginInvoke(Action action)
        {
            try
            {
                _dispatcher.BeginInvoke(action);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
