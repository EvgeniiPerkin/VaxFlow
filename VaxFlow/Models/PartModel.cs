using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace VaxFlow.Models
{
    /// <summary>
    /// Часть документа
    /// </summary>
    public class PartModel : ObservableValidator
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

        private int _PatternId;

        /// <summary> Идентификатор шаблона </summary>
        public int PatternId
        {
            get { return _PatternId; }
            set
            {
                SetProperty(ref _PatternId, value);
            }
        }

        private int _SerialNumber;

        /// <summary> Порядковый номер </summary>
        public int SerialNumber
        {
            get { return _SerialNumber; }
            set
            {
                SetProperty(ref _SerialNumber, value);
            }
        }

        private string _Body = string.Empty;

        /// <summary> Содержание </summary>
        [Required(ErrorMessage = "Не должно быть пустым.")]
        [MinLength(1, ErrorMessage = "От 1 симв.")]
        [MaxLength(2048, ErrorMessage = "До 2048 симв.")]
        public string Body
        {
            get { return _Body; }
            set
            {
                SetProperty(ref _Body, value);
            }
        }

        private string? _Desc;

        /// <summary> Описание фрагмента </summary>
        [Required(ErrorMessage = "Не должно быть пустым.")]
        [MinLength(3, ErrorMessage = "От 3 симв.")]
        [MaxLength(60, ErrorMessage = "До 60 симв.")]
        public string? Desc
        {
            get { return _Desc; }
            set
            {
                SetProperty(ref _Desc, value);
            }
        }

        private bool _IsUrl;

        /// <summary> Является ссылкой </summary>
        public bool IsUrl
        {
            get { return _IsUrl; }
            set
            {
                SetProperty(ref _IsUrl, value);
            }
        }

        private string? _Url;

        /// <summary> Ссылка </summary>
        [MaxLength(200, ErrorMessage = "До 200 симв.")]
        public string? Url
        {
            get { return _Url; }
            set
            {
                SetProperty(ref _Url, value);
            }
        }

        private bool _IsBold;

        /// <summary> Жирный </summary>
        public bool IsBold
        {
            get { return _IsBold; }
            set
            {
                SetProperty(ref _IsBold, value);
            }
        }

        private bool _IsItalic;

        /// <summary> Курсив </summary>
        public bool IsItalic
        {
            get { return _IsItalic; }
            set
            {
                SetProperty(ref _IsItalic, value);
            }
        }

        private bool _IsUnderline;

        /// <summary> Подчеркивать </summary>
        public bool IsUnderline
        {
            get { return _IsUnderline; }
            set
            {
                SetProperty(ref _IsUnderline, value);
            }
        }
    }
}
