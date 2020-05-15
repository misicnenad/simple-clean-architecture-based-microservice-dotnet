using System;

namespace Shared.Messages
{
    public interface IMessage
    {
        public Guid CorrelationId { get; }
    }
}
