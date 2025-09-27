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
    public partial class VaccineVersionsViewModel : ViewModelBase
    {
        public VaccineVersionsViewModel(DbContext context, IMyLogger logger, IDialogWindow dialogWindow, IListService listService)
        {
            this.context = context;
            this.logger = logger;
            this.dialogWindow = dialogWindow;
            this.ListService = listService;
        }

        #region fields
        private readonly DbContext context;
        private readonly IMyLogger logger;
        private readonly IDialogWindow dialogWindow;
        #endregion

        #region properties
        public IListService ListService { get; }
        [ObservableProperty]
        private VaccineVersionModel? _SelectedVaccineVersion;
        [ObservableProperty]
        private string _Output = "";
        #endregion

        #region methods
        #endregion

        #region commands
        [RelayCommand]
        public async Task AddVaccineVersionAsync(object parameter)
        {
            try
            {
                if (ListService.VaccineVersions == null) ListService.VaccineVersions = [];

                VaccineVersionModel newVaccineVersion = new()
                {
                    Version = "VX"
                };
                int affectedRows = await context.VaccineVersion.CreateAsync(newVaccineVersion);
                if (affectedRows > 0)
                {
                    logger.Info($"Создана запись рверсии вакцины id:{newVaccineVersion.Id}");
                    ListService.VaccineVersions.Add(newVaccineVersion);
                    Output = "Успешнове создание новой записи версии вакцины.";
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Ошибка создания записи версии вакцины.");
                Output = $"Ошибка создания записи версии вакцины: {ex.Message}";
                await dialogWindow.ShowDialogOkCancelAsync("Ошибка", Output);
            }
        }

        [RelayCommand]
        public async Task UpdateVaccineVersionAsync(object parameter)
        {
            try
            {
                if (parameter == null) return;

                if (parameter is VaccineVersionModel model)
                {
                    int affectedRows = await context.VaccineVersion.UpdateAsync(model);
                    if (affectedRows > 0)
                    {
                        logger.Info($"Обновление данных версии вакцины id:{model.Id}");
                        Output = "Успешное обновление данных версии вакцины.";
                        await dialogWindow.ShowDialogOkCancelAsync("Информация.", Output);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Ошибка изменения данных версии вакцины.");
                Output = $"Ошибка изменения записи версии вакцины: {ex.Message}";
                await dialogWindow.ShowDialogOkCancelAsync("Ошибка", Output);
            }
        }
        private bool CanUpdateVaccineVersionAsync(object parameter)
        {
            return parameter != null;
        }
        #endregion
    }
}
