using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace VaxFlow.Models
{
    /// <summary>
    /// Модель категории вида трудовой деятельности пациента
    /// </summary>
    public class JobCategoryModel : ObservableValidator
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
        private string _Desc = string.Empty;

        /// <summary> Описание </summary>
        [Required(ErrorMessage = "Не должно быть пустым.")]
        [MinLength(1, ErrorMessage = "От 1 симв.")]
        [MaxLength(255, ErrorMessage = "До 255 симв.")]
        public string Desc
        {
            get { return _Desc; }
            set
            {
                SetProperty(ref _Desc, value);
            }
        }
    }
}
