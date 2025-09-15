using CommunityToolkit.Mvvm.ComponentModel;
using System;

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

        private string _PartyName = string.Empty;

        /// <summary> Имя партии </summary>
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
