using Azure.Messaging;
using CH.MailMinder.Services;
using Microsoft.Graph.Models;

namespace CH.MailMinder.WorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly GraphService _graphService;
        private readonly IEODService _eodService;
        private static DateOnly _dateLastNotified;

        public Worker(ILogger<Worker> logger, GraphService graphService, IEODService eodService)
        {
            _logger = logger;
            _graphService = graphService;
            _eodService = eodService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _graphService.Login();
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(10000, stoppingToken);
                await CheckAndSend();
            }
        }

        private async Task CheckAndSend()
        {
            _logger.LogInformation($"Checking for end of day: {DateTimeOffset.Now}");

            var dateToday = DateOnly.FromDateTime(DateTime.Now);
            if (_dateLastNotified != dateToday && _eodService.IsEndOfDay())
            {
                _logger.LogInformation($"EOD has arrived!");
                var unreadMessages = await _graphService.GetUnreadEmailsFromTodayAsync();

                if (unreadMessages.Any())
                {
                    string messageContent = BuildMessageContent(unreadMessages);
                    await _graphService.SendTeamsMessageToSelf(messageContent);

                    _dateLastNotified = dateToday;
                    _logger.LogInformation($"Notified self with {unreadMessages.Count} unread messages");
                }
            }
        }

        private string BuildMessageContent(List<Message> emails)
        {
            return string.Join(Environment.NewLine, 
                emails.Select((m, i) => $"Unread Email {i + 1}: Subject '{m.Subject}', From '{m.From.EmailAddress.Address}' (<a href='{m.WebLink}'>View<a/>)")
                );
        }
    }
}