using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using VaxFlow.Data;
using VaxFlow.Models;
using VaxFlow.Services;

namespace VaxFlow.ViewModels
{
    public partial class DoctorViewModel : ViewModelBase
    {
        public DoctorViewModel(DbContext context, IMyLogger logger)
        {
            this.context = context;
            this.logger = logger;
        }

        #region fields
        private readonly DbContext context;
        private readonly IMyLogger logger;
        #endregion

        #region properties
        [ObservableProperty]
        private DoctorModel? _SelectedDoctor;
        #endregion

        #region methods
        #endregion

        #region commands
        [RelayCommand]
        public async Task AddDoctorAsync(object parameter)
        {
            try
            {
                if (parameter == null) return;

                if (parameter is ObservableCollection<DoctorModel> collection)
                {
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
                    }
                    collection.Add(newDoctor);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Ошибка создания записи доктора.");
            }
        }
        private bool CanAddDoctorAsync(object parameter)
        {
            return parameter != null;
        }

        [RelayCommand]
        public async Task RemoveDoctorAsync(object parameter)
        {
            try
            {
                if (SelectedDoctor != null)
                {
                    if (parameter == null) return;

                    if (parameter is ObservableCollection<DoctorModel> collection)
                    {
                        int affectedRows = await context.Doctor.DeleteAsync(SelectedDoctor);
                        if (affectedRows > 0)
                        {
                            logger.Info($"Удалена запись доктора id:{SelectedDoctor.Id}");
                        }
                        collection.Remove(SelectedDoctor);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Ошибка удаления записи доктора.");
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
                if (SelectedDoctor == null) return;

                if (parameter == null) return;

                if (parameter is ObservableCollection<DoctorModel> collection)
                {
                    int affectedRows = await context.Doctor.UpdateAsync(SelectedDoctor);
                    if (affectedRows > 0)
                    {
                        logger.Info($"Обновление данных доктора id:{SelectedDoctor.Id}");
                        int indx = collection.IndexOf(SelectedDoctor);
                        collection[indx] = SelectedDoctor;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Ошибка изменения данных доктора.");
            }
        }
        private bool CanUpdateDoctorAsync(object parameter)
        {
            return parameter != null;
        }
        #endregion
    }
}
