using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace VaxFlow.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {
            Items = new ObservableCollection<ListItemTemplate>(_templates);
            SelectedListItem = Items.First(vm => vm.ModelType == typeof(HomeViewModel));
        }

        [ObservableProperty]
        private bool _isPaneOpen;

        [ObservableProperty]
        private ViewModelBase? _currentPage;

        [ObservableProperty]
        private ListItemTemplate? _SelectedListItem;

        partial void OnSelectedListItemChanged(ListItemTemplate? value)
        {
            if (value is null) return;

            var vm = Design.IsDesignMode
                ? Activator.CreateInstance(value.ModelType)
                : Ioc.Default.GetService(value.ModelType);

            if (vm is not ViewModelBase vmb) return;

            CurrentPage = vmb;
        }
        public ObservableCollection<ListItemTemplate> Items { get; }

        [RelayCommand]
        private void TriggerPane()
        {
            IsPaneOpen = !IsPaneOpen;
        }

        private readonly List<ListItemTemplate> _templates =
        [
            new ListItemTemplate(typeof(HomeViewModel), "HomeRegular", "Home"),
            new ListItemTemplate(typeof(SettingsViewModel), "SettingsRegular", "Settings"),
            new ListItemTemplate(typeof(HelpViewModel), "HelpRegular", "Help"),
        ];
    }
    public record ListItemTemplate(Type ModelType, string IconKey, string Label);
}
