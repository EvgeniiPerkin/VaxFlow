using System.ComponentModel.DataAnnotations;

namespace VaxFlow.Models
{
    /// <summary>
    /// Модель данных View (party_summary) из БД 
    /// </summary>
    public class PartySummaryModel : PartyModel
    {
        private string _DiseaseName = "";
        /// <summary> Наименование заболевания </summary>
        public string DiseaseName
        {
            get { return _DiseaseName; }
            set
            {
                SetProperty(ref _DiseaseName, value);
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
