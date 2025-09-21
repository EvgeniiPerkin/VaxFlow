using Microsoft.Data.Sqlite;
using System.Threading.Tasks;
using VaxFlow.Data.Repositories;
using VaxFlow.Models;
using VaxFlow.Services;

namespace VaxFlow.Data.Services
{
    public class PartyService
    {
        public PartyService(IAppConfiguration configuration, PartyRepository repository)
        {
            this.configuration = configuration;
            this.repository = repository;
        }

        #region fields
        private readonly IAppConfiguration configuration;
        private readonly PartyRepository repository;
        #endregion

        public async Task<int> CreateAsync(PartyModel model)
        {
            using var connection = new SqliteConnection(configuration.DataSourceSQLite);
            await connection.OpenAsync().ConfigureAwait(false);
            return await repository.CreateAsync(connection, model);
        }
        public async Task<int> UpdateCountAsync(PartyModel model)
        {
            using var connection = new SqliteConnection(configuration.DataSourceSQLite);
            await connection.OpenAsync().ConfigureAwait(false);
            return await repository.UpdateCountAsync(connection, model);
        }
        public async Task<int> DeleteAsync(PartyModel model)
        {
            using var connection = new SqliteConnection(configuration.DataSourceSQLite);
            await connection.OpenAsync().ConfigureAwait(false);
            return await repository.DeleteAsync(connection, model);
        }
    }
}
