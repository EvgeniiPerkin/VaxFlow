using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using VaxFlow.Data;
using VaxFlow.Models;

namespace VaxFlow.Services
{
    public class ListService : IListService
    {
        private readonly DbContext context;
        private readonly IMyLogger logger;
        public ListService(DbContext context, IMyLogger logger)
        {
            this.context = context;
            this.logger = logger;
        }
        public ObservableCollection<DoctorModel>? Doctors { get; set; }
        public ObservableCollection<JobCategoryModel>? JobCategories { get; set; }
        public ObservableCollection<DiseaseModel>? Diseases { get; set; }
        public ObservableCollection<VaccineVersionModel>? VaccineVersions { get; set; }
        public ObservableCollection<VaccineSummaryModel>? Vaccines { get; set; }

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
