using System.Threading.Tasks;

namespace UserManager.Domain
{
    public delegate Task<TResponse> RequestHandlerDelegate<TResponse>();
}
