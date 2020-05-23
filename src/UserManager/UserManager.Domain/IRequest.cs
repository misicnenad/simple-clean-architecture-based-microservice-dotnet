namespace UserManager.Domain
{
    public interface IRequest
    {
    }

    public interface IRequest<out TResponse> : IRequest
    {
    }
}
