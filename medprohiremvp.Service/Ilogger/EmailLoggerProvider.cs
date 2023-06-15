using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace medprohiremvp.Service.Ilogger
{
    public class EmailLoggerProvider : IEmailLoggerProvider
    {
        private readonly IEmailLogger _emailLogger;

        public EmailLoggerProvider(IEmailLogger emailLogger)
        {
            _emailLogger = emailLogger;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return _emailLogger;
        }

        public void Dispose()
        {
        }
    }
}
