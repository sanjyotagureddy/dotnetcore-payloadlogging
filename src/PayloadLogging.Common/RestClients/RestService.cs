using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PayloadLogging.Common.RestClients.Interface;
using RestSharp;

namespace PayloadLogging.Common.RestClients
{
  public class RestService<T> : IRestService<T>
  {
    private readonly IRestClient _client;

    public RestService(IRestClient client)
    {
      _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    /// <summary>
    /// Makes GET request
    /// </summary>
    /// <param name="url">Base Url</param>
    /// <param name="methodName">Method name to be called</param>
    /// <param name="headers">Dictionary of header strings</param>
    /// <param name="queryParameters">Dictionary of Query strings</param>
    /// <returns>Response of the request</returns>
    public async Task<IRestResponse> Get(string url, string methodName, IDictionary<string, string> headers = null, IDictionary<string, string> queryParameters = null)
    {
      var request = BuildBaseRequest(Method.GET, url, methodName, headers, queryParameters);
      return await _client.ExecuteGetAsync(request);
    }

    /// <summary>
    /// Make GET request for given id
    /// </summary>
    /// <param name="url">Base Url</param>
    /// <param name="methodName">Method name to be called</param>
    /// <param name="id">Id to Get</param>
    /// <param name="headers">Dictionary of header strings</param>
    /// <param name="queryParameters">Dictionary of Query strings</param>
    /// <returns>Response of the request</returns>
    public async Task<IRestResponse> Get(string url, string methodName, string id, IDictionary<string, string> headers = null, IDictionary<string, string> queryParameters = null)
    {
      var request = BuildBaseRequest(Method.GET, url, $"{methodName}/{id}", headers, queryParameters);
      return await _client.ExecuteGetAsync(request);
    }

    /// <summary>
    /// Makes POST request
    /// </summary>
    /// <param name="url">Base Url</param>
    /// <param name="methodName">Method name to be called</param>
    /// <param name="body">Object of type T</param>
    /// <param name="headers">Dictionary of header strings</param>
    /// <param name="queryParameters">Dictionary of Query strings</param>
    /// <returns>Response of the request</returns>
    public async Task<IRestResponse> Post(string url, string methodName, T body, IDictionary<string, string> headers = null, IDictionary<string, string> queryParameters = null)
    {
      var request = BuildBaseRequest(Method.POST, url, methodName, headers, queryParameters);
      request.AddJsonBody(body);
      return await _client.ExecutePostAsync(request);
    }

    /// <summary>
    /// Makes PUT request
    /// </summary>
    /// <param name="url">Base Url</param>
    /// <param name="methodName">Method name to be called</param>
    /// <param name="body">Object of type T</param>
    /// <param name="headers">Dictionary of header strings</param>
    /// <param name="queryParameters">Dictionary of Query strings</param>
    /// <returns>Response of the request</returns>
    public async Task<IRestResponse> Put(string url, string methodName, T body, IDictionary<string, string> headers = null, IDictionary<string, string> queryParameters = null)
    {
      var request = BuildBaseRequest(Method.PUT, url, methodName, headers, queryParameters);
      request.AddJsonBody(body);
      return await _client.ExecuteAsync(request);
    }

    /// <summary>
    /// Make DELETE request
    /// </summary>
    /// <param name="url">Base Url</param>
    /// <param name="methodName">Method name to be called</param>
    /// <param name="id">id of the object to be deleted</param>
    /// <param name="headers">Dictionary of header strings</param>
    /// <param name="queryParameters">Dictionary of Query strings</param>
    /// <returns>true or false </returns>
    public async Task<bool> Delete(string url, string methodName, int id, IDictionary<string, string> headers = null, IDictionary<string, string> queryParameters = null)
    {
      var request = BuildBaseRequest(Method.DELETE, url, methodName, headers, queryParameters);
      var response = await _client.ExecuteAsync(request);
      return response.IsSuccessful;
    }

    #region Private Methods

    private static RestRequest BuildBaseRequest(Method method, string url, string methodName, IDictionary<string, string> headers,
      IDictionary<string, string> queryParameters)
    {
      if (!string.IsNullOrWhiteSpace(url))
      {
        if (!url.Last().Equals('/'))
        {
          url += '/';
        }
      }

      var request = new RestRequest($"{url}{methodName}", method, DataFormat.Json);

      if (headers != null)
      {
        request.AddHeaders(headers);
      }


      if (queryParameters != null)
      {
        foreach (var (key, value) in queryParameters)
        {
          request.AddQueryParameter(key, value);
        }
      }

      return request;
    }

    #endregion
  }
}
