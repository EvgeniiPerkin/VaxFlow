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
    public partial class VaccinationJournalViewModel : ViewModelBase
    {
        public VaccinationJournalViewModel() { }
        public VaccinationJournalViewModel(DbContext context, IMyLogger logger, IDialogWindow dialogWindow)
        {
            this.context = context;
            this.logger = logger;
            this.dialogWindow = dialogWindow;
            var dtn = DateTime.Now;
            DtFrom = new DateTime(dtn.Year, dtn.Month, dtn.Day);
            _ = OnLoadAsync();
        }

        #region fields
        private readonly DbContext context;
        private readonly IMyLogger logger;
        private readonly IDialogWindow dialogWindow;
        #endregion

        #region properties
        [ObservableProperty]
        private string _SearchStr = "";

        [ObservableProperty]
        private DateTimeOffset _DtFrom;

        [ObservableProperty]
        private DateTimeOffset? _DtTo;

        [ObservableProperty]
        private ObservableCollection<PatientSummaryModel>? _Patients;

        [ObservableProperty]
        private PatientSummaryModel? _SelectedPatient;
        #endregion

        #region methods
        private async Task OnLoadAsync()
        {
            try
            {
                Patients = await context.PatientVaccinationDataProcessing
                    .FilteredPatientsByDateCreateAsync(DtFrom.DateTime);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Ошибка загрузки данных списков по умолчанию.");
            }
        }
        #endregion

        #region commands
        [RelayCommand]
        public async Task SearchAsync()
        {
            try
            {
                if (DtTo == null)
                {
                    Patients = await context.PatientVaccinationDataProcessing
                        .FilteredPatientsByDateCreateAsync(DtFrom.DateTime);
                }
                else
                {
                    Patients = await context.PatientVaccinationDataProcessing
                        .FilteredPatientsByDateCreateAsync(DtFrom.DateTime, DtTo.Value.DateTime);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Ошибка поиска пациента.");
                await dialogWindow.ShowDialogOkCancelAsync("Ошибка", "Ошибка поиска пациентов");
            }
        }
        //private bool CanAddDoctorAsync(object parameter)
        //{
        //    return parameter != null;
        //}

        //[RelayCommand]
        //public async Task RemoveDoctorAsync(object parameter)
        //{
        //    try
        //    {
        //        if (SelectedDoctor != null)
        //        {
        //            if (parameter == null) return;

        //            if (parameter is ObservableCollection<DoctorModel> collection)
        //            {
        //                bool result = await dialogWindow.ShowDialogYesNoAsync("Подтверждение удаления.", "Удалить запись доктора?");
        //                if (result)
        //                {
        //                    int affectedRows = await context.Doctor.DeleteAsync(SelectedDoctor);
        //                    if (affectedRows > 0)
        //                    {
        //                        logger.Info($"Удалена запись доктора id:{SelectedDoctor.Id}");
        //                        collection.Remove(SelectedDoctor);
        //                        Output = "Успешное удаление записи доктора.";
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Error(ex, "Ошибка удаления записи доктора.");
        //        Output = $"Ошибка удаления записи доктора: {ex.Message}";
        //        await dialogWindow.ShowDialogOkCancelAsync("Ошибка", Output);
        //    }
        //}
        //private bool CanRemoveDoctorAsync(object parameter)
        //{
        //    return parameter != null;
        //}

        //[RelayCommand]
        //public async Task UpdateDoctorAsync(object parameter)
        //{
        //    try
        //    {
        //        if (SelectedDoctor == null) return;

        //        if (parameter == null) return;

        //        if (parameter is ObservableCollection<DoctorModel> collection)
        //        {
        //            int affectedRows = await context.Doctor.UpdateAsync(SelectedDoctor);
        //            if (affectedRows > 0)
        //            {
        //                logger.Info($"Обновление данных доктора id:{SelectedDoctor.Id}");
        //                int indx = collection.IndexOf(SelectedDoctor);
        //                collection[indx] = SelectedDoctor;
        //                Output = "Успешное обновление данных доктора.";
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Error(ex, "Ошибка изменения данных доктора.");
        //        Output = $"Ошибка изменения записи доктора: {ex.Message}";
        //        await dialogWindow.ShowDialogOkCancelAsync("Ошибка", Output);
        //    }
        //}
        //private bool CanUpdateDoctorAsync(object parameter)
        //{
        //    return parameter != null;
        //}
        #endregion
    }
}
