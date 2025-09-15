using CommunityToolkit.Mvvm.ComponentModel;

namespace VaxFlow.Models
{
    /// <summary>
    /// Модель данных вакцины
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

        private int _PartyId;

        /// <summary> Идентификатор партии вакцины </summary>
        public int PartyId
        {
            get { return _PartyId; }
            set
            {
                SetProperty(ref _PartyId, value);
            }
        }
    }
}
