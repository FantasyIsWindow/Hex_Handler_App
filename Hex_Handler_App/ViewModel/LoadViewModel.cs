using Hex_Handler_App.Infrastructure.EventsArgs;
using Hex_Handler_App.Infrastructure.Services;
using System;
using System.Threading.Tasks;

namespace Hex_Handler_App.ViewModel
{
    internal class LoadViewModel : ModelBase
    {
        public event EventHandler CloseHandler;

        /// <summary>
        /// Executing a function with a single parameter
        /// </summary>
        /// <typeparam name="T">Type of parameter</typeparam>
        /// <param name="action">Execution method</param>
        /// <param name="parameter">Method parameter</param>
        public void ExecuteAction<T>(Action<T> action, T parameter)
        {
            Task.Factory.StartNew(() => action(parameter)).ContinueWith(t =>
            {
                CloseHandler?.Invoke(this, EventArgs.Empty);
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        /// <summary>
        /// Executing the method and returning the result
        /// </summary>
        /// <typeparam name="T">Type of returned result</typeparam>
        /// <param name="func">Execution method</param>
        public void ExecuteFunc<T>(Func<T> func)
        {
            Task.Factory.StartNew(func).ContinueWith(t =>
            {
                var args = new PackageEventArgs() { Message = t };
                CloseHandler?.Invoke(this, args);
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        /// <summary>
        /// Resetting subscriptions
        /// </summary>
        public void ResettingSubscriptions()
        {
            if (CloseHandler != null)
            {
                CloseHandler = null;
            }
        }
    }
}
