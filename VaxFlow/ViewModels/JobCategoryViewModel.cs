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
    public partial class JobCategoryViewModel : ViewModelBase
    {
        public JobCategoryViewModel(DbContext context, IMyLogger logger, IDialogWindow dialogWindow)
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
        private JobCategoryModel? _SelectedJobCategory;
        [ObservableProperty]
        private string _Output = "";
        #endregion

        #region methods
        #endregion

        #region commands
        [RelayCommand]
        public async Task AddJobCategoryAsync(object parameter)
        {
            try
            {
                if (parameter == null) return;

                if (parameter is ObservableCollection<JobCategoryModel> collection)
                {
                    JobCategoryModel newJobCategory = new()
                    {
                        Category = "Работники бюджетной сферы («бюджетники»)",
                        Desc = "Работают в учреждениях, финансируемых из государственного или муниципального бюджета (образование, здравоохранение, культура, госуправление)."
                    };
                    int affectedRows = await context.JobCategory.CreateAsync(newJobCategory);
                    if (affectedRows > 0)
                    {
                        logger.Info($"Создана запись рабочей категории id:{newJobCategory.Id}");
                        collection.Add(newJobCategory);
                        Output = "Успешнове создание новой записи рабочей категории.";
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Ошибка создания записи рабочей категории.");
                Output = $"Ошибка создания записи рабочей категории: {ex.Message}";
                await dialogWindow.ShowDialogOkCancelAsync("Ошибка", Output);
            }
        }
        private bool CanAddJobCategoryAsync(object parameter)
        {
            return parameter != null;
        }

        [RelayCommand]
        public async Task RemoveJobCategoryAsync(object parameter)
        {
            try
            {
                if (SelectedJobCategory != null)
                {
                    if (parameter == null) return;

                    if (parameter is ObservableCollection<JobCategoryModel> collection)
                    {
                        bool result = await dialogWindow.ShowDialogYesNoAsync("Подтверждение удаления.", "Удалить запись рабочей категории?");
                        if (result)
                        {
                            int affectedRows = await context.JobCategory.DeleteAsync(SelectedJobCategory);
                            if (affectedRows > 0)
                            {
                                logger.Info($"Удалена запись рабочей категории id:{SelectedJobCategory.Id}");
                                collection.Remove(SelectedJobCategory);
                                Output = "Успешное удаление записи рабочей категории.";
                            }
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
                if (SelectedJobCategory == null) return;

                if (parameter == null) return;

                if (parameter is ObservableCollection<JobCategoryModel> collection)
                {
                    int affectedRows = await context.JobCategory.UpdateAsync(SelectedJobCategory);
                    if (affectedRows > 0)
                    {
                        logger.Info($"Обновление данных рабочей категории id:{SelectedJobCategory.Id}");
                        int indx = collection.IndexOf(SelectedJobCategory);
                        collection[indx] = SelectedJobCategory;
                        Output = "Успешное обновление данных рабочей категории.";
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
