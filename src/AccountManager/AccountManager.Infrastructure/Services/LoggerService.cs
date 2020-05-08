using System;
using Microsoft.Extensions.Logging;

namespace AccountManager.Infrastructure.Services
{
    public class LoggerService<T> : ILoggerService<T>
    {
        private readonly ILogger _logger;

        public LoggerService(ILogger<T> logger)
        {
            _logger = logger;
        }

        public void LogInfo(Guid correlationId, string message)
        {
            _logger.LogInformation($"{nameof(correlationId)}={correlationId} - {message}");
        }
    }

    public interface ILoggerService<T>
    {
        public void LogInfo(Guid correlationId, string message);
    }
}
