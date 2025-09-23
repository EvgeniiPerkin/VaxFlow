using System.Threading.Tasks;

namespace VaxFlow.DialogWindows
{
    public interface IDialogWindow
    {
        Task<bool> ShowDialogOkCancelAsync(string title, string message);
        Task<bool> ShowDialogYesNoAsync(string title, string message);
        Task<string[]?> ShowOpenFileAsync(string title, bool allowMultiple = false);
        Task<string[]?> ShowOpenFolderAsync(string title, bool allowMultiple = false);
    }
}
