using Microsoft.Data.Sqlite;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using VaxFlow.Data.Repositories;
using VaxFlow.Models;
using VaxFlow.Services;

namespace VaxFlow.Data.Services
{
    public class VaccineVersionService
    {
        public VaccineVersionService(IAppConfiguration configuration, VaccineVersionRepository repository)
        {
            this.configuration = configuration;
            this.repository = repository;
        }

        #region fields
        private readonly IAppConfiguration configuration;
        private readonly VaccineVersionRepository repository;
        #endregion

        public async Task<int> CreateAsync(VaccineVersionModel doctor)
        {
            using var connection = new SqliteConnection(configuration.DataSourceSQLite);
            await connection.OpenAsync().ConfigureAwait(false);
            return await repository.CreateAsync(connection, doctor);
        }
        public async Task<ObservableCollection<VaccineVersionModel>> GetAllAsync()
        {
            using var connection = new SqliteConnection(configuration.DataSourceSQLite);
            await connection.OpenAsync().ConfigureAwait(false);
            return await repository.GetAllAsync(connection);
        }
        public async Task<int> UpdateAsync(VaccineVersionModel doctor)
        {
            using var connection = new SqliteConnection(configuration.DataSourceSQLite);
            await connection.OpenAsync().ConfigureAwait(false);
            return await repository.UpdateAsync(connection, doctor);
        }
    }
}
