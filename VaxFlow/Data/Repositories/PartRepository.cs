using Microsoft.Data.Sqlite;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using VaxFlow.Models;

namespace VaxFlow.Data.Repositories
{
    public class PartRepository
    {
        public async Task<int> CreateAsync(SqliteConnection connection, PartModel model)
        {
            using var cmd = connection.CreateCommand();
            cmd.CommandText = @"
                INSERT INTO parts (pattern_id, serial_number, body, desc, is_url, url, is_bold, is_italic, is_underline)
                VALUES (@pattern_id, @serial_number, @body, @desc, @is_url, @url, @is_bold, @is_italic, @is_underline);
                SELECT last_insert_rowid();";
            cmd.Parameters.AddWithValue("@pattern_id", model.PatternId);
            cmd.Parameters.AddWithValue("@serial_number", model.SerialNumber);
            cmd.Parameters.AddWithValue("@body", model.Body);
            cmd.Parameters.AddWithValue("@desc", SqliteHelper.FromStringNull(model.Desc));
            cmd.Parameters.AddWithValue("@is_url", SqliteHelper.FromBoolean(model.IsUrl));
            cmd.Parameters.AddWithValue("@url", SqliteHelper.FromStringNull(model.Url));
            cmd.Parameters.AddWithValue("@is_bold", SqliteHelper.FromBoolean(model.IsBold));
            cmd.Parameters.AddWithValue("@is_italic", SqliteHelper.FromBoolean(model.IsItalic));
            cmd.Parameters.AddWithValue("@is_underline", SqliteHelper.FromBoolean(model.IsUnderline));
            var id = await cmd.ExecuteScalarAsync().ConfigureAwait(false);
            model.Id = Convert.ToInt32(id);
            return model.Id;
        }
        public async Task<ObservableCollection<PartModel>> FindByPatternIdAsync(SqliteConnection connection, int patternId)
        {
            ObservableCollection<PartModel> items = [];
            using var cmd = connection.CreateCommand();
            cmd.CommandText = @"
                SELECT 
                    id, pattern_id, serial_number, body, desc, is_url, url, is_bold, is_italic, is_underline
                FROM parts
                WHERE pattern_id=@pattern_id;";
            cmd.Parameters.AddWithValue("@pattern_id", patternId);
            using var reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false);            
            while (await reader.ReadAsync().ConfigureAwait(false))
            {
                items.Add(new()
                {
                    Id = reader.GetInt32(0),
                    PatternId = reader.GetInt32(1),
                    SerialNumber = reader.GetInt32(2),
                    Body = reader.GetString(3),
                    Desc = SqliteHelper.GetStringNull(reader, 4),
                    IsUrl = SqliteHelper.GetBoolean(reader, 5),
                    Url = SqliteHelper.GetStringNull(reader, 6),
                    IsBold = SqliteHelper.GetBoolean(reader, 7),
                    IsItalic = SqliteHelper.GetBoolean(reader, 8),
                    IsUnderline = SqliteHelper.GetBoolean(reader, 9)
                });
            }
            return items;
        }
        public async Task<int> UpdateAsync(SqliteConnection connection, PartModel model)
        {
            using var cmd = connection.CreateCommand();
            cmd.CommandText = @"
                UPDATE parts
                SET 
                    pattern_id=@pattern_id, serial_number=@serial_number, body=@body, 
                    desc=@desc, is_url=@is_url, url=@url,
                    is_bold=@is_bold, is_italic=@is_italic, is_underline=@is_underline
                WHERE id=@id;";
            cmd.Parameters.AddWithValue("@pattern_id", model.PatternId);
            cmd.Parameters.AddWithValue("@serial_number", model.SerialNumber);
            cmd.Parameters.AddWithValue("@body", model.Body);
            cmd.Parameters.AddWithValue("@desc", SqliteHelper.FromStringNull(model.Desc));
            cmd.Parameters.AddWithValue("@is_url", SqliteHelper.FromBoolean(model.IsUrl));
            cmd.Parameters.AddWithValue("@url", SqliteHelper.FromStringNull(model.Url));
            cmd.Parameters.AddWithValue("@is_bold", SqliteHelper.FromBoolean(model.IsBold));
            cmd.Parameters.AddWithValue("@is_italic", SqliteHelper.FromBoolean(model.IsItalic));
            cmd.Parameters.AddWithValue("@is_underline", SqliteHelper.FromBoolean(model.IsUnderline));
            cmd.Parameters.AddWithValue("@id", model.Id);
            return await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
        }
        public async Task<int> DeleteAsync(SqliteConnection connection, PartModel model)
        {
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "DELETE FROM parts WHERE id=@id;";
            cmd.Parameters.AddWithValue("@id", model.Id);
            return await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
        }
    }
}
