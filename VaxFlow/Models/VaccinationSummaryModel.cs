namespace VaxFlow.Models
{
    /// <summary>
    /// Модель данных о вакцинации из вью бд vaccination_summary
    /// </summary>
    public class VaccinationSummaryModel : VaccinationModel
    {

        private string _DoctorInitials = string.Empty;

        /// <summary> Инициалы доктора (Иванов И.И.) </summary>
        public string DoctorInitials
        {
            get { return _DoctorInitials; }
            set
            {
                SetProperty(ref _DoctorInitials, value);
            }
        }

        private string _DescParty = string.Empty;

        /// <summary> Описание парции вакцины </summary>
        public string DescParty
        {
            get { return _DescParty; }
            set
            {
                SetProperty(ref _DescParty, value);
            }
        }
    }
}
