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
        public VaccinationJournalViewModel(DbContext context, IMyLogger logger, IDialogWindow dialogWindow, IListService listService)
        {
            this.context = context;
            this.logger = logger;
            this.dialogWindow = dialogWindow;
            this.ListService = listService;
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
        public IListService ListService { get; }

        [ObservableProperty]
        private string _SearchStr = "";

        [ObservableProperty]
        private DateTimeOffset _DtFrom;

        [ObservableProperty]
        private DateTimeOffset? _DtTo;

        [ObservableProperty]
        private ObservableCollection<PatientModel>? _Patients;

        private PatientModel? _SelectedPatient;
        public PatientModel? SelectedPatient
        {
            get { return _SelectedPatient; }
            set
            {
                SetProperty(ref _SelectedPatient, value);
                if (_SelectedPatient != null)
                {
                    try
                    {
                        _SelectedPatient.Vaccinations = context.PatientVaccinationDataProcessing
                            .FindVaccinationByPatientIdAsync(_SelectedPatient.Id).Result;
                    }
                    catch(Exception ex)
                    {
                        logger.Error(ex, "Ошибка поиска вакцинации пациента по Id.");
                    }
                }
                IsEnabledSelectedPatient = value != null;
            }
        }

        [ObservableProperty]
        private bool _IsEnabledSelectedPatient;

        [ObservableProperty]
        private VaccinationSummaryModel? _SelectedVaccination;

        private VaccinationModel? _Vaccination;

        /// <summary> Description </summary>
        public VaccinationModel? Vaccination
        {
            get { return _Vaccination; }
            set
            {
                SetProperty(ref _Vaccination, value);
                IsEnabledVacctination = _Vaccination != null;
            }
        }

        [ObservableProperty]
        private bool _IsEnabledVacctination;
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
        public async Task SearchByDateAsync()
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
                logger.Error(ex, "Ошибка поиска пациента по датам.");
                await dialogWindow.ShowDialogOkCancelAsync("Ошибка", "Ошибка поиска пациентов по датам:" + ex.Message);
            }
            finally
            {
                SelectedPatient = null;
            }
        }

        [RelayCommand]
        public async Task SearchByFullNameAsync()
        {
            try
            {
                if (!string.IsNullOrEmpty(SearchStr))
                {
                    Patients = await context.PatientVaccinationDataProcessing
                        .FindByInitialsOrPolicyNumberAsync(SearchStr);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Ошибка поиска пациента по фио.");
                await dialogWindow.ShowDialogOkCancelAsync("Ошибка", "Ошибка поиска пациентов по фио:" + ex.Message );
            }
            finally
            {
                SelectedPatient = null;
            }
        }

        [RelayCommand]
        public async Task CreatePatientAsync()
        {
            try
            {
                int affectedRows = await context.PatientVaccinationDataProcessing
                    .CreatePatientAsync(new PatientModel()
                    {
                        FullName = "Иванов_Иван_Иванович",
                        DateTimeCreate = DateTime.Now,
                        Birthday = new DateTime(2000, 1, 1),
                        PolicyNumber = "0000 0000 0000 0000",
                        RegistrationAddress = "Москва",
                        WorkingPosition = "Слесарь",
                        JobCategoryId = 1
                    });
                if (affectedRows > 0)
                {
                    var dtn = DateTime.Now;
                    DtFrom = new DateTime(dtn.Year, dtn.Month, dtn.Day);
                    DtTo = null;
                    SearchStr = string.Empty;
                    await OnLoadAsync();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Ошибка создания пустой записи пациента.");
                await dialogWindow.ShowDialogOkCancelAsync("Ошибка",
                    "Ошибка создания пустой записи пациента:" + ex.Message);
            }
            finally
            {
                SelectedPatient = null;
            }
        }

        [RelayCommand]
        public async Task DeletePatientAsync(object parameter)
        {
            try
            {
                bool result = await dialogWindow.ShowDialogYesNoAsync("Подтверждение удаления.",
                    "Удалить безвозвратно данные пациента?\n" +
                    "Если у пациента были записи вакцинации, они так же будут удалены," +
                    "что приведет к искажению отчетности.");
                if (!result) return;

                if (parameter is PatientModel p)
                {
                    int affectedRows = await context.PatientVaccinationDataProcessing
                        .DeletePatientAsync(p);

                    if (affectedRows > 0)
                    {
                        Patients?.Remove(p);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Ошибка удаления записи пациента.");
                await dialogWindow.ShowDialogOkCancelAsync("Ошибка",
                    "Ошибка удаления записи пациента:" + ex.Message);
            }
            finally
            {
                SelectedPatient = null;
            }
        }
        private bool CanDeletePatientAsync(object parameter)
        {
            return parameter != null;
        }

        [RelayCommand]
        public async Task SavePatientAsync(object parameter)
        {
            try
            {
                if (parameter is PatientModel p)
                {
                    int affectedRows = await context.PatientVaccinationDataProcessing
                        .UpdatePatientAsync(p);

                    if (affectedRows > 0)
                    {
                        await dialogWindow.ShowDialogOkCancelAsync("Сохранение данных", "Данные пациента успешно обновлены.");
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Ошибка изменения записи пациента.");
                await dialogWindow.ShowDialogOkCancelAsync("Ошибка",
                    "Ошибка изменения записи пациента:" + ex.Message);
            }
        }
        private bool CanSavePatientAsync(object parameter)
        {
            return parameter != null;
        }

        [RelayCommand]
        public async Task SetVaccinationAsync(object parameter)
        {
            try
            {
                if (SelectedPatient == null) return;

                if (parameter is VaccinationModel v)
                {
                    int affectedRows = await context.PatientVaccinationDataProcessing
                        .CreateVaccinationAsync(v);
                    if (affectedRows > 0)
                    {
                        SelectedPatient.Vaccinations = await context.PatientVaccinationDataProcessing
                            .FindVaccinationByPatientIdAsync(SelectedPatient.Id);
                        await ListService.RefreshAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Ошибка создания записи вакцинации пациента.");
                await dialogWindow.ShowDialogOkCancelAsync("Ошибка",
                    "Ошибка создания записи вакцинации пациента:" + ex.Message);
            }
            finally
            {
                Vaccination = null;
                SelectedVaccination = null;
            }
        }
        private bool CanSetVaccinationAsync(object parameter)
        {
            return parameter != null;
        }

        [RelayCommand]
        public async Task RemoveVaccinationAsync(object parameter)
        {
            try
            {
                if (SelectedPatient == null) return;

                if (parameter is VaccinationSummaryModel v)
                {
                    int affectedRows = await context.PatientVaccinationDataProcessing
                        .DeleteVaccinationAsync(v);
                    if (affectedRows > 0)
                    {
                        SelectedPatient.Vaccinations = await context.PatientVaccinationDataProcessing
                            .FindVaccinationByPatientIdAsync(SelectedPatient.Id);
                        await ListService.RefreshAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Ошибка удаления записи вакцинации пациента.");
                await dialogWindow.ShowDialogOkCancelAsync("Ошибка",
                    "Ошибка удаления записи вакцинации пациента:" + ex.Message);
            }
            finally
            {
                Vaccination = null;
                SelectedVaccination = null;
            }
        }
        private bool CanRemoveVaccinationAsync(object parameter)
        {
            return parameter != null;
        }

        [RelayCommand]
        public void CreateVaccination()
        {
            if (SelectedPatient == null) return;

            Vaccination = new()
            {
                DoctorId = 0,
                VaccineId = 0,
                PatientId = SelectedPatient.Id,
                DateTimeOfVaccination = DateTime.Now,
            };
        }
        private bool CanCreateVaccination(object parameter)
        {
            return parameter != null;
        }
        #endregion
    }
}
