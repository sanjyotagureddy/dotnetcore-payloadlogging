using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Moq.RestSharp.Helpers;
using PayloadLogging.Common.Models.PayloadLogging;
using PayloadLogging.Common.RestClients;
using PayloadLogging.Common.RestClients.Interface;
using PayloadLogging.Common.UnitTest.Helpers;
using RestSharp;
using Xunit;

namespace PayloadLogging.Common.UnitTest.RestClients
{
  public class RestClientTests
  {
    private readonly Mock<IRestClient> _mockRestClient;
    private readonly FakeClass _fakeClass;

    public RestClientTests()
    {
      _mockRestClient = new Mock<IRestClient>();
      IRestService<PayloadModel> rest = new RestService<PayloadModel>(_mockRestClient.Object);
      _fakeClass = new FakeClass(rest);
    }

    #region POST
    [Fact]
    public async Task POST_Returns400Response()
    {
      var mockResponse = _mockRestClient
        .MockApiResponse<PayloadModel>()
        .WithStatusCode(HttpStatusCode.BadRequest)
        .Returns(RestClientHelpers.GetResponsePayload())
        .MockExecute();

      _mockRestClient.Setup(x => x.ExecutePostAsync(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(mockResponse);

      var result = await _fakeClass.Post("http://fakeapi.com", "payload", RestClientHelpers.GetRequestPayload(), RestClientHelpers.GetRandomDictionary());

      result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      result.IsSuccessful.Should().BeFalse();
      result.Content.Should().Be("{\"Source\":\"http://fakeapi.com/payload\",\"Payload\":\"{\\\"Body\\\":\\\"{\\\\\\\"Node1\\\\\\\":{\\\\\\\"Node2\\\\\\\":\\\\\\\"Value1\\\\\\\"}}\\\",\\\"Headers\\\":{\\\"string1\\\":\\\"value1\\\",\\\"string2\\\":\\\"value2\\\"},\\\"Query\\\":\\\"?api-version=1.0\\\"}\",\"Type\":\"Request\",\"CorrelationId\":\"202109111157004007-1921\",\"ResponseCode\":200}");
    }


    [Fact]
    public async Task POST_Returns201Response()
    {
      var mockResponse = _mockRestClient
        .MockApiResponse<PayloadModel>()
        .WithStatusCode(HttpStatusCode.Created)
        .Returns(RestClientHelpers.GetResponsePayload())
        .MockExecutePostAsync();

      _mockRestClient.Setup(x => x.ExecutePostAsync(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(mockResponse);

      var result = await _fakeClass.Post("http://fakeapi.com/", "payload", RestClientHelpers.GetRequestPayload(), null, RestClientHelpers.GetRandomDictionary());

      result.StatusCode.Should().Be(HttpStatusCode.Created);
      result.IsSuccessful.Should().BeTrue();
      result.Content.Should().Be("{\"Source\":\"http://fakeapi.com/payload\",\"Payload\":\"{\\\"Body\\\":\\\"{\\\\\\\"Node1\\\\\\\":{\\\\\\\"Node2\\\\\\\":\\\\\\\"Value1\\\\\\\"}}\\\",\\\"Headers\\\":{\\\"string1\\\":\\\"value1\\\",\\\"string2\\\":\\\"value2\\\"},\\\"Query\\\":\\\"?api-version=1.0\\\"}\",\"Type\":\"Request\",\"CorrelationId\":\"202109111157004007-1921\",\"ResponseCode\":200}");
    }

    #endregion


    #region PUT
    [Fact]
    public async Task PUT_Returns400Response()
    {
      var mockResponse = _mockRestClient
        .MockApiResponse<PayloadModel>()
        .WithStatusCode(HttpStatusCode.BadRequest)
        .Returns(RestClientHelpers.GetResponsePayload())
        .MockExecute();

      _mockRestClient.Setup(x => x.ExecuteAsync(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(mockResponse);

      var result = await _fakeClass.Put("http://fakeapi.com", "payload", RestClientHelpers.GetRequestPayload(), RestClientHelpers.GetRandomDictionary());

      result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      result.IsSuccessful.Should().BeFalse();
      result.Content.Should().Be("{\"Source\":\"http://fakeapi.com/payload\",\"Payload\":\"{\\\"Body\\\":\\\"{\\\\\\\"Node1\\\\\\\":{\\\\\\\"Node2\\\\\\\":\\\\\\\"Value1\\\\\\\"}}\\\",\\\"Headers\\\":{\\\"string1\\\":\\\"value1\\\",\\\"string2\\\":\\\"value2\\\"},\\\"Query\\\":\\\"?api-version=1.0\\\"}\",\"Type\":\"Request\",\"CorrelationId\":\"202109111157004007-1921\",\"ResponseCode\":200}");
    }


    [Fact]
    public async Task PUT_Returns202Response()
    {
      var mockResponse = _mockRestClient
        .MockApiResponse<PayloadModel>()
        .WithStatusCode(HttpStatusCode.Accepted)
        .Returns(RestClientHelpers.GetResponsePayload())
        .MockExecutePostAsync();

      _mockRestClient.Setup(x => x.ExecuteAsync(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(mockResponse);

      var result = await _fakeClass.Put("http://fakeapi.com/", "payload", RestClientHelpers.GetRequestPayload(), null, RestClientHelpers.GetRandomDictionary());

      result.StatusCode.Should().Be(HttpStatusCode.Accepted);
      result.IsSuccessful.Should().BeTrue();
      result.Content.Should().Be("{\"Source\":\"http://fakeapi.com/payload\",\"Payload\":\"{\\\"Body\\\":\\\"{\\\\\\\"Node1\\\\\\\":{\\\\\\\"Node2\\\\\\\":\\\\\\\"Value1\\\\\\\"}}\\\",\\\"Headers\\\":{\\\"string1\\\":\\\"value1\\\",\\\"string2\\\":\\\"value2\\\"},\\\"Query\\\":\\\"?api-version=1.0\\\"}\",\"Type\":\"Request\",\"CorrelationId\":\"202109111157004007-1921\",\"ResponseCode\":200}");
    }

    #endregion

    #region Delete
    [Fact]
    public async Task Delete_Returns400Response()
    {
      var mockResponse = _mockRestClient
        .MockApiResponse<PayloadModel>()
        .WithStatusCode(HttpStatusCode.BadRequest)
        .Returns(RestClientHelpers.GetResponsePayload())
        .MockExecute();
      _mockRestClient.Setup(x => x.ExecuteAsync(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(mockResponse);

      var result = await _fakeClass.Delete("http://fakeapi.com", "payload", 2, RestClientHelpers.GetRandomDictionary());

      result.Should().BeFalse();
    }


    [Fact]
    public async Task Delete_Returns202Response()
    {
      var mockResponse = _mockRestClient
        .MockApiResponse<PayloadModel>()
        .WithStatusCode(HttpStatusCode.Accepted)
        .Returns(RestClientHelpers.GetResponsePayload())
        .MockExecutePostAsync();

      _mockRestClient.Setup(x => x.ExecuteAsync(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(mockResponse);

      var result = await _fakeClass.Delete("http://fakeapi.com/", "payload", 1, null, RestClientHelpers.GetRandomDictionary());
      result.Should().BeTrue();
    }

    #endregion

    #region GET
    [Fact]
    public async Task GET_Returns400Response()
    {
      var mockResponse = _mockRestClient
        .MockApiResponse<PayloadModel>()
        .WithStatusCode(HttpStatusCode.BadRequest)
        .Returns(RestClientHelpers.GetResponsePayload())
        .MockExecute();
      _mockRestClient.Setup(x => x.ExecuteGetAsync(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(mockResponse);

      var result = await _fakeClass.Get("http://fakeapi.com/", "payload", null, RestClientHelpers.GetRandomDictionary());

      result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      result.IsSuccessful.Should().BeFalse();
      result.Content.Should().Be("{\"Source\":\"http://fakeapi.com/payload\",\"Payload\":\"{\\\"Body\\\":\\\"{\\\\\\\"Node1\\\\\\\":{\\\\\\\"Node2\\\\\\\":\\\\\\\"Value1\\\\\\\"}}\\\",\\\"Headers\\\":{\\\"string1\\\":\\\"value1\\\",\\\"string2\\\":\\\"value2\\\"},\\\"Query\\\":\\\"?api-version=1.0\\\"}\",\"Type\":\"Request\",\"CorrelationId\":\"202109111157004007-1921\",\"ResponseCode\":200}");
    }

    [Fact]
    public async Task GET_ById_Returns400Response()
    {
      var mockResponse = _mockRestClient
        .MockApiResponse<PayloadModel>()
        .WithStatusCode(HttpStatusCode.BadRequest)
        .Returns(RestClientHelpers.GetResponsePayload())
        .MockExecute();
      _mockRestClient.Setup(x => x.ExecuteGetAsync(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(mockResponse);

      var result = await _fakeClass.GetById("http://fakeapi.com/", "payload","13654", null, RestClientHelpers.GetRandomDictionary());

      result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      result.IsSuccessful.Should().BeFalse();
      result.Content.Should().Be("{\"Source\":\"http://fakeapi.com/payload\",\"Payload\":\"{\\\"Body\\\":\\\"{\\\\\\\"Node1\\\\\\\":{\\\\\\\"Node2\\\\\\\":\\\\\\\"Value1\\\\\\\"}}\\\",\\\"Headers\\\":{\\\"string1\\\":\\\"value1\\\",\\\"string2\\\":\\\"value2\\\"},\\\"Query\\\":\\\"?api-version=1.0\\\"}\",\"Type\":\"Request\",\"CorrelationId\":\"202109111157004007-1921\",\"ResponseCode\":200}");
    }


    [Fact]
    public async Task Get_Returns200Response()
    {
      var mockResponse = _mockRestClient
        .MockApiResponse<PayloadModel>()
        .WithStatusCode(HttpStatusCode.OK)
        .Returns(RestClientHelpers.GetResponsePayload())
        .MockExecutePostAsync();

      _mockRestClient.Setup(x => x.ExecuteGetAsync(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(mockResponse);

      var result = await _fakeClass.Get("http://fakeapi.com/", "payload", null, RestClientHelpers.GetRandomDictionary());

      result.StatusCode.Should().Be(HttpStatusCode.OK);
      result.IsSuccessful.Should().BeTrue();
      result.Content.Should().Be("{\"Source\":\"http://fakeapi.com/payload\",\"Payload\":\"{\\\"Body\\\":\\\"{\\\\\\\"Node1\\\\\\\":{\\\\\\\"Node2\\\\\\\":\\\\\\\"Value1\\\\\\\"}}\\\",\\\"Headers\\\":{\\\"string1\\\":\\\"value1\\\",\\\"string2\\\":\\\"value2\\\"},\\\"Query\\\":\\\"?api-version=1.0\\\"}\",\"Type\":\"Request\",\"CorrelationId\":\"202109111157004007-1921\",\"ResponseCode\":200}");
    }

    [Fact]
    public async Task Get_ById_Returns200Response()
    {
      var mockResponse = _mockRestClient
        .MockApiResponse<PayloadModel>()
        .WithStatusCode(HttpStatusCode.OK)
        .Returns(RestClientHelpers.GetResponsePayload())
        .MockExecutePostAsync();

      _mockRestClient.Setup(x => x.ExecuteGetAsync(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(mockResponse);

      var result = await _fakeClass.GetById("http://fakeapi.com/", "payload", "123", null, RestClientHelpers.GetRandomDictionary());

      result.StatusCode.Should().Be(HttpStatusCode.OK);
      result.IsSuccessful.Should().BeTrue();
      result.Content.Should().Be("{\"Source\":\"http://fakeapi.com/payload\",\"Payload\":\"{\\\"Body\\\":\\\"{\\\\\\\"Node1\\\\\\\":{\\\\\\\"Node2\\\\\\\":\\\\\\\"Value1\\\\\\\"}}\\\",\\\"Headers\\\":{\\\"string1\\\":\\\"value1\\\",\\\"string2\\\":\\\"value2\\\"},\\\"Query\\\":\\\"?api-version=1.0\\\"}\",\"Type\":\"Request\",\"CorrelationId\":\"202109111157004007-1921\",\"ResponseCode\":200}");
    }

    #endregion

  }
}
