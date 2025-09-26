using Microsoft.Data.Sqlite;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using VaxFlow.Data.Repositories;
using VaxFlow.Models;
using VaxFlow.Services;

namespace VaxFlow.Data.Services
{
    public class VaccineService
    {
        public VaccineService(IAppConfiguration configuration, VaccineRepository repository)
        {
            this.configuration = configuration;
            this.repository = repository;
        }

        #region fields
        private readonly IAppConfiguration configuration;
        private readonly VaccineRepository repository;
        #endregion

        public async Task<int> CreateAsync(VaccineModel model)
        {
            using var connection = new SqliteConnection(configuration.DataSourceSQLite);
            await connection.OpenAsync().ConfigureAwait(false);
            return await repository.CreateAsync(connection, model);
        }
        internal async Task<ObservableCollection<VaccineSummaryModel>> GetAvailableVaccinesAsync()
        {
            using var connection = new SqliteConnection(configuration.DataSourceSQLite);
            await connection.OpenAsync().ConfigureAwait(false);
            return await repository.GetAvailableVaccinesAsync(connection);
        }
        internal async Task<ObservableCollection<VaccineSummaryModel>> FindByVaccineNameAsync(string vaccineName)
        {
            using var connection = new SqliteConnection(configuration.DataSourceSQLite);
            await connection.OpenAsync().ConfigureAwait(false);
            return await repository.FindByVaccineNameAsync(connection, vaccineName);
        }

        public async Task<int> UpdateCountAsync(VaccineModel model)
        {
            using var connection = new SqliteConnection(configuration.DataSourceSQLite);
            await connection.OpenAsync().ConfigureAwait(false);
            return await repository.UpdateCountAsync(connection, model);
        }
        public async Task<int> DeleteAsync(VaccineModel model)
        {
            using var connection = new SqliteConnection(configuration.DataSourceSQLite);
            await connection.OpenAsync().ConfigureAwait(false);
            return await repository.DeleteAsync(connection, model);
        }
        public async Task<int> UpdateAsync(VaccineModel model)
        {
            using var connection = new SqliteConnection(configuration.DataSourceSQLite);
            await connection.OpenAsync().ConfigureAwait(false);
            return await repository.UpdateAsync(connection, model);
        }
    }
}
