using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace VaxFlow.DialogWindows
{
    public delegate void OnCloseDialog(object? dialogResult);
    public partial class OkCancelViewModel : ObservableValidator
    {
        public OkCancelViewModel(string title, string message)
        {
            Title = title;
            Message = message;
        }

        [ObservableProperty]
        private string? _Title;
        [ObservableProperty]
        private string? _Message;

        public event OnCloseDialog? CloseDialog;

        [RelayCommand]
        public void OkCommand(object parameter)
        {
            CloseDialog?.Invoke(true);
        }
        [RelayCommand]
        public void CancelCommand(object parameter)
        {
            CloseDialog?.Invoke(false);
        }
    }
}
