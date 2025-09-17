using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using VaxFlow.Data;
using VaxFlow.Data.Repositories;
using VaxFlow.Data.Services;
using VaxFlow.Services;
using VaxFlow.ViewModels;
using VaxFlow.Views;

namespace VaxFlow
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            var locator = new ViewLocator();
            DataTemplates.Add(locator);

            var services = new ServiceCollection();
            ConfigureServices(services);
            var provider = services.BuildServiceProvider();

            Ioc.Default.ConfigureServices(provider);

            var appConf = Ioc.Default.GetRequiredService<IAppConfiguration>();
            appConf.Init();

            var context = Ioc.Default.GetRequiredService<DbContext>();
            int resultInit = Task.Run(() => context.SetupAsync()).Result;

            var vm = Ioc.Default.GetRequiredService<MainWindowViewModel>();

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                if (resultInit == -1) desktop.Shutdown(0);
                desktop.MainWindow = new MainWindow { DataContext = vm };
            }
            else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
            {
                singleViewPlatform.MainView = new MainView { DataContext = vm };
            }

            base.OnFrameworkInitializationCompleted();
        }

        private void ConfigureServices(ServiceCollection services)
        {
            services.AddSingleton<IAppConfiguration, AppConfiguration>();
            services.AddSingleton<IMyLogger, Logger>();
            services.AddSingleton<DbContext>();
            services.AddSingleton<DoctorRepository>();
            services.AddSingleton<DoctorsAppointmentsRepository>();
            services.AddSingleton<JobCategoryRepository>();
            services.AddSingleton<PartRepository>();
            services.AddSingleton<PatternRepository>();
            services.AddSingleton<VaccineVersionRepository>();
            services.AddSingleton<VaccineRepository>();
            services.AddSingleton<PartyRepository>();
            services.AddSingleton<PatientRepository>();

            services.AddSingleton<DoctorService>();

            services.AddSingleton<MainWindow>();
            services.AddTransient<VaccinationJournalView>();
            services.AddSingleton<DoctorView>();
            services.AddSingleton<VaccineVersionsView>();
            services.AddSingleton<VaccineView>();
            services.AddSingleton<PatryView>();
            services.AddSingleton<JobCategoryView>();
            services.AddTransient<SettingsView>();
            services.AddTransient<HelpView>();
            
            services.AddSingleton<MainWindowViewModel>();
            services.AddSingleton<VaccinationJournalViewModel>();
            services.AddSingleton<DoctorViewModel>();
            services.AddSingleton<VaccineVersionsViewModel>();
            services.AddSingleton<VaccineViewModel>();
            services.AddSingleton<PatryViewModel>();
            services.AddSingleton<JobCategoryViewModel>();
            services.AddTransient<SettingsViewModel>();
            services.AddTransient<HelpViewModel>();
        }
    }
}