using Microsoft.Data.Sqlite;
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
                    CREATE TABLE IF NOT EXISTS [doctors] (
                        id INTEGER NOT NULL,
                        last_name TEXT NOT NULL,
                        first_name TEXT NOT NULL,
                        patronymic TEXT,
                        name_suffix TEXT,
                        PRIMARY KEY(id AUTOINCREMENT)
                    );
                    CREATE TABLE IF NOT EXISTS [job_categories] (
                        id INTEGER NOT NULL,
                        desc TEXT NOT NULL,
                        PRIMARY KEY(id AUTOINCREMENT)
                    );
                    CREATE TABLE IF NOT EXISTS [vaccines] (
                        id INTEGER NOT NULL,
                        desc TEXT NOT NULL,
                        PRIMARY KEY(id AUTOINCREMENT)
                    );
                    CREATE TABLE IF NOT EXISTS [parties] (
                        id INTEGER NOT NULL,
                        vaccine_id INTEGER NOT NULL REFERENCES [vaccines](id) ON DELETE CASCADE ON UPDATE CASCADE,
                        party_name TEXT NOT NULL,
                        count INTEGER NOT NULL DEFAULT 0,
                        dt_create DATETIME NOT NULL,
                        PRIMARY KEY(id AUTOINCREMENT)
                    );
                    CREATE TABLE IF NOT EXISTS [patients] (
                        id INTEGER NOT NULL,
                        last_name TEXT NOT NULL,
                        first_name TEXT NOT NULL,
                        patronymic TEXT,
                        name_suffix TEXT,
                        birthday DATE NOT NULL,
                        registration_address TEXT NOT NULL,
                        dt_create DATETIME NOT NULL,
                        policy_number TEXT NOT NULL,
                        working_position TEXT NOT NULL,
                        job_category_id INTEGER NOT NULL REFERENCES [job_categories](id) ON DELETE CASCADE ON UPDATE CASCADE,
                        PRIMARY KEY(id AUTOINCREMENT)
                    );
                    CREATE TABLE IF NOT EXISTS [doctors_appointments] (
                        doctor_id INTEGER NOT NULL REFERENCES [doctors](id) ON DELETE CASCADE ON UPDATE CASCADE,
                        patient_id INTEGER NOT NULL REFERENCES [patients](id) ON DELETE CASCADE ON UPDATE CASCADE,
                        vaccine_id INTEGER NOT NULL REFERENCES [vaccines](id) ON DELETE CASCADE ON UPDATE CASCADE,
                        dt_of_appointment DATETIME NOT NULL,
                        PRIMARY KEY (doctor_id, patient_id, vaccine_id)
                    );
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
                        is_bold BOOLEAN NOT NULL CHECK (is_bold IN (0, 1)),
                        is_italic BOOLEAN NOT NULL CHECK (is_italic IN (0, 1)),
                        is_underline BOOLEAN NOT NULL CHECK (is_underline IN (0, 1)),
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
