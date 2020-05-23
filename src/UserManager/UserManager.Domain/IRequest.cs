namespace UserManager.Domain
{
    public interface IRequest : IRequest<Void>
    {
    }

    public interface IRequest<out TResponse>
    {
    }

    public class Void { }
}
