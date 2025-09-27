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
    public partial class DoctorViewModel : ViewModelBase
    {
        public DoctorViewModel(DbContext context, IMyLogger logger, IDialogWindow dialogWindow, IListService listService)
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
        private DoctorModel? _SelectedDoctor;
        [ObservableProperty]
        private string _Output = "";
        public ObservableCollection<DoctorModel>? Doctors
        {
            get => listService.Doctors; 
            set => listService.Doctors = value; 
        }
        #endregion

        #region methods
        #endregion

        #region commands
        [RelayCommand]
        public async Task AddDoctorAsync()
        {
            try
            {
                if (Doctors == null) Doctors = [];

                DoctorModel newDoctor = new()
                {
                    FirstName = "Зарема",
                    LastName = "Гусейнова",
                    Patronymic = "Юсуф",
                    NameSuffix = "кызы",
                    IsDismissed = false
                };
                int affectedRows = await context.Doctor.CreateAsync(newDoctor);
                if (affectedRows > 0)
                {
                    logger.Info($"Создана запись доктора id:{newDoctor.Id}");
                    Doctors.Add(newDoctor);
                    Output = "Успешнове создание новой записи доктора.";
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Ошибка создания записи доктора.");
                Output = $"Ошибка создания записи доктора: {ex.Message}";
                await dialogWindow.ShowDialogOkCancelAsync("Ошибка", Output);
            }
        }

        [RelayCommand]
        public async Task RemoveDoctorAsync(object parameter)
        {
            try
            {
                if (parameter == null) return;

                if (parameter is DoctorModel model)
                {
                    bool result = await dialogWindow.ShowDialogYesNoAsync("Подтверждение удаления.", "Удалить запись доктора?");
                    if (result)
                    {
                        int affectedRows = await context.Doctor.DeleteAsync(model);
                        if (affectedRows > 0)
                        {
                            logger.Info($"Удалена запись доктора id:{model.Id}");
                            Doctors.Remove(model);
                            Output = "Успешное удаление записи доктора.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Ошибка удаления записи доктора.");
                Output = $"Ошибка удаления записи доктора: {ex.Message}";
                await dialogWindow.ShowDialogOkCancelAsync("Ошибка", Output);
            }
        }
        private bool CanRemoveDoctorAsync(object parameter)
        {
            return parameter != null;
        }

        [RelayCommand]
        public async Task UpdateDoctorAsync(object parameter)
        {
            try
            {
                if (parameter == null) return;

                if (parameter is DoctorModel model)
                {
                    int affectedRows = await context.Doctor.UpdateAsync(model);
                    if (affectedRows > 0)
                    {
                        logger.Info($"Обновление данных доктора id:{model.Id}");
                        Output = "Успешное обновление данных доктора.";
                        await dialogWindow.ShowDialogOkCancelAsync("Информация.", Output);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Ошибка изменения данных доктора.");
                Output = $"Ошибка изменения записи доктора: {ex.Message}";
                await dialogWindow.ShowDialogOkCancelAsync("Ошибка", Output);
            }
        }
        private bool CanUpdateDoctorAsync(object parameter)
        {
            return parameter != null;
        }
        #endregion
    }
}
