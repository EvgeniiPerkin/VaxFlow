using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel.DataAnnotations;

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
        [Required(ErrorMessage = "Не должно быть пустым.")]
        [MinLength(1, ErrorMessage = "От 1 симв.")]
        [MaxLength(100, ErrorMessage = "До 100 симв.")]
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
        [Required(ErrorMessage = "Не должно быть пустым.")]
        [MinLength(1, ErrorMessage = "От 1 симв.")]
        [MaxLength(100, ErrorMessage = "До 100 симв.")]
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
        [MaxLength(100, ErrorMessage = "До 100 симв.")]
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
        [MaxLength(100, ErrorMessage = "До 100 симв.")]
        public string? NameSuffix
        {
            get { return _NameSuffix; }
            set
            {
                SetProperty(ref _NameSuffix, value);
            }
        }

        private bool _IsDismissed = false;

        /// <summary> Уволен? Истина-да, Ложь-нет(действующий сотрудник) </summary>
        public bool IsDismissed
        {
            get { return _IsDismissed; }
            set
            {
                SetProperty(ref _IsDismissed, value);
            }
        }

        public override string ToString()
        {
            return $"{ LastName } { FirstName }"
                + (string.IsNullOrEmpty(Patronymic) ? "" : $" { Patronymic }")
                + (string.IsNullOrEmpty(NameSuffix) ? "" : $" { NameSuffix }");
        }
    }
}
