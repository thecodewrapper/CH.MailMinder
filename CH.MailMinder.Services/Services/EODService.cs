using Microsoft.Extensions.Options;

namespace CH.MailMinder.Services
{
    /// <summary>
    /// Implementation of <see cref="IEODService"/>. Retrieves configuration from <see cref="MailMinderSettings"/> and checks for EOD
    /// </summary>
    public class EODService : IEODService
    {
        private readonly MailMinderSettings _mailMinderSettings;
        public EODService(IOptions<MailMinderSettings> mailMinderSettings)
        {
            _mailMinderSettings = mailMinderSettings.Value;
        }

        public bool IsEndOfDay()
        {
            TimeSpan EOD = new TimeSpan(_mailMinderSettings.EODHour24, 0, 0);
            DateTime endOfDayDateTime = DateTime.Now.Date.Add(EOD);
            return DateTime.Now >= endOfDayDateTime;
        }
    }
}
