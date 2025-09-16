using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace VaxFlow.Models
{
    /// <summary>
    /// Модель данных версии вакцины
    /// </summary>
    public class VaccineVersionModel : ObservableValidator
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

        private string _Version = string.Empty;

        /// <summary> Версия </summary>
        [Required(ErrorMessage = "Не должно быть пустым.")]
        [MinLength(1, ErrorMessage = "От 1 симв.")]
        [MaxLength(10, ErrorMessage = "До 10 симв.")]
        public string Version
        {
            get { return _Version; }
            set
            {
                SetProperty(ref _Version, value);
            }
        }
    }
}
