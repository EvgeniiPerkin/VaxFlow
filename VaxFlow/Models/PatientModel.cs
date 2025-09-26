using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace VaxFlow.Models
{
    /// <summary>
    /// Модель данных пациента
    /// </summary>
    public class PatientModel : ObservableValidator
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
        private string _FullName = string.Empty;

        /// <summary> Фамилия Имя Отчество </summary>
        [Required(ErrorMessage = "Не должно быть пустым.")]
        [MinLength(1, ErrorMessage = "От 1 симв.")]
        [MaxLength(100, ErrorMessage = "До 255 симв.")]
        public string FullName
        {
            get { return _FullName; }
            set
            {
                SetProperty(ref _FullName, value);
            }
        }

        private DateTime _Birthday = DateTime.MinValue;

        /// <summary> День рождения </summary>
        public DateTime Birthday
        {
            get { return _Birthday; }
            set
            {
                SetProperty(ref _Birthday, value);
            }
        }

        private string _RegistrationAddress = string.Empty;

        /// <summary> Адрес регистрации </summary>
        [Required(ErrorMessage = "Не должно быть пустым.")]
        [MinLength(1, ErrorMessage = "От 1 симв.")]
        [MaxLength(255, ErrorMessage = "До 255 симв.")]
        public string RegistrationAddress
        {
            get { return _RegistrationAddress; }
            set
            {
                SetProperty(ref _RegistrationAddress, value);
            }
        }

        private string _PolicyNumber = string.Empty;

        /// <summary> Номер полиса </summary>
        [Required(ErrorMessage = "Не должно быть пустым.")]
        [MinLength(1, ErrorMessage = "От 1 симв.")]
        [MaxLength(30, ErrorMessage = "До 30 симв.")]
        public string PolicyNumber
        {
            get { return _PolicyNumber; }
            set
            {
                SetProperty(ref _PolicyNumber, value);
            }
        }
        
        private string _WorkingPosition = string.Empty;

        /// <summary> Должность на работе </summary>
        [Required(ErrorMessage = "Не должно быть пустым.")]
        [MinLength(1, ErrorMessage = "От 1 симв.")]
        [MaxLength(255, ErrorMessage = "До 255 симв.")]
        public string WorkingPosition
        {
            get { return _WorkingPosition; }
            set
            {
                SetProperty(ref _WorkingPosition, value);
            }
        }
        
        private int? _JobCategoryId;

        /// <summary> Идентификатор категории работы </summary>
        public int? JobCategoryId
        {
            get { return _JobCategoryId; }
            set
            {
                SetProperty(ref _JobCategoryId, value);
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

        private ObservableCollection<VaccinationSummaryModel> _Vaccinations = [];

        /// <summary> Данные по вакцинации пациета </summary>
        public ObservableCollection<VaccinationSummaryModel> Vaccinations
        {
            get { return _Vaccinations; }
            set
            {
                SetProperty(ref _Vaccinations, value);
            }
        }
    }
}
