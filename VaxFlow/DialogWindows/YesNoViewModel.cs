using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace VaxFlow.DialogWindows
{
    public partial class YesNoViewModel : ObservableValidator
    {
        public YesNoViewModel(string title, string message)
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
        public void YesCommand(object parameter)
        {
            CloseDialog?.Invoke(true);
        }
        [RelayCommand]
        public void NoCommand(object parameter)
        {
            CloseDialog?.Invoke(false);
        }
    }
}
