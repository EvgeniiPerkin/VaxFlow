using CommunityToolkit.Mvvm.ComponentModel;
using System;

namespace VaxFlow.Models
{
    /// <summary>
    /// Модель вакцинации пациента 
    /// </summary>
    public class VaccinationModel : ObservableValidator
    {
        private int _DoctorId;

        /// <summary> Идентификатор врача </summary>
        public int DoctorId
        {
            get { return _DoctorId; }
            set
            {
                SetProperty(ref _DoctorId, value);
            }
        }

        private int _PatientId;

        /// <summary> Идентификатор пациента </summary>
        public int PatientId
        {
            get { return _PatientId; }
            set
            {
                SetProperty(ref _PatientId, value);
            }
        }

        private int _VaccineId;

        /// <summary> Идентификатор вакцины </summary>
        public int VaccineId
        {
            get { return _VaccineId; }
            set
            {
                SetProperty(ref _VaccineId, value);
            }
        }

        private DateTime _DateTimeOfVaccination = DateTime.Now;

        /// <summary> Дата и время назначения </summary>
        public DateTime DateTimeOfVaccination
        {
            get { return _DateTimeOfVaccination; }
            set
            {
                SetProperty(ref _DateTimeOfVaccination, value);
            }
        }
    }
}
