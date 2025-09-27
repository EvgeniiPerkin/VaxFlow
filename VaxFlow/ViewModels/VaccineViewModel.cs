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
        public VaccineViewModel(DbContext context, IMyLogger logger, IDialogWindow dialogWindow, IListService listService)
        {
            this.context = context;
            this.logger = logger;
            this.dialogWindow = dialogWindow;
            this.listService = listService;
            Vaccines = listService.Vaccines;
        }

        #region fields
        private readonly DbContext context;
        private readonly IMyLogger logger;
        private readonly IDialogWindow dialogWindow;
        private readonly IListService listService;
        #endregion

        #region properties
        [ObservableProperty]
        private VaccineSummaryModel? _SelectedVaccine;
        [ObservableProperty]
        private string _Output = "";
        private ObservableCollection<VaccineSummaryModel>? _Vaccines;
        public ObservableCollection<VaccineSummaryModel>? Vaccines
        {
            get => _Vaccines;
            set
            {
                SetProperty(ref _Vaccines, value);
            }
        }

        public ObservableCollection<DiseaseModel>? Diseases
        {
            get => listService.Diseases;
        }
        public ObservableCollection<VaccineVersionModel>? VaccineVersions
        {
            get => listService.VaccineVersions;
        }
        #endregion

        #region commands
        [RelayCommand]
        public async Task AddVaccineAsync()
        {
            try
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
                    await listService.RefreshAsync();
                    Vaccines = listService.Vaccines;
                    Output = "Успешнове создание новой записи вакцины.";
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Ошибка создания записи вакцины.");
                Output = $"Ошибка создания записи вакцины: {ex.Message}";
                await dialogWindow.ShowDialogOkCancelAsync("Ошибка", Output);
            }
        }

        [RelayCommand]
        public async Task RemoveVaccineAsync(object parameter)
        {
            try
            {
                if (parameter == null) return;

                if (parameter is VaccineSummaryModel model)
                {
                    bool result = await dialogWindow.ShowDialogYesNoAsync("Подтверждение удаления", 
                        "Удалить вакцину?\nВ случае удаления, " +
                        "связанные с ней данныые примема пациентов так же буудут удалены безвозвратно.");
                    if (result)
                    {
                        int affectedRows = await context.Vaccine.DeleteAsync(model);
                        if (affectedRows > 0)
                        {
                            logger.Info($"Удалена запись вакцины id:{model.Id}");
                            await listService.RefreshAsync();
                            Vaccines = listService.Vaccines;
                            Output = "Успешное удаление записи вакцины.";
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
                if (parameter == null) return;

                if (parameter is VaccineSummaryModel model)
                {
                    int affectedRows = await context.Vaccine.UpdateAsync(model);
                    if (affectedRows > 0)
                    {
                        logger.Info($"Обновление данных вакцины id:{model.Id}");
                        Output = "Успешное обновление данных вакцины.";
                        await listService.RefreshAsync();
                        Vaccines = listService.Vaccines;
                        await dialogWindow.ShowDialogOkCancelAsync("Информация.", Output);
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
