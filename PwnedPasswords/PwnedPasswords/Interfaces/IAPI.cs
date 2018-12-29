using System.Net.Http;

namespace PwnedPasswords
{
    public interface IAPI
    {
        bool GetAPI(string url);
        string GetHIBP(string url);
        HttpResponseMessage GetAsyncAPI(string url);
    }
}
