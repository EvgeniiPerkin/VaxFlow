using Avalonia.Controls;
using Avalonia.Controls.Templates;
using CommunityToolkit.Mvvm.DependencyInjection;
using System;
using System.Collections.Generic;
using VaxFlow.ViewModels;
using VaxFlow.Views;

namespace VaxFlow
{
    public class ViewLocator : IDataTemplate
    {
        public ViewLocator()
        {
            RegisterViewFactory<MainWindowViewModel, MainWindow>();
            RegisterViewFactory<VaccinationJournalViewModel, VaccinationJournalView>();
            RegisterViewFactory<DoctorViewModel, DoctorView>();
            RegisterViewFactory<VaccineVersionsViewModel, VaccineVersionsView>();
            RegisterViewFactory<VaccineViewModel, VaccineView>();
            RegisterViewFactory<PatryViewModel, PatryView>();
            RegisterViewFactory<JobCategoryViewModel, JobCategoryView>();
            RegisterViewFactory<SettingsViewModel, SettingsView>();
            RegisterViewFactory<HelpViewModel, HelpView>();
        }

        private readonly Dictionary<Type, Func<Control?>> _locator = [];

        public Control? Build(object? param)
        {
            if (param is null)
                return null;

            _locator.TryGetValue(param.GetType(), out var factory);

            return factory?.Invoke() ?? new TextBlock { Text = $"VM Not Registered: {param.GetType()}" };
        }

        public bool Match(object? data)
        {
            return data is ViewModelBase;
        }
        private void RegisterViewFactory<TViewModel, TView>()
            where TViewModel : class
            where TView : Control
            => _locator.Add(
                typeof(TViewModel),
                Design.IsDesignMode
                    ? Activator.CreateInstance<TView>
                    : Ioc.Default.GetService<TView>);
    }
}
