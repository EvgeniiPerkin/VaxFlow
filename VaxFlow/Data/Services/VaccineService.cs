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

        public async Task<int> CreateAsync(VaccineModel doctor)
        {
            using var connection = new SqliteConnection(configuration.DataSourceSQLite);
            await connection.OpenAsync().ConfigureAwait(false);
            return await repository.CreateAsync(connection, doctor);
        }
        public async Task<ObservableCollection<VaccineModel>> GetAllAsync()
        {
            using var connection = new SqliteConnection(configuration.DataSourceSQLite);
            await connection.OpenAsync().ConfigureAwait(false);
            return await repository.GetAllAsync(connection);
        }
        public async Task<int> UpdateAsync(VaccineModel doctor)
        {
            using var connection = new SqliteConnection(configuration.DataSourceSQLite);
            await connection.OpenAsync().ConfigureAwait(false);
            return await repository.UpdateAsync(connection, doctor);
        }
    }
}
