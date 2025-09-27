using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using VaxFlow.Data;
using VaxFlow.Models;

namespace VaxFlow.Services
{
    public class ListService : ObservableValidator, IListService
    {
        private readonly DbContext context;
        private readonly IMyLogger logger;
        public ListService(DbContext context, IMyLogger logger)
        {
            this.context = context;
            this.logger = logger;
        }
        private ObservableCollection<DoctorModel>? _Doctors;
        public ObservableCollection<DoctorModel>? Doctors
        {
            get => _Doctors;
            set
            {
                SetProperty(ref _Doctors, value);
            }
        }
        private ObservableCollection<JobCategoryModel>? _JobCategories;
        public ObservableCollection<JobCategoryModel>? JobCategories
        {
            get => _JobCategories;
            set
            {
                SetProperty(ref _JobCategories, value);
            }
        }
        private ObservableCollection<DiseaseModel>? _Diseases;
        public ObservableCollection<DiseaseModel>? Diseases
        {
            get => _Diseases;
            set
            {
                SetProperty(ref _Diseases, value);
            }
        }
        private ObservableCollection<VaccineVersionModel>? _VaccineVersions;
        public ObservableCollection<VaccineVersionModel>? VaccineVersions
        {
            get => _VaccineVersions;
            set
            {
                SetProperty(ref _VaccineVersions, value);
            }
        }
        private ObservableCollection<VaccineSummaryModel>? _Vaccines;
        public ObservableCollection<VaccineSummaryModel>? Vaccines
        {
            get => _Vaccines;
            set
            {
                SetProperty(ref _Vaccines, value);
            }
        }

        public async Task RefreshAsync()
        {
            try
            {
                Doctors = await context.Doctor.GetAllAsync();
                JobCategories = await context.JobCategory.GetAllAsync();
                Diseases = await context.Disease.GetAllAsync();
                VaccineVersions = await context.VaccineVersion.GetAllAsync();
                Vaccines = await context.Vaccine.GetAvailableVaccinesAsync();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Ошибка загрузки данных списков.");
            }
        }
    }
}
