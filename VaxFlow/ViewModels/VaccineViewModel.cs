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
        private VaccineModel? _SelectedVaccine;
        [ObservableProperty]
        private string _Output = "";
        #endregion

        #region methods
        #endregion

        #region commands
        [RelayCommand]
        public async Task AddVaccineAsync(object parameter)
        {
            try
            {
                if (parameter == null) return;

                if (parameter is ObservableCollection<VaccineModel> collection)
                {
                    VaccineModel newVaccine= new()
                    {
                        Desc = "Спутник5"
                    };
                    int affectedRows = await context.Vaccine.CreateAsync(newVaccine);
                    if (affectedRows > 0)
                    {
                        logger.Info($"Создана запись вакцины id:{newVaccine.Id}");
                        collection.Add(newVaccine);
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
        public async Task UpdateVaccineAsync(object parameter)
        {
            try
            {
                if (SelectedVaccine == null) return;

                if (parameter == null) return;

                if (parameter is ObservableCollection<VaccineModel> collection)
                {
                    int affectedRows = await context.Vaccine.UpdateAsync(SelectedVaccine);
                    if (affectedRows > 0)
                    {
                        logger.Info($"Обновление данных вакцины id:{SelectedVaccine.Id}");
                        int indx = collection.IndexOf(SelectedVaccine);
                        collection[indx] = SelectedVaccine;
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
