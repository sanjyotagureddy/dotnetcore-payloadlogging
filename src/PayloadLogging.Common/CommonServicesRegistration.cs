using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using PayloadLogging.Common.Middlewares;
using PayloadLogging.Common.RestClients;
using PayloadLogging.Common.RestClients.Interface;
using RestSharp;

namespace PayloadLogging.Common
{
  public static class CommonServicesRegistration
  {
    #region IServiceCollections

    public static IServiceCollection TryAddRestServiceCollection(this IServiceCollection services, IConfiguration configuration)
    {
      services.TryAddTransient<IRestClient, RestClient>();
      services.TryAddSingleton(typeof(IRestService<>), typeof(RestService<>));
      return services;
    }

    #endregion IServiceCollections

    #region IApplicationBuilders

    public static IApplicationBuilder UsePayloadLogging(this IApplicationBuilder builder)
    {
      return builder.UseMiddleware<PayloadLoggingMiddleware>();
    }

    #endregion IApplicationBuilders
  }
}