using CommunityToolkit.Mvvm.ComponentModel;

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
