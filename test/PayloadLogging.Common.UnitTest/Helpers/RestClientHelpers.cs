using PayloadLogging.Common.Models.PayloadLogging;
using PayloadLogging.Common.Models.PayloadLogging.Enum;
using System.Collections.Generic;

namespace PayloadLogging.Common.UnitTest.Helpers
{
  public static class RestClientHelpers
  {
    public static PayloadModel GetRequestPayload()
    {
      return new PayloadModel
      {
        CorrelationId = "202109111157004007-1921",
        Type = nameof(PayloadType.Request),
        Source = "http://fakeapi.com/payload",
        Payload = "{\"Node1\":{\"Node2\":\"Value1\"}}",
        Headers = "{'Ellie':'Rutherford', 'Nikko':'Skiles', 'Lucius':'Hartmann'}",
        Query = "?name=test",
        HttpVerb = "POST"
      };
    }

    public static PayloadModel GetResponsePayload()
    {
      return new PayloadModel
      {
        CorrelationId = "202109111157004007-1921",
        Type = nameof(PayloadType.Request),
        Source = "http://fakeapi.com/payload",
        Payload = "{\"Node1\":{\"Node2\":\"Value1\"}}",
        ResponseCode = 200,
        Headers = "{'Ellie':'Rutherford', 'Nikko':'Skiles', 'Lucius':'Hartmann'}",
        Query = "?name=test",
        HttpVerb = "POST"
      };
    }

    public static IDictionary<string, string> GetRandomDictionary() =>
      new Dictionary<string, string>()
      {
        {Faker.Name.First(), Faker.Name.Last()},
        {Faker.Name.First(), Faker.Name.Last()},
        {Faker.Name.First(), Faker.Name.Last()}
      };
  }
}