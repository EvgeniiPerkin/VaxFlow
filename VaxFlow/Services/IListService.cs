using System.Collections.ObjectModel;
using System.Threading.Tasks;
using VaxFlow.Models;

namespace VaxFlow.Services
{
    public interface IListService
    {
        ObservableCollection<DoctorModel>? Doctors { get; set; }
        ObservableCollection<JobCategoryModel>? JobCategories { get; set; }
        ObservableCollection<DiseaseModel>? Diseases { get; set; }
        ObservableCollection<VaccineVersionModel>? VaccineVersions { get; set; }
        ObservableCollection<VaccineSummaryModel>? Vaccines { get; set; }
        Task RefreshVaccinesAsync();
        Task LoadAsync();
    }
}
