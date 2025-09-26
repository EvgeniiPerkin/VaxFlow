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
    public partial class DiseaseViewModel : ViewModelBase
    {
        public DiseaseViewModel(DbContext context, IMyLogger logger, IDialogWindow dialogWindow)
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
        private DiseaseModel? _SelectedDisease;
        [ObservableProperty]
        private string _Output = "";
        #endregion

        #region methods
        #endregion

        #region commands
        [RelayCommand]
        public async Task AddDiseaseAsync(object parameter)
        {
            try
            {
                if (parameter == null) return;

                if (parameter is ObservableCollection<DiseaseModel> collection)
                {
                    DiseaseModel newDisease= new()
                    {
                        Desc = "Короновирус"
                    };
                    int affectedRows = await context.Disease.CreateAsync(newDisease);
                    if (affectedRows > 0)
                    {
                        logger.Info($"Создана запись заболевания id:{newDisease.Id}");
                        collection.Add(newDisease);
                        Output = "Успешнове создание новой записи заболевания.";
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Ошибка создания записи заболевания.");
                Output = $"Ошибка создания записи заболевания: {ex.Message}";
                await dialogWindow.ShowDialogOkCancelAsync("Ошибка", Output);
            }
        }
        private bool CanAddDiseaseAsync(object parameter)
        {
            return parameter != null;
        }

        [RelayCommand]
        public async Task UpdateDiseaseAsync(object parameter)
        {
            try
            {
                if (SelectedDisease == null) return;

                if (parameter == null) return;

                if (parameter is ObservableCollection<DiseaseModel> collection)
                {
                    int affectedRows = await context.Disease.UpdateAsync(SelectedDisease);
                    if (affectedRows > 0)
                    {
                        logger.Info($"Обновление данных заболевания id:{SelectedDisease.Id}");
                        int indx = collection.IndexOf(SelectedDisease);
                        collection[indx] = SelectedDisease;
                        Output = "Успешное обновление данных заболевания.";
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Ошибка изменения данных заболевания.");
                Output = $"Ошибка изменения записи заболевания: {ex.Message}";
                await dialogWindow.ShowDialogOkCancelAsync("Ошибка", Output);
            }
        }
        private bool CanUpdateDiseaseAsync(object parameter)
        {
            return parameter != null;
        }
        #endregion
    }
}
