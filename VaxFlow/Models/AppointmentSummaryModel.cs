namespace VaxFlow.Models
{
    /// <summary>
    /// Модель данных из вью бд appointment_summary
    /// </summary>
    public class AppointmentSummaryModel : DoctorsAppointmentsModel
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

        private string _PatientInitials = string.Empty;

        /// <summary>  Инициалы пациента (Иванов И.И.) </summary>
        public string PatientInitials
        {
            get { return _PatientInitials; }
            set
            {
                SetProperty(ref _PatientInitials, value);
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
