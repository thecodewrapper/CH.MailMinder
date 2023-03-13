using CH.MailMinder.Services;

namespace CH.MailMinder.WorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly GraphService _graphService;
        private const string CLIENT_ID = "d46a079f-087e-4e01-9c73-32f32621fbb7";

        public Worker(ILogger<Worker> logger, GraphService graphService)
        {
            _logger = logger;
            _graphService = graphService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _graphService.Login("d46a079f-087e-4e01-9c73-32f32621fbb7");
            await _graphService.GetUnreadEmailsFromTodayAsync();
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}