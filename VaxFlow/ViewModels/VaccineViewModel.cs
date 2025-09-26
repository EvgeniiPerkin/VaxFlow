using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using VaxFlow.Data;
using VaxFlow.DialogWindows;
using VaxFlow.Models;
using VaxFlow.Services;

namespace VaxFlow.ViewModels
{
    public partial class VaccineViewModel : ViewModelBase
    {
        public VaccineViewModel(DbContext context, IMyLogger logger, IDialogWindow dialogWindow)
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
        private VaccineSummaryModel? _SelectedVaccine;
        [ObservableProperty]
        private string _Output = "";
        #endregion

        #region methods
        public static void RefreshCollections(ObservableCollection<VaccineSummaryModel> oldItems,
            ObservableCollection<VaccineSummaryModel> newItems)
        {
            for (int i = 0; i < oldItems.Count; i++)
            {
                bool isFound = false;
                
                for (int j = 0; j < newItems.Count; j++)
                {
                    if (oldItems[i].Id == newItems[j].Id)
                    {
                        oldItems[i] = newItems[j];
                        isFound = true;
                        break;
                    }
                }

                if (!isFound)
                {
                    oldItems.Remove(oldItems[i]);
                }
            }
            for (int j = 0; j < newItems.Count; j++)
            {
                bool isFound = false;
                
                for (int i = 0; i < oldItems.Count; i++)
                {
                    if (oldItems[i].Id == newItems[j].Id)
                    {
                        isFound = true;
                        break;
                    }
                }

                if (!isFound)
                {
                    oldItems.Add(newItems[j]);
                }
            }
        }
        #endregion

        #region commands
        [RelayCommand]
        public async Task AddVaccineAsync(object parameter)
        {
            try
            {
                if (parameter == null) return;

                if (parameter is ObservableCollection<VaccineSummaryModel> collection)
                {
                    VaccineModel newVaccine = new()
                    {
                        VaccineName = "Regivag",
                        Count = 290,
                        DateTimeCreate = DateTime.Now,
                        ExpirationDate = DateTime.Now,
                        Series = "s23",
                        DiseaseId = 1,
                        VaccineVersionId = 1
                    };
                    int affectedRows = await context.Vaccine.CreateAsync(newVaccine);
                    if (affectedRows > 0)
                    {
                        logger.Info($"Создана запись вакцины id:{newVaccine.Id}");
                        RefreshCollections(collection, await context.Vaccine.GetAvailableVaccinesAsync());
                        Output = "Успешнове создание новой записи вакцины.";
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Ошибка создания записи вакцины.");
                Output = $"Ошибка создания записи вакцины: {ex.Message}";
                await dialogWindow.ShowDialogOkCancelAsync("Ошибка", Output);
            }
        }
        private bool CanAddVaccineAsync(object parameter)
        {
            return parameter != null;
        }

        [RelayCommand]
        public async Task RemoveVaccineAsync(object parameter)
        {
            try
            {
                if (SelectedVaccine != null)
                {
                    if (parameter == null) return;

                    if (parameter is ObservableCollection<VaccineSummaryModel> collection)
                    {
                        bool result = await dialogWindow.ShowDialogYesNoAsync("Подтверждение удаления", 
                            "Удалить вакцину?\nВ случае удаления, " +
                            "связанные с ней данныые примема пациентов так же буудут удалены безвозвратно.");
                        if (result)
                        {
                            int affectedRows = await context.Vaccine.DeleteAsync(SelectedVaccine);
                            if (affectedRows > 0)
                            {
                                logger.Info($"Удалена запись вакцины id:{SelectedVaccine.Id}");
                                collection.Remove(SelectedVaccine);
                                Output = "Успешное удаление записи вакцины.";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Ошибка удаления записи вакцины.");
                Output = $"Ошибка удаления записи вакцины: {ex.Message}";
                await dialogWindow.ShowDialogOkCancelAsync("Ошибка", Output);
            }
        }
        private bool CanRemoveVaccineAsync(object parameter)
        {
            return parameter != null;
        }

        [RelayCommand]
        public async Task UpdateVaccineAsync(object parameter)
        {
            try
            {
                if (SelectedVaccine == null) return;

                if (parameter == null) return;

                if (parameter is ObservableCollection<VaccineSummaryModel> collection)
                {
                    int affectedRows = await context.Vaccine.UpdateAsync((VaccineModel)SelectedVaccine);
                    if (affectedRows > 0)
                    {
                        logger.Info($"Обновление данных вакцины id:{SelectedVaccine.Id}");
                        RefreshCollections(collection, await context.Vaccine.GetAvailableVaccinesAsync());
                        Output = "Успешное обновление данных вакцины.";
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Ошибка изменения данных вакцины.");
                Output = $"Ошибка изменения записи вакцины: {ex.Message}";
                await dialogWindow.ShowDialogOkCancelAsync("Ошибка", Output);
            }
        }
        private bool CanUpdateVaccineAsync(object parameter)
        {
            return parameter != null;
        }
        #endregion
    }
}
