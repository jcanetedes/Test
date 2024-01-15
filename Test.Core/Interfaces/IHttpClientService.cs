namespace Test2.Core.Interfaces;
public interface IHttpClientService
{

    Task<T> DeleteAsync<T>(string url, string logPrefix = "");
    Task<T> GetAsync<T>(string url, string logPrefix = "");
    Task<T> PostAsync<T>(string url, object postData, string logPrefix = "");
    Task<T> PutAsync<T>(string url, object postData, string logPrefix = "");
}
