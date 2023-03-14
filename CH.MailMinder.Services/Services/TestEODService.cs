using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CH.MailMinder.Services
{
    /// <summary>
    /// Implementation of <see cref="IEODService"/> for dev purposes.
    /// </summary>
    public class TestEODService : IEODService
    {
        /// <summary>
        /// Always returns true
        /// </summary>
        /// <returns></returns>
        public bool IsEndOfDay() => true;
    }
}
