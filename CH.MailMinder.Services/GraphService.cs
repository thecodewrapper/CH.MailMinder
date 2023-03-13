using Azure.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;

namespace CH.MailMinder.Services
{
    public class GraphService
    {
        private GraphServiceClient _graphClient;
        private readonly ILogger<GraphService> _logger;

        public GraphService(ILogger<GraphService> logger)
        {
            _logger = logger;
        }

        public async Task Login(string clientId)
        {
            _graphClient = GetGraphClientInteractiveBrowser(clientId);
        }

        public async Task GetUnreadEmailsFromTodayAsync()
        {
            // Retrieve unread emails
            DateTimeOffset today = DateTimeOffset.UtcNow.Date;
            var messages = await _graphClient.Me.Messages
                            .GetAsync(requestConfig =>
                            {
                                requestConfig.QueryParameters.Filter = $"isRead eq false and receivedDateTime ge {today:yyyy-MM-dd}";
                                requestConfig.QueryParameters.Orderby = new string[] { "receivedDateTime" };
                            });

            _logger.LogInformation($"You have {messages.Value.Count} unread emails. Here are the senders:\n");
        }

        private GraphServiceClient GetGraphClientInteractiveBrowser(string clientId)
        {
            var scopes = new[] { "User.Read", "Mail.Read", "Mail.Send" };
            var interactiveBrowserCredentialOptions = new InteractiveBrowserCredentialOptions
            {
                ClientId = clientId
            };
            var tokenCredential = new InteractiveBrowserCredential(interactiveBrowserCredentialOptions);

            var graphClient = new GraphServiceClient(tokenCredential, scopes);

            return graphClient;
            //var me = await graphClient.Me.GetAsync();
        }
    }
}