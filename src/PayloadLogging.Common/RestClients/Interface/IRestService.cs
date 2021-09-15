using System.Collections.Generic;
using System.Threading.Tasks;
using RestSharp;

namespace PayloadLogging.Common.RestClients.Interface
{

  public interface IRestService<T>
  {
    Task<IRestResponse> Get(string url, string methodName, string id, IDictionary<string, string> headers = null, IDictionary<string, string> queryParameters = null);

    Task<IRestResponse> Get(string url, string methodName, IDictionary<string, string> headers = null,
      IDictionary<string, string> queryParameters = null);

    Task<IRestResponse> Post(string url, string methodName, T body, IDictionary<string, string> headers = null, IDictionary<string, string> queryParameters = null);
    Task<IRestResponse> Put(string url, string methodName, T body, IDictionary<string, string> headers = null, IDictionary<string, string> queryParameters = null);
    Task<bool> Delete(string url, string methodName, int id, IDictionary<string, string> headers = null, IDictionary<string, string> queryParameters = null);
  }
}
