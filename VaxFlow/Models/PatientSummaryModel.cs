namespace VaxFlow.Models
{
    /// <summary>
    /// Модель краткой информации о пациенте из вью бд patient_summary
    /// </summary>
    public class PatientSummaryModel : PatientModel
    {
        private string _PatientInitials = string.Empty;

        /// <summary> Инициалы пациента (Иванов И.И.) </summary>
        public string PatientInitials
        {
            get { return _PatientInitials; }
            set
            {
                SetProperty(ref _PatientInitials, value);
            }
        }
    }
}
