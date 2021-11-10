using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using PayloadLogging.Common.Extensions;
using System.Collections.Generic;
using Xunit;

namespace PayloadLogging.Common.UnitTest.Extensions
{
  public class RestRequestExtensionsTests
  {
    #region ToStringValue

    [Fact]
    public void ToStringValue_EmptyDictionary_ReturnEmptyStrings()
    {
      var dictionary = new Dictionary<string, string>();
      var result = dictionary.ToStringValue();
      result.Should().Be(string.Empty);
    }

    [Fact]
    public void ToStringValue_NullDictionary_ReturnEmptyStrings()
    {
      var result = ((Dictionary<string, string>)null).ToStringValue();
      result.Should().Be(string.Empty);
    }

    [Fact]
    public void ToStringValue_ValidDictionary_ReturnStrings()
    {
      var dictionary = new Dictionary<string, string>
      {
        {"string1", "value1"},
        {"string2", "value2"}
      };
      var result = dictionary.ToStringValue();
      result.Should().Be("{'string1':'value1', 'string2':'value2'}");
    }

    #endregion ToStringValue

    #region IQueryCollection to IDictionary<string,string>

    [Fact]
    public void ToDictionary_NullIQueryCollection_ReturnsNull()
    {
      var resultDictionary = ((QueryCollection)null).ToStringDictionary();
      resultDictionary.Should().BeNull();
    }

    [Fact]
    public void ToDictionary_ValidIQueryCollection_ReturnsIDictionary()
    {
      var dictionary = new Dictionary<string, StringValues>
      {
        {"string1", "value1"},
        {"string2", "value2"}
      };
      var iQueryDictionary = new QueryCollection(dictionary);
      var resultDictionary = iQueryDictionary.ToStringDictionary();

      resultDictionary.Count.Should().Be(2);
      resultDictionary.ContainsKey("string1").Should().BeTrue();
      resultDictionary.ContainsKey("string2").Should().BeTrue();
    }

    #endregion IQueryCollection to IDictionary<string,string>

    #region IHeaderDictionary to IDictionary<string,string>

    [Fact]
    public void ToDictionary_NullIHeaderDictionary_ReturnsNull()
    {
      var resultDictionary = ((HeaderDictionary)null).ToStringDictionary();
      resultDictionary.Should().BeNull();
    }

    [Fact]
    public void ToDictionary_ValidIHeaderDictionary_ReturnsIDictionary()
    {
      var iQueryDictionary = new HeaderDictionary{
        {"string1", "value1"},
        {"string2", "value2"}
      };
      var resultDictionary = iQueryDictionary.ToStringDictionary();

      resultDictionary.Count.Should().Be(2);
      resultDictionary.ContainsKey("string1").Should().BeTrue();
      resultDictionary.ContainsKey("string2").Should().BeTrue();
    }

    #endregion IHeaderDictionary to IDictionary<string,string>
  }
}