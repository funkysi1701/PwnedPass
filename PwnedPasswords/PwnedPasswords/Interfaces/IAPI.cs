using System.Net.Http;
using System.Threading.Tasks;

namespace PwnedPasswords
{
    public interface IAPI
    {
        Task<bool> GetAPI(string url);

        Task<string> GetHIBP(string url);

        Task<HttpResponseMessage> GetAsyncAPI(string url);
    }
}
