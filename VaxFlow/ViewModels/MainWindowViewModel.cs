using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using VaxFlow.Data;
using VaxFlow.Models;
using VaxFlow.Services;

namespace VaxFlow.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel(DbContext context, IMyLogger logger)
        {
            Items = new ObservableCollection<ListItemTemplate>(_templates);
            SelectedListItem = Items.First(vm => vm.ModelType == typeof(VaccinationJournalViewModel));
            this.context = context;
            this.logger = logger;
            Task.Run(() => OnLoadAsync());
        }

        #region fields
        private readonly DbContext context;
        private readonly IMyLogger logger;
        #endregion

        #region properties
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
        public ObservableCollection<DoctorModel>? Doctors { get; set; }
        public ObservableCollection<JobCategoryModel>? JobCategories { get; set; }
        public ObservableCollection<VaccineModel>? Vaccines { get; set; }
        public ObservableCollection<VaccineVersionModel>? VaccineVersions { get; set; }
        #endregion

        #region methods
        private async Task OnLoadAsync()
        {
            try
            {
                Doctors = await context.Doctor.GetAllAsync();
                //JobCategories = await context.JobCategory.GetAllAsync();
                //Vaccines = await context.Vaccine.GetAllAsync();
                //VaccineVersions = await context.VaccineVersion.GetAllAsync();
            }
            catch (Exception ex) 
            {
                logger.Error(ex, "Ошибка загрузки данных списков.");
            }
        }
        #endregion

        #region commands
        [RelayCommand]
        private void TriggerPane()
        {
            IsPaneOpen = !IsPaneOpen;
        }
        #endregion

        private readonly List<ListItemTemplate> _templates =
        [
            new ListItemTemplate(typeof(VaccinationJournalViewModel), "JournalRegular", "Журнал вакцинации"),
            new ListItemTemplate(typeof(PatryViewModel), "PatryRegular", "Партии вакцин"),
            new ListItemTemplate(typeof(VaccineViewModel),"VaccineRegular", "Вакцины"),
            new ListItemTemplate(typeof(VaccineVersionsViewModel),"VaccineVersionRegular", "Версии вакцин"),
            new ListItemTemplate(typeof(DoctorViewModel), "PeopleSettingsRegular", "Врачи"),
            new ListItemTemplate(typeof(JobCategoryViewModel), "JobCategoryRegular", "Рабочие категории"),
            new ListItemTemplate(typeof(PatternsViewModel), "DocumentRegular", "Редактор документации"),
            new ListItemTemplate(typeof(SettingsViewModel), "SettingsRegular", "Настройки"),
            new ListItemTemplate(typeof(HelpViewModel), "HelpRegular", "Помощь"),
        ];
    }
    public record ListItemTemplate(Type ModelType, string IconKey, string Label);
}
