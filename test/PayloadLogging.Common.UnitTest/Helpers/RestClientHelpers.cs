using System.Collections.Generic;
using Newtonsoft.Json;
using PayloadLogging.Common.Models.PayloadLogging;
using PayloadLogging.Common.Models.PayloadLogging.Enum;

namespace PayloadLogging.Common.UnitTest.Helpers
{
  public class RestClientHelpers
  {
    public static PayloadModel GetRequestPayload()
    {
      var payloadContent = GetPayloadContent();
      return new PayloadModel
      {
        CorrelationId = "202109111157004007-1921",
        Type = PayloadType.Request.ToString(),
        Source = "http://fakeapi.com/payload",
        Payload = JsonConvert.SerializeObject(payloadContent)
      };
    }

    public static PayloadModel GetResponsePayload()
    {
      var payloadContent = GetPayloadContent();
      return new PayloadModel
      {
        CorrelationId = "202109111157004007-1921",
        Type = PayloadType.Request.ToString(),
        Source = "http://fakeapi.com/payload",
        Payload = JsonConvert.SerializeObject(payloadContent),
        ResponseCode = 200
      };
    }

    public static IDictionary<string, string> GetRandomDictionary() =>
      new Dictionary<string, string>()
      {
        {Faker.Name.First(), Faker.Name.Last()},
        {Faker.Name.First(), Faker.Name.Last()},
        {Faker.Name.First(), Faker.Name.Last()}
      };

    private static PayloadContent GetPayloadContent()
      => new()
      {
        Query = "?api-version=1.0",
        Headers = new Dictionary<string, string>
        {
          {"string1", "value1"}, {"string2", "value2"}
        },
        Body = "{\"Node1\":{\"Node2\":\"Value1\"}}"
      };

    
  }
}
