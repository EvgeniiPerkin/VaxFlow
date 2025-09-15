using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.ComponentModel.DataAnnotations;

namespace VaxFlow.Models
{
    /// <summary>
    /// Модель партии вакцины
    /// </summary>
    public class PartyModel : ObservableValidator
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

        private string _PartyName = string.Empty;

        /// <summary> Имя партии </summary>
        [Required(ErrorMessage = "Не должно быть пустым.")]
        [MinLength(1, ErrorMessage = "От 1 симв.")]
        [MaxLength(255, ErrorMessage = "До 255 симв.")]
        public string PartyName
        {
            get { return _PartyName; }
            set
            {
                SetProperty(ref _PartyName, value);
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
