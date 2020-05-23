using System;

namespace UserManager.Domain.Providers
{
    public class DateTimeProvider
    {
        public virtual DateTime UtcNow => DateTime.UtcNow;
    }
}
