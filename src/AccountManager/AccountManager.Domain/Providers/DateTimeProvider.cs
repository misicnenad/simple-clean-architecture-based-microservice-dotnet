using System;

namespace AccountManager.Domain.Providers
{
    public class DateTimeProvider
    {
        public virtual DateTime UtcNow => DateTime.UtcNow;
    }
}
