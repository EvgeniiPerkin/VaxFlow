using System.ComponentModel.DataAnnotations;

namespace VaxFlow.Models
{
    /// <summary>
    /// Модель данных View (party_summary) из БД 
    /// </summary>
    public class PartySummaryModel : PartyModel
    {
        private string _VaccineName = "";
        /// <summary> Наименование вакцины </summary>
        public string VaccineName
        {
            get { return _VaccineName; }
            set
            {
                SetProperty(ref _VaccineName, value);
            }
        }
        private string _VaccineVersion = "";
        /// <summary> Версия вакцины </summary>
        public string VaccineVersion
        {
            get { return _VaccineVersion; }
            set
            {
                SetProperty(ref _VaccineVersion, value);
            }
        }
    }
}
