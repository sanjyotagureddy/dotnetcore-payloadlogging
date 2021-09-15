using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PayloadLogging.Common.Models.PayloadLogging;
using PayloadLogging.Common.RestClients.Interface;
using RestSharp;

namespace PayloadLogging.Common.UnitTest.Helpers
{
  public class FakeClass
  {
    private readonly IRestService<PayloadModel> _restService;

    public FakeClass(IRestService<PayloadModel> restService)
    {
      _restService = restService ?? throw new ArgumentNullException(nameof(restService));
    }
    public async Task<IRestResponse> Get(string url,
      string methodName,
      IDictionary<string, string> headers = null,
      IDictionary<string, string> queryParameters = null)
      => await _restService.Get(url, methodName, headers, queryParameters);

    public async Task<IRestResponse> GetById(string url,
      string methodName,
      string id,
      IDictionary<string, string> headers = null,
      IDictionary<string, string> queryParameters = null)
      => await _restService.Get(url, methodName, id, headers, queryParameters);

    public async Task<IRestResponse> Post(string url,
      string methodName,
      PayloadModel body,
      IDictionary<string, string> headers = null,
      IDictionary<string, string> queryParameters = null)
      => await _restService.Post(url, methodName, body, headers, queryParameters);

    public async Task<IRestResponse> Put(string url,
      string methodName,
      PayloadModel body,
      IDictionary<string, string> headers = null,
      IDictionary<string, string> queryParameters = null)
      => await _restService.Put(url, methodName, body, headers, queryParameters);

    public async Task<bool> Delete(string url,
      string methodName,
      int id,
      IDictionary<string, string> headers = null,
      IDictionary<string, string> queryParameters = null)
      => await _restService.Delete(url, methodName, id, headers, queryParameters);
  }
}
