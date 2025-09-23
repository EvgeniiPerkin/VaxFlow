using System;
using System.IO;

namespace VaxFlow.Services
{
    public class AppConfiguration : IAppConfiguration
    {
        public string? DataSourceSQLite { get; set; }
        public string? PathToLogFile { get; set; }

        public void Init()
        {
            string pathToAppFiles = Path.Combine(GetHomeDirectory(), ".vax_flow");

            if (!Directory.Exists(pathToAppFiles))
            {
                Directory.CreateDirectory(pathToAppFiles);
            }

            string pathToDatabase = Path.Combine(pathToAppFiles, "vax_flow.db");
            DataSourceSQLite = $"Data Source ={pathToDatabase}";
            PathToLogFile = Path.Combine(pathToAppFiles, "vax_flow.log");
        }
        public static string GetHomeDirectory()
        {
            string? home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

            if (string.IsNullOrEmpty(home))
            {
                if (OperatingSystem.IsWindows())
                {
                    home = Environment.GetEnvironmentVariable("USERPROFILE");
                }
                else
                {
                    home = Environment.GetEnvironmentVariable("HOME");
                }
            }

            return home ?? throw new InvalidOperationException("Home directory not found");
        }
    }
}
