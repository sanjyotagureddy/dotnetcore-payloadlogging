using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PayloadLogging.Common.Settings;
using RestSharp;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace PayloadLogging.Common.IntegrationTest.MiddlewareTests
{
  public class PayloadLoggingMiddlewareTests
  {
    [Fact]
    public async Task MiddlewareTest_ReturnsNotFoundForRequest()
    {
      using var host = await CreateNewHostBuilder().ConfigureAwait(false);

      var response = await host.GetTestClient().GetAsync("https://fakeapi.com/").ConfigureAwait(false);

      response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task MiddlewareTest_ReturnsOkForRequest()
    {
      using var host = await CreateNewHostBuilder().ConfigureAwait(false);

      var server = host.GetTestServer();
      server.BaseAddress = new Uri("https://example.com/A/Path/");

      var context = await server.SendAsync(c =>
      {
        c.Request.Method = HttpMethods.Post;
        c.Request.Path = "/and/file.txt";
        c.Request.QueryString = new QueryString("?and=query");
      }).ConfigureAwait(false);

      context.Request.Method.Should().Be(Method.POST.ToString());
      context.Request.Scheme.Should().Be("https");
      context.Request.Body.Should().NotBeNull();
      context.Request.Headers.Should().NotBeNull();
      context.Response.Headers.Should().NotBeNull();
      context.Response.Body.Should().NotBeNull();
      context.Request.Host.Value.Should().Be("example.com");
      context.Request.PathBase.Value.Should().Be("/A/Path");
      context.Request.Path.Value.Should().Be("/and/file.txt");
      context.Request.QueryString.Value.Should().Be("?and=query");
      context.Response.StatusCode.Should().Be(StatusCodes.Status404NotFound);
    }

    private async Task<IHost> CreateNewHostBuilder() =>
      await new HostBuilder()
        .ConfigureWebHost(webBuilder =>
        {
          webBuilder
            .UseTestServer()
            .ConfigureServices(services =>
            {
              services.TryAddRestServiceCollection(null);
              var settings = new ApiSettings();
              ApiSettings.PayloadLoggingHost = "https://fakeloggingapi.com";
              ApiSettings.IgnorePayloadUrls = "/health/live,/health/ready";

              services.AddSingleton(settings);
            })
            .Configure(app =>
            {
              app.UsePayloadLogging();
            });
        })
        .StartAsync()
        .ConfigureAwait(false);
  }
}