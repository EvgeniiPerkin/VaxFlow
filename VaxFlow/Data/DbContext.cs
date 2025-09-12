using DocumentFormat.OpenXml.Office2010.CustomUI;
using Microsoft.Data.Sqlite;
using System.Text;
using System.Threading.Tasks;
using VaxFlow.Services;

namespace VaxFlow.Data
{
    public class DbContext
    {
        public DbContext(IAppConfiguration configuration)
        {
            this.configuration = configuration;
        }

        #region Properties
        private readonly IAppConfiguration configuration;
        #endregion

        public async Task<int> SetupAsync()
        {
            try
            {
                string sql = @"
                    CREATE TABLE IF NOT EXISTS [patterns] ( 
                        id INTEGER NOT NULL,
                        desc TEXT,
                        PRIMARY KEY(id AUTOINCREMENT)
                    );

                    CREATE TABLE IF NOT EXISTS [parts] ( 
                        id INTEGER NOT NULL,
                        pattern_id INTEGER NOT NULL REFERENCES [patterns](id) ON DELETE CASCADE ON UPDATE CASCADE,
                        serial_number INTEGER NOT NULL,
                        body TEXT NOT NULL,
                        desc TEXT,
                        is_url BOOLEAN NOT NULL CHECK (is_url IN (0, 1)),
                        url TEXT,
                        PRIMARY KEY(id AUTOINCREMENT)
                    );
                ";

                using var connection = new SqliteConnection(configuration.DataSourceSQLite);
                connection.Open();

                using var command = connection.CreateCommand();
                command.CommandText = sql;

                await command.ExecuteNonQueryAsync().ConfigureAwait(false);

                return 0;
            }
            catch
            {
                return -1;
            }
        }
    }
}
