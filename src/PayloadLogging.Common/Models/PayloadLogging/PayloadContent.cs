using System.Collections.Generic;

namespace PayloadLogging.Common.Models.PayloadLogging
{
  public class PayloadContent
  {
    public string Body { get; set; }
    public IDictionary<string,string> Headers { get; set; }
    public string Query { get; set; }
  }
}
