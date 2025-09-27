using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using VaxFlow.Data;
using VaxFlow.DialogWindows;
using VaxFlow.Models;
using VaxFlow.Services;

namespace VaxFlow.ViewModels
{
    public partial class JobCategoryViewModel : ViewModelBase
    {
        public JobCategoryViewModel(DbContext context, IMyLogger logger, IDialogWindow dialogWindow, IListService listService)
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
        private JobCategoryModel? _SelectedJobCategory;
        [ObservableProperty]
        private string _Output = "";
        public ObservableCollection<JobCategoryModel>? JobCategories
        { 
            get => listService.JobCategories; 
            set => listService.JobCategories = value; 
        }
        #endregion

        #region methods
        #endregion

        #region commands
        [RelayCommand]
        public async Task AddJobCategoryAsync()
        {
            try
            {
                if (JobCategories == null) JobCategories = [];

                JobCategoryModel newJobCategory = new()
                {
                    Category = "Работники бюджетной сферы («бюджетники»)",
                    Desc = "Работают в учреждениях, финансируемых из государственного или муниципального бюджета (образование, здравоохранение, культура, госуправление)."
                };
                int affectedRows = await context.JobCategory.CreateAsync(newJobCategory);
                if (affectedRows > 0)
                {
                    logger.Info($"Создана запись рабочей категории id:{newJobCategory.Id}");
                    JobCategories.Add(newJobCategory);
                    Output = "Успешнове создание новой записи рабочей категории.";
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Ошибка создания записи рабочей категории.");
                Output = $"Ошибка создания записи рабочей категории: {ex.Message}";
                await dialogWindow.ShowDialogOkCancelAsync("Ошибка", Output);
            }
        }

        [RelayCommand]
        public async Task RemoveJobCategoryAsync(object parameter)
        {
            try
            {
                if (parameter == null) return;

                if (parameter is JobCategoryModel model)
                {
                    bool result = await dialogWindow.ShowDialogYesNoAsync("Подтверждение удаления.", "Удалить запись рабочей категории?");
                    if (result)
                    {
                        int affectedRows = await context.JobCategory.DeleteAsync(model);
                        if (affectedRows > 0)
                        {
                            logger.Info($"Удалена запись рабочей категории id:{model.Id}");
                            JobCategories?.Remove(model);
                            Output = "Успешное удаление записи рабочей категории.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Ошибка удаления записи рабочей категории.");
                Output = $"Ошибка удаления записи рабочей категории: {ex.Message}";
                await dialogWindow.ShowDialogOkCancelAsync("Ошибка", Output);
            }
        }
        private bool CanRemoveJobCategoryAsync(object parameter)
        {
            return parameter != null;
        }

        [RelayCommand]
        public async Task UpdateJobCategoryAsync(object parameter)
        {
            try
            {
                if (parameter == null) return;

                if (parameter is JobCategoryModel model)
                {
                    int affectedRows = await context.JobCategory.UpdateAsync(model);
                    if (affectedRows > 0)
                    {
                        logger.Info($"Обновление данных рабочей категории id:{model.Id}");
                        Output = "Успешное обновление данных рабочей категории.";
                        await dialogWindow.ShowDialogOkCancelAsync("Информация.", Output);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Ошибка изменения данных рабочей категории.");
                Output = $"Ошибка изменения записи рабочей категории: {ex.Message}";
                await dialogWindow.ShowDialogOkCancelAsync("Ошибка", Output);
            }
        }
        private bool CanUpdateJobCategoryAsync(object parameter)
        {
            return parameter != null;
        }
        #endregion
    }
}
