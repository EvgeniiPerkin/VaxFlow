using Microsoft.Data.Sqlite;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using VaxFlow.Models;

namespace VaxFlow.Data.Repositories
{
    public class JobCategoryRepository
    {
        public async Task<int> CreateAsync(SqliteConnection connection, JobCategoryModel jc)
        {
            using var cmd = connection.CreateCommand();
            cmd.CommandText = @"
                INSERT INTO job_categories (category, desc)
                VALUES (@category, @desc);
                SELECT last_insert_rowid();";
            cmd.Parameters.AddWithValue("@category", jc.Category);
            cmd.Parameters.AddWithValue("@desc", jc.Desc);
            var id = await cmd.ExecuteScalarAsync().ConfigureAwait(false);
            jc.Id = Convert.ToInt32(id);
            return jc.Id;
        }
        public async Task<ObservableCollection<JobCategoryModel>> GetAllAsync(SqliteConnection connection)
        {
            ObservableCollection<JobCategoryModel> items = [];
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT id, category, desc FROM job_categories;";
            using var reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false);
            while (await reader.ReadAsync().ConfigureAwait(false))
            {
                items.Add(new()
                {
                    Id = reader.GetInt32(0),
                    Category = reader.GetString(1),
                    Desc = reader.GetString(2)
                });
            }
            return items;
        }
        public async Task<int> UpdateAsync(SqliteConnection connection, JobCategoryModel jc)
        {
            using var cmd = connection.CreateCommand();
            cmd.CommandText = @"
                UPDATE job_categories
                SET category=@category, desc=@desc
                WHERE id=@id;";
            cmd.Parameters.AddWithValue("@category", jc.Category);
            cmd.Parameters.AddWithValue("@desc", jc.Desc);
            cmd.Parameters.AddWithValue("@id", jc.Id);
            return await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
        }
        public async Task<int> DeleteAsync(SqliteConnection connection, JobCategoryModel jc)
        {
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "DELETE FROM job_categories WHERE id=@id;";
            cmd.Parameters.AddWithValue("@id", jc.Id);
            return await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
        }
    }
}
