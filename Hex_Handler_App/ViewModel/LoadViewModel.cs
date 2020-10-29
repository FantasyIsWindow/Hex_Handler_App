using Hex_Handler_App.Infrastructure.EventsArgs;
using Hex_Handler_App.Infrastructure.Services;
using System;
using System.Threading.Tasks;

namespace Hex_Handler_App.ViewModel
{
    internal class LoadViewModel : ModelBase
    {
        public event EventHandler CloseHandler;

        public void ExecuteAction(Action action)
        {
            Task.Factory.StartNew(action).ContinueWith(t =>
            {
                CloseHandler?.Invoke(this, EventArgs.Empty);
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        public void ExecuteAction<T>(Action<T> action, T parameter)
        {
            Task.Factory.StartNew(() => action(parameter)).ContinueWith(t =>
            {
                CloseHandler?.Invoke(this, EventArgs.Empty);
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        public void ExecuteFunc<T>(Func<T> func)
        {
            Task.Factory.StartNew(func).ContinueWith(t =>
            {
                var args = new PackageEventArgs() { Message = t };
                CloseHandler?.Invoke(this, args);
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }
    }
}
