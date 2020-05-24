using System;

namespace UserManager.Domain.Interfaces
{
    public interface IDateTimeProvider
    {
        public DateTime UtcNow { get; }
    }
}
