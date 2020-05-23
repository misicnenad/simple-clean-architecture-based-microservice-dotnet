using System.Threading.Tasks;

namespace UserManager.Domain
{
    public delegate Task RequestHandlerDelegate();
    public delegate Task<TResponse> RequestHandlerDelegate<TResponse>();
}
