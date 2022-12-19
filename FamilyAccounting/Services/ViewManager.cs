using FamilyAccounting.Views;
using MaterialDesignThemes.Wpf;
using Proconsult.Core;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace FamilyAccounting.Services
{
    public sealed class ViewManager
    {
        private readonly Stack<Window> _windows = new Stack<Window>();
        public Window CurrentDialog => _windows.Count == 0 ? null : _windows.Peek();
        public static DialogSession Session { get; private set; }
        private async Task ShowDialogAsync(FrameworkElement frameworkElement)
        {
            if (Session != null)
                if (!Session.IsEnded)
                    CloseDialog();//проблема с диалогом палоли при старте одноврименно откріваются с загрузкой
            await DialogHost.Show(frameworkElement, "ShellDialogHost", new DialogOpenedEventHandler((sender, args) =>
            {
                Session = args.Session;
            }));
        }
        public void CloseDialog() => DispatcherService.Invoke(() =>
        {
            if (!((bool)Session?.IsEnded))
                Session?.Close();
            CurrentDialog?.Close();
        });
        private async Task ShowDialogAsync<TUserControl>(ObservableObject model)
    where TUserControl : UserControl, new()
        {
            var userControl = new TUserControl
            {
                DataContext = model
            };

            try
            {
                //  model.InitializeDialog(userControl);
                // model.Activate(PageViewMode.Dialog);
                await ShowDialogAsync(userControl);



            }
            finally
            {

                //    model.Deactivate();
                //   model.OwnerWindow = null;
            }
        }
        public async void ShowDialog(ObservableObject model) => await ShowDialogAsync<DialogWiew>(model);

    }
}
