using System;
using System.Windows;
using System.Windows.Threading;

namespace FamilyAccounting.Services
{
    public static class DispatcherService
    {
        public static void Invoke(Action action) => Application.Current?.Dispatcher.Invoke(action);
        public static void BeginInvoke<T>(Action<T> action, T arg, DispatcherPriority priority = DispatcherPriority.Normal)
        {
            Application.Current.Dispatcher.BeginInvoke(priority, action, arg);
        }

    }
}
