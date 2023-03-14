using Azure.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Graph;
using Microsoft.Graph.Models;

namespace CH.MailMinder.Services
{
    public class GraphService
    {
        private GraphServiceClient _graphClient;
        private readonly ILogger<GraphService> _logger;
        private readonly MailMinderSettings _mailMinderSettings;
        private readonly string[] _graphScopes = new[] { "User.Read", "Mail.Read", "Mail.Send", "ChatMessage.Send", "Chat.ReadWrite" };
        private const string SELF_CHAT_ID = "48:notes";

        public GraphService(ILogger<GraphService> logger, IOptions<MailMinderSettings> mailMinderSettings)
        {
            _logger = logger;
            _mailMinderSettings = mailMinderSettings.Value;
        }

        public async Task Login()
        {
            _graphClient = await GetGraphClientInteractiveBrowserAsync();
        }

        public async Task SendTeamsMessageToSelf(string content)
        {
            var chatMessage = new ChatMessage
            {
                Body = new ItemBody
                {
                    ContentType = BodyType.Html,
                    Content = content
                }
            };
            await _graphClient.Me.Chats[SELF_CHAT_ID].Messages.PostAsync(chatMessage);
        }

        public async Task<List<Message>> GetUnreadEmailsFromTodayAsync()
        {
            _logger.LogDebug($"Retreiving unread message from today");
            // Retrieve unread emails
            DateTimeOffset today = DateTimeOffset.UtcNow.Date;
            var messages = await _graphClient.Me.Messages
                            .GetAsync(requestConfig =>
                            {
                                requestConfig.QueryParameters.Filter = $"isRead ne true and receivedDateTime ge {today:yyyy-MM-dd}";
                            });

            _logger.LogInformation($"You have {messages.Value.Count} unread emails.");
            return messages.Value;
        }

        private async Task<GraphServiceClient> GetGraphClientInteractiveBrowserAsync()
        {
            var interactiveBrowserCredentialOptions = new InteractiveBrowserCredentialOptions
            {
                ClientId = _mailMinderSettings.Graph.ClientId,
                TenantId = _mailMinderSettings.Graph.TenantId
            };
            var tokenCredential = new InteractiveBrowserCredential(interactiveBrowserCredentialOptions);

            var graphClient = new GraphServiceClient(tokenCredential, _graphScopes);
            _ = await graphClient.Me.GetAsync(); //trigger login
            return graphClient;
        }
    }
}