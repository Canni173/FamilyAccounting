using Proconsult.Core;

namespace FamilyAccounting.Services
{
    public static class DialogService
    {
        public static ViewManager Manager { get; } = new ViewManager();
        public static void ShowDialog(ObservableObject model) => DispatcherService.Invoke(() => Manager.ShowDialog(model));
        public static void CloseDialog() => DispatcherService.Invoke(() => Manager.CloseDialog());
    }
}
