using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.ComponentModel.DataAnnotations;

namespace VaxFlow.Models
{
    /// <summary>
    /// Модель вакцины
    /// </summary>
    public class VaccineModel : ObservableValidator
    {
        private int _Id;

        /// <summary> Идентификатор </summary>
        public int Id
        {
            get { return _Id; }
            set
            {
                SetProperty(ref _Id, value);
            }
        }

        private int _DiseaseId;

        /// <summary> Идентификатор заболевания </summary>
        public int DiseaseId
        {
            get { return _DiseaseId; }
            set
            {
                SetProperty(ref _DiseaseId, value);
            }
        }

        private int _VaccineVersionId;

        /// <summary> Идентификатор версии вакцины </summary>
        public int VaccineVersionId
        {
            get { return _VaccineVersionId; }
            set
            {
                SetProperty(ref _VaccineVersionId, value);
            }
        }
        
        private string _VaccineName = string.Empty;

        /// <summary> Имя вакцины </summary>
        [Required(ErrorMessage = "Не должно быть пустым.")]
        [MinLength(1, ErrorMessage = "От 1 симв.")]
        [MaxLength(255, ErrorMessage = "До 255 симв.")]
        public string VaccineName
        {
            get { return _VaccineName; }
            set
            {
                SetProperty(ref _VaccineName, value);
            }
        }

        private string _Series = string.Empty;

        /// <summary> Серия вакцины </summary>
        [Required(ErrorMessage = "Не должно быть пустым.")]
        [MinLength(1, ErrorMessage = "От 1 симв.")]
        [MaxLength(50, ErrorMessage = "До 50 симв.")]
        public string Series
        {
            get { return _Series; }
            set
            {
                SetProperty(ref _Series, value);
            }
        }

        private DateTime _ExpirationDate = DateTime.Now;

        /// <summary> Срок годности </summary>
        public DateTime ExpirationDate
        {
            get { return _ExpirationDate; }
            set
            {
                SetProperty(ref _ExpirationDate, value);
            }
        }

        private int _Count;

        /// <summary> Кол-во вакцин в партии </summary>
        public int Count
        {
            get { return _Count; }
            set
            {
                SetProperty(ref _Count, value);
            }
        }

        private DateTime _DateTimeCreate = DateTime.Now;

        /// <summary> Дата и время создания записи </summary>
        public DateTime DateTimeCreate
        {
            get { return _DateTimeCreate; }
            set
            {
                SetProperty(ref _DateTimeCreate, value);
            }
        }
    }
}
