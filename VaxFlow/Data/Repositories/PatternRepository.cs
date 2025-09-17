using Microsoft.Data.Sqlite;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using VaxFlow.Models;

namespace VaxFlow.Data.Repositories
{
    public class PatternRepository
    {
        public async Task<ObservableCollection<PatternModel>> GetAllAsync(SqliteConnection connection)
        {
            ObservableCollection<PatternModel> items = [];
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT id, desc FROM patterns;";
            using var reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false);
            while (await reader.ReadAsync().ConfigureAwait(false))
            {
                items.Add(new()
                {
                    Id = reader.GetInt32(0),
                    Desc = reader.GetString(1)
                });
            }
            return items;
        }
    }
}
