using Microsoft.Data.Sqlite;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using VaxFlow.Data.Repositories;
using VaxFlow.Models;
using VaxFlow.Services;

namespace VaxFlow.Data.Services
{
    public class JobCategoryService
    {
        public JobCategoryService(IAppConfiguration configuration, JobCategoryRepository repository)
        {
            this.configuration = configuration;
            this.repository = repository;
        }

        #region fields
        private readonly IAppConfiguration configuration;
        private readonly JobCategoryRepository repository;
        #endregion

        public async Task<int> CreateAsync(JobCategoryModel doctor)
        {
            using var connection = new SqliteConnection(configuration.DataSourceSQLite);
            await connection.OpenAsync().ConfigureAwait(false);
            return await repository.CreateAsync(connection, doctor);
        }
        public async Task<ObservableCollection<JobCategoryModel>> GetAllAsync()
        {
            using var connection = new SqliteConnection(configuration.DataSourceSQLite);
            await connection.OpenAsync().ConfigureAwait(false);
            return await repository.GetAllAsync(connection);
        }
        public async Task<int> UpdateAsync(JobCategoryModel doctor)
        {
            using var connection = new SqliteConnection(configuration.DataSourceSQLite);
            await connection.OpenAsync().ConfigureAwait(false);
            return await repository.UpdateAsync(connection, doctor);
        }

        public async Task<int> DeleteAsync(JobCategoryModel doctor)
        {
            using var connection = new SqliteConnection(configuration.DataSourceSQLite);
            await connection.OpenAsync().ConfigureAwait(false);
            return await repository.DeleteAsync(connection, doctor);
        }
    }
}
