using CH.MailMinder.Services;
using System.Reflection;

namespace CH.MailMinder.WorkerService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IHost host = Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.SetBasePath(Directory.GetCurrentDirectory());
                    config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                    config.AddEnvironmentVariables();
                    config.AddUserSecrets(typeof(Program).Assembly);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.Configure<MailMinderSettings>(hostContext.Configuration.GetSection("MailMinder"));

                    services.AddLogging();
                    services.AddSingleton<GraphService>();

                    var settings = GetSettings(hostContext.Configuration);
                    if (settings.DevModeEnabled)
                    {
                        services.AddSingleton<IEODService, TestEODService>();
                    }
                    else
                    {
                        services.AddSingleton<IEODService, EODService>();
                    }
                    services.AddHostedService<Worker>();
                })
                .Build();

            host.Run();
        }

        private static MailMinderSettings GetSettings(IConfiguration configuration)
        {
            MailMinderSettings settings = new MailMinderSettings();
            configuration.GetSection("MailMinder").Bind(settings);
            return settings;
        }
    }
}