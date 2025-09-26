using DocumentFormat.OpenXml.Bibliography;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using VaxFlow.Data.Repositories;
using VaxFlow.Models;
using VaxFlow.Services;

namespace VaxFlow.Data.Services
{
    public class PatientVaccinationDataProcessingService
    {
        public PatientVaccinationDataProcessingService(
            IAppConfiguration configuration,
            PatientRepository patientRepository,
            VaccinationRepository vaccinationRepository)
        {
            this.configuration = configuration;
            this.patientRepository = patientRepository;
            this.vaccinationRepository = vaccinationRepository;
        }

        #region fields
        private readonly IAppConfiguration configuration;
        private readonly PatientRepository patientRepository;
        private readonly VaccinationRepository vaccinationRepository;
        #endregion
        public async Task<int> CreatePatientAsync(PatientModel model)
        {
            using var connection = new SqliteConnection(configuration.DataSourceSQLite);
            await connection.OpenAsync().ConfigureAwait(false);
            return await patientRepository.CreateAsync(connection, model);
        }
        public async Task<int> CreateVaccinationAsync(VaccinationModel model)
        {
            using var connection = new SqliteConnection(configuration.DataSourceSQLite);
            await connection.OpenAsync().ConfigureAwait(false);
            return await vaccinationRepository.CreateAsync(connection, model);
        }
        public async Task<ObservableCollection<VaccinationSummaryModel>> FindVaccinationByPatientIdAsync(int patientId)
        {
            using var connection = new SqliteConnection(configuration.DataSourceSQLite);
            await connection.OpenAsync().ConfigureAwait(false);
            return await vaccinationRepository.FindByPatientId(connection, patientId);
        }
        public async Task<ObservableCollection<PatientModel>> FilteredPatientsByDateCreateAsync(DateTime from, DateTime? to = null)
        {
            using var connection = new SqliteConnection(configuration.DataSourceSQLite);
            await connection.OpenAsync().ConfigureAwait(false);
            return await patientRepository.FilteredByDateCreateAsync(connection, from, to);
        }
        public async Task<ObservableCollection<PatientModel>> FindByInitialsOrPolicyNumberAsync(string searchStr)
        {
            using var connection = new SqliteConnection(configuration.DataSourceSQLite);
            await connection.OpenAsync().ConfigureAwait(false);
            return await patientRepository.FindByInitialsOrPolicyNumberAsync(connection, searchStr);
        }
        public async Task<int> UpdatePatientAsync(PatientModel model)
        {
            using var connection = new SqliteConnection(configuration.DataSourceSQLite);
            await connection.OpenAsync().ConfigureAwait(false);
            return await patientRepository.UpdateAsync(connection, model);
        }
        public async Task<int> DeletePatientAsync(PatientModel model)
        {
            using var connection = new SqliteConnection(configuration.DataSourceSQLite);
            await connection.OpenAsync().ConfigureAwait(false);
            return await patientRepository.DeleteAsync(connection, model);
        }
        public async Task<int> DeleteVaccinationAsync(VaccinationModel model)
        {
            using var connection = new SqliteConnection(configuration.DataSourceSQLite);
            await connection.OpenAsync().ConfigureAwait(false);
            return await vaccinationRepository.DeleteAsync(connection, model);
        }
    }
}
