using CH.MailMinder.Services.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CH.MailMinder.Services
{
    public class MailMinderSettings
    {
        public int EODHour24 { get; set; }
        public MailMinderGraphSettings Graph { get; set; } = new MailMinderGraphSettings();
        public bool DevModeEnabled { get; set; }
    }
}
