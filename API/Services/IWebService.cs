using System.Threading.Tasks;
using API.Models.Core;

namespace API.Services
{
    public interface IWebService<T>
    {
        Task<Result<T>> OnGetData(string url);
    }
}