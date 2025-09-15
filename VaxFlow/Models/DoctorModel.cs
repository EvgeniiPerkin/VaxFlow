using CommunityToolkit.Mvvm.ComponentModel;

namespace VaxFlow.Models
{
    /// <summary>
    /// Модель данных врача
    /// </summary>
    public class DoctorModel : ObservableValidator
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

        private string _LastName = string.Empty;

        /// <summary> Фамилия </summary>
        public string LastName
        {
            get { return _LastName; }
            set
            {
                SetProperty(ref _LastName, value);
            }
        }

        private string _FirstName = string.Empty;

        /// <summary> Имя </summary>
        public string FirstName
        {
            get { return _FirstName; }
            set
            {
                SetProperty(ref _FirstName, value);
            }
        }

        private string? _Patronymic;

        /// <summary> Отчество </summary>
        public string? Patronymic
        {
            get { return _Patronymic; }
            set
            {
                SetProperty(ref _Patronymic, value);
            }
        }

        private string? _NameSuffix;

        /// <summary> Префикс(оглы, кызы) </summary>
        public string? NameSuffix
        {
            get { return _NameSuffix; }
            set
            {
                SetProperty(ref _NameSuffix, value);
            }
        }
    }
}
