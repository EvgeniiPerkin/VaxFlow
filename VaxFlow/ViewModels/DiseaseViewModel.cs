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
        public DiseaseViewModel(DbContext context, IMyLogger logger, IDialogWindow dialogWindow, IListService listService)
        {
            this.context = context;
            this.logger = logger;
            this.dialogWindow = dialogWindow;
            this.listService = listService;
        }

        #region fields
        private readonly DbContext context;
        private readonly IMyLogger logger;
        private readonly IDialogWindow dialogWindow;
        private readonly IListService listService;
        #endregion

        #region properties
        [ObservableProperty]
        private DiseaseModel? _SelectedDisease;
        [ObservableProperty]
        private string _Output = "";
        public ObservableCollection<DiseaseModel>? Diseases
        {
            get { return listService.Diseases; }
            set { listService.Diseases = value; }
        }
        #endregion

        #region commands
        [RelayCommand]
        public async Task AddDiseaseAsync()
        {
            try
            {
                if (Diseases == null) return;

                DiseaseModel newDisease= new()
                {
                    Desc = "Короновирус"
                };
                int affectedRows = await context.Disease.CreateAsync(newDisease);
                if (affectedRows > 0)
                {
                    logger.Info($"Создана запись заболевания id:{newDisease.Id}");
                    Diseases.Add(newDisease);
                    Output = "Успешнове создание новой записи заболевания.";
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Ошибка создания записи заболевания.");
                Output = $"Ошибка создания записи заболевания: {ex.Message}";
                await dialogWindow.ShowDialogOkCancelAsync("Ошибка", Output);
            }
        }

        [RelayCommand]
        public async Task UpdateDiseaseAsync(object parameter)
        {
            try
            {
                if (parameter == null) return;

                if (parameter is DiseaseModel model)
                {
                    int affectedRows = await context.Disease.UpdateAsync(model);
                    if (affectedRows > 0)
                    {
                        logger.Info($"Обновление данных заболевания id:{model.Id}");
                        Output = "Успешное обновление данных заболевания.";
                        await dialogWindow.ShowDialogOkCancelAsync("Информация.", Output);
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
