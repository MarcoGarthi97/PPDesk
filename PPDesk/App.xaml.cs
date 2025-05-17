using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Shapes;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using PPDesk.Repository.Collection;
using PPDesk.Service.Collection;
using System.Threading.Tasks;
using Windows.Storage;
using PPDesk.Repository.Factory;
using PPDesk.Service.Services.PP;
using AutoMapper;
using PPDesk.Service.Services.Window;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using PPDesk.Abstraction.DTO.Service.Eventbrite;
using PPDesk.Service.Storages.Eventbride;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using Z.Dapper.Plus;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace PPDesk
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        private IHost _host;

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            _host = Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration((ctx, cfg) =>
                {
                    cfg.SetBasePath(AppContext.BaseDirectory)
              .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                })
                .ConfigureServices((context, services) =>
                {
                    services.AddSingleton<IConfiguration>(context.Configuration);
                    ConfigureServices(services, context.Configuration);
                })
                .Build();


            InizializeDatabase().Wait();

            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when the application is launched.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            var mainWindowService = _host.Services.GetRequiredService<ISrvMainWindowService>();
            m_window = new MainWindow(mainWindowService);
            m_window.Activate();
        }

        private void ConfigureServices(IServiceCollection services, IConfiguration config)
        {
            services.AddAutoMapper(typeof(App).Assembly,
                          typeof(SrvVersionService).Assembly);

            services.AddSharedLibraryServices();
            services.AddSharedLibraryRepositories();

            string dbPath = System.IO.Path.Combine(ApplicationData.Current.LocalFolder.Path, "app.db");
            var connectionString = $"Data Source={dbPath}";
            services.AddSingleton<IDatabaseConnectionFactory>(provider =>
                new MdlSqliteConnectionFactory(connectionString));

            LoadConfigurations(config);
        }

        private async Task InizializeDatabase()
        {
            //C:\Users\marco\AppData\Local\Packages\dfcae022-bc71-4537-b546-539103578783_2jm902zmczqjy\LocalState
            var path = ApplicationData.Current.LocalFolder;
            await ApplicationData.Current.LocalFolder.CreateFileAsync("app.db", CreationCollisionOption.OpenIfExists);            

            var databaseService = _host.Services.GetRequiredService<ISrvDatabaseService>();
            await databaseService.CreateTablesAsync();

            ConfigurationDatabase();
        }

        private void ConfigurationDatabase()
        {
            RepositoryCollectionExtension.ConfigurationDatabase();
        }

        private void LoadConfigurations(IConfiguration config)
        {
            var apikey = config.GetSection("App:EventbriteApiKey").Get<SrvEApiKey>();
            SrvEApiKeyStorage.SetpiKeyStorage(apikey);
        }

        private Window? m_window;
    }
}
