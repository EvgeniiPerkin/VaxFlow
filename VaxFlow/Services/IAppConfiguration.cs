namespace VaxFlow.Services
{
    public interface IAppConfiguration
    {
        /// <summary>
        /// инициализация конфига
        /// </summary>
        void Init();
        /// <summary> Строка подключения к бд sqlite </summary>
        string? DataSourceSQLite { get; set; }
        /// <summary> Путь к лог файлу </summary>
        string? PathToLogFile { get; set; }
    }
}
