using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using VaxFlow.Data;
using VaxFlow.DialogWindows;
using VaxFlow.Models;
using VaxFlow.Services;

namespace VaxFlow.ViewModels
{
    public partial class PatryViewModel : ViewModelBase
    {
        public PatryViewModel(DbContext context, IMyLogger logger, IDialogWindow dialogWindow)
        {
            this.context = context;
            this.logger = logger;
            this.dialogWindow = dialogWindow;
        }

        #region fields
        private readonly DbContext context;
        private readonly IMyLogger logger;
        private readonly IDialogWindow dialogWindow;
        #endregion

        #region properties
        [ObservableProperty]
        private PartySummaryModel? _SelectedParty;
        [ObservableProperty]
        private string _Output = "";
        #endregion

        #region methods
        //public void RefreshCollections(ObservableCollection<PartySummaryModel> oldItems,
        //    ObservableCollection<PartySummaryModel> newItems)
        //{
        //    for (int i = 0; i < oldItems.Count; i++)
        //    {
        //        bool isFound = false;
        //        PartySummaryModel? required = null;
        //        for (int j = 0; j < newItems.Count; j++)
        //        {
        //            if (oldItems[i].Id == newItems[j].Id)
        //            {
        //                required = newItems[j];
        //                isFound = true;
        //                break;
        //            }
        //        }

        //        if (isFound)
        //        {
        //            oldItems[i] = required;
        //        }
        //        else
        //        {
        //            oldItems.Add(required);
        //        }
        //    }
        //}
        #endregion

        #region commands
        [RelayCommand]
        public async Task AddPartyAsync(object parameter)
        {
            try
            {
                if (parameter == null) return;

                if (parameter is ObservableCollection<PartySummaryModel> collection)
                {
                    PartyModel newParty = new()
                    {
                        PartyName = "Партия001",
                        Count = 290,
                        DateTimeCreate = DateTime.Now,
                        VaccineId = 1,
                        VaccineVersionId = 1
                    };
                    int affectedRows = await context.Party.CreateAsync(newParty);
                    if (affectedRows > 0)
                    {
                        logger.Info($"Создана запись партии вакцины id:{newParty.Id}");
                        collection = await context.Party.GetAvailablePartiesAsync();
                        Output = "Успешнове создание новой записи партии вакцины.";
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Ошибка создания записи партии вакцины.");
                Output = $"Ошибка создания записи партии вакцины: {ex.Message}";
            }
        }
        private bool CanAddPartyAsync(object parameter)
        {
            return parameter != null;
        }

        [RelayCommand]
        public async Task RemovePartyAsync(object parameter)
        {
            try
            {
                if (SelectedParty != null)
                {
                    if (parameter == null) return;

                    if (parameter is ObservableCollection<PartySummaryModel> collection)
                    {
                        bool result = await dialogWindow.ShowDialogYesNoAsync("Подтверждение удаления", 
                            "Удалить партию вакцины?\nВ случае удаления партии, " +
                            "связанные с ней данныые примема пациентов так же буудут удалены безвозвратно.");
                        if (result)
                        {
                            int affectedRows = await context.Party.DeleteAsync(SelectedParty);
                            if (affectedRows > 0)
                            {
                                logger.Info($"Удалена запись партии вакцины id:{SelectedParty.Id}");
                                collection.Remove(SelectedParty);
                                Output = "Успешное удаление записи партии вакцины.";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Ошибка удаления записи партии вакцины.");
                Output = $"Ошибка удаления записи партии вакцины: {ex.Message}";
            }
        }
        private bool CanRemovePartyAsync(object parameter)
        {
            return parameter != null;
        }

        [RelayCommand]
        public async Task UpdatePartyAsync(object parameter)
        {
            try
            {
                if (SelectedParty == null) return;

                if (parameter == null) return;

                if (parameter is ObservableCollection<PartySummaryModel> collection)
                {
                    int affectedRows = await context.Party.UpdateAsync((PartyModel)SelectedParty);
                    if (affectedRows > 0)
                    {
                        logger.Info($"Обновление данных партии вакцины id:{SelectedParty.Id}");
                        int indx = collection.IndexOf(SelectedParty);
                        collection[indx] = SelectedParty;
                        Output = "Успешное обновление данных партии вакцины.";
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Ошибка изменения данных партии вакцины.");
                Output = $"Ошибка изменения записи партии вакцины: {ex.Message}";
            }
        }
        private bool CanUpdatePartyAsync(object parameter)
        {
            return parameter != null;
        }
        #endregion
    }
}
