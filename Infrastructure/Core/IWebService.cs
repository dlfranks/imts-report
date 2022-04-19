using System.Threading.Tasks;
using Application.Core;

namespace Infrastructure.Core
{
    public interface IWebService<T>
    {
        Task<Result<T>> OnGetData(string url, bool isSample = false);
    }
}