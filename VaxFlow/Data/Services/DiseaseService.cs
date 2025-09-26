using Microsoft.Data.Sqlite;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using VaxFlow.Data.Repositories;
using VaxFlow.Models;
using VaxFlow.Services;

namespace VaxFlow.Data.Services
{
    public class DiseaseService
    {
        public DiseaseService(IAppConfiguration configuration, DiseaseRepository repository)
        {
            this.configuration = configuration;
            this.repository = repository;
        }

        #region fields
        private readonly IAppConfiguration configuration;
        private readonly DiseaseRepository repository;
        #endregion

        public async Task<int> CreateAsync(DiseaseModel doctor)
        {
            using var connection = new SqliteConnection(configuration.DataSourceSQLite);
            await connection.OpenAsync().ConfigureAwait(false);
            return await repository.CreateAsync(connection, doctor);
        }
        public async Task<ObservableCollection<DiseaseModel>> GetAllAsync()
        {
            using var connection = new SqliteConnection(configuration.DataSourceSQLite);
            await connection.OpenAsync().ConfigureAwait(false);
            return await repository.GetAllAsync(connection);
        }
        public async Task<int> UpdateAsync(DiseaseModel doctor)
        {
            using var connection = new SqliteConnection(configuration.DataSourceSQLite);
            await connection.OpenAsync().ConfigureAwait(false);
            return await repository.UpdateAsync(connection, doctor);
        }
    }
}
