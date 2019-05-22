using System.Net.Http;
using System.Threading.Tasks;

namespace PwnedPasswords
{
    public interface IAPI
    {
        bool GetAPI(string url);

        Task<string> GetHIBP(string url);

        HttpResponseMessage GetAsyncAPI(string url);
    }
}
