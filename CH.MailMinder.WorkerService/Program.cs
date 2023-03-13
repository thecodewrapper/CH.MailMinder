using CH.MailMinder.Services;

namespace CH.MailMinder.WorkerService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IHost host = Host.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    services.AddLogging();
                    services.AddHostedService<Worker>();
                    services.AddSingleton<GraphService>();
                })
                .Build();

            host.Run();
        }
    }
}