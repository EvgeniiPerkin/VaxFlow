using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;
using System;
using System.Linq;
using System.Threading.Tasks;
using VaxFlow.Services;

namespace VaxFlow.DialogWindows
{
    public class DialogWindow : IDialogWindow
    {
        public async Task<bool> ShowDialogOkCancelAsync(string title, string message)
        {
            var vm = new OkCancelViewModel(title, message);
            OkCancelWindow window = new()
            {
                DataContext = vm
            };
            vm.CloseDialog += window.Close;
            return await window.ShowDialog<bool>(GetActiveWindow()??throw new Exception("No active window found!"));
        }
        public async Task<bool> ShowDialogYesNoAsync(string title, string message)
        {
            var vm = new YesNoViewModel(title, message);
            YesNoWindow window = new()
            {
                DataContext = vm
            };
            vm.CloseDialog += window.Close;
            return await window.ShowDialog<bool>(GetActiveWindow() ?? throw new Exception("No active window found!"));
        }
        public async Task<string[]?> ShowOpenFileAsync(string title, bool allowMultiple = false)
        {
            var topLevel = TopLevel.GetTopLevel(GetActiveWindow());

            IStorageFolder? customFolder = await topLevel!.StorageProvider.TryGetFolderFromPathAsync(AppConfiguration.GetHomeDirectory());

            var storageFiles = await topLevel!.StorageProvider.OpenFilePickerAsync(
                new FilePickerOpenOptions()
                {
                    AllowMultiple = allowMultiple,
                    Title = title,
                    FileTypeFilter = [FilePickerFileTypes.TextPlain], // [XmlPlain]
                    SuggestedStartLocation = customFolder,
                    SuggestedFileName = " name_file.txt"
                });

            return storageFiles?.Select(x => x.Path.LocalPath)?.ToArray();
        }
        public async Task<string[]?> ShowOpenFolderAsync(string title, bool allowMultiple = false)
        {
            var topLevel = TopLevel.GetTopLevel(GetActiveWindow());

            IStorageFolder? customFolder = await topLevel!.StorageProvider.TryGetFolderFromPathAsync(AppConfiguration.GetHomeDirectory());

            var storageFiles = await topLevel!.StorageProvider.OpenFolderPickerAsync(
                new FolderPickerOpenOptions()
                {
                    AllowMultiple = allowMultiple,
                    Title = title,
                    SuggestedStartLocation = customFolder,
                    SuggestedFileName = " name_file.txt"
                });

            return storageFiles?.Select(x => x.Path.LocalPath)?.ToArray();
        }
        public static FilePickerFileType XmlPlain { get; } = new("Plain xml")
        {
            Patterns = ["*.xml"],
            AppleUniformTypeIdentifiers = ["public.plain-xml"],
            MimeTypes = ["xml/plain"]
        };
        private static Window? GetActiveWindow()
        {
            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                return desktop.MainWindow;
            }

            return new Window
            {
                CanResize = false,
                CanMinimize = false,
                CanMaximize = false,
                Width = 400,
                Height = 200
            };
        }
    }
}
