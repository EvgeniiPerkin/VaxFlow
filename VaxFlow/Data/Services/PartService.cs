using Microsoft.Data.Sqlite;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using VaxFlow.Data.Repositories;
using VaxFlow.Models;
using VaxFlow.Services;

namespace VaxFlow.Data.Services
{
    public class PartService
    {
        public PartService(IAppConfiguration configuration, PartRepository repository)
        {
            this.configuration = configuration;
            this.repository = repository;
        }

        #region fields
        private readonly IAppConfiguration configuration;
        private readonly PartRepository repository;
        #endregion

        internal async Task<int> CreateAsync(PartModel value)
        {
            using var connection = new SqliteConnection(configuration.DataSourceSQLite);
            await connection.OpenAsync().ConfigureAwait(false);
            return await repository.CreateAsync(connection, value);
        }

        internal async Task<int> DeleteAsync(PartModel selectedPart)
        {
            using var connection = new SqliteConnection(configuration.DataSourceSQLite);
            await connection.OpenAsync().ConfigureAwait(false);
            return await repository.DeleteAsync(connection, selectedPart);
        }

        internal async Task<ObservableCollection<PartModel>> FindByPatternIdAsync(int patternId)
        {
            using var connection = new SqliteConnection(configuration.DataSourceSQLite);
            await connection.OpenAsync().ConfigureAwait(false);
            return await repository.FindByPatternIdAsync(connection, patternId);
        }

        internal async Task<int> UpdateAsync(PartModel selectedPart)
        {
            using var connection = new SqliteConnection(configuration.DataSourceSQLite);
            await connection.OpenAsync().ConfigureAwait(false);
            return await repository.UpdateAsync(connection, selectedPart);
        }
    }
}
