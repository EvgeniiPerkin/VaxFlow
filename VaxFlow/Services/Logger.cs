using Serilog;
using System;

namespace VaxFlow.Services
{
    public class Logger : IMyLogger
    {
        public Logger(IAppConfiguration configuration)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.File(
                    path: configuration.PathToLogFile ?? "C:\\Users\\Public\\vax_flow.log",
                    rollingInterval: RollingInterval.Month,
                    retainedFileCountLimit: 10,
                    fileSizeLimitBytes: 20_971_520,
                    rollOnFileSizeLimit: true,
                    shared: true)
                .CreateLogger();
        }

        public void Debug(string message)
        {
            Log.Logger.Debug(message);
        }

        public void Error(Exception e, string message)
        {
            Log.Logger.Error(e, message);
        }

        public void Fatal(Exception e, string message)
        {
            Log.Logger.Fatal(e, message);
        }

        public void Info(string message)
        {
            Log.Logger.Information(message);
        }

        public void Warn(string message)
        {
            Log.Logger.Warning(message);
        }
    }
}
