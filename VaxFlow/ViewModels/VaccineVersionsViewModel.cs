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
        public VaccineVersionsViewModel(DbContext context, IMyLogger logger, IDialogWindow dialogWindow)
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
                if (parameter == null) return;

                if (parameter is ObservableCollection<VaccineVersionModel> collection)
                {
                    VaccineVersionModel newVaccineVersion = new()
                    {
                        Version = "VX"
                    };
                    int affectedRows = await context.VaccineVersion.CreateAsync(newVaccineVersion);
                    if (affectedRows > 0)
                    {
                        logger.Info($"Создана запись рверсии вакцины id:{newVaccineVersion.Id}");
                        collection.Add(newVaccineVersion);
                        Output = "Успешнове создание новой записи версии вакцины.";
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Ошибка создания записи версии вакцины.");
                Output = $"Ошибка создания записи версии вакцины: {ex.Message}";
                await dialogWindow.ShowDialogOkCancelAsync("Ошибка", Output);
            }
        }
        private bool CanAddVaccineVersionAsync(object parameter)
        {
            return parameter != null;
        }

        [RelayCommand]
        public async Task UpdateVaccineVersionAsync(object parameter)
        {
            try
            {
                if (SelectedVaccineVersion == null) return;

                if (parameter == null) return;

                if (parameter is ObservableCollection<VaccineVersionModel> collection)
                {
                    int affectedRows = await context.VaccineVersion.UpdateAsync(SelectedVaccineVersion);
                    if (affectedRows > 0)
                    {
                        logger.Info($"Обновление данных версии вакцины id:{SelectedVaccineVersion.Id}");
                        int indx = collection.IndexOf(SelectedVaccineVersion);
                        collection[indx] = SelectedVaccineVersion;
                        Output = "Успешное обновление данных версии вакцины.";
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
