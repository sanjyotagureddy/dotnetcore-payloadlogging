using FluentAssertions;
using PayloadLogging.Common.Extensions;
using System.Text;
using Xunit;

namespace PayloadLogging.Common.UnitTest.Extensions
{
  public class StringExtensionsTests
  {
    #region ToSnakeCase

    [Fact]
    public void ToSnakeCase_ShouldReturnStringIfEmpty()
    {
      const string value = "";
      var result = value.ToSnakeCase();
      result.Should().Be(string.Empty);
    }

    [Fact]
    public void ToSnakeCase_ShouldReturnSnakeCaseString()
    {
      const string value = "ClientDetailId";
      const string expectedValue = "client_detail_id";
      var result = value.ToSnakeCase();
      result.Should().Be(expectedValue);
    }

    #endregion ToSnakeCase

    #region MinifyJson

    [Fact]
    public void MinifyJsonText_ShouldEmptyString()
    {
      const string value = "";
      var result = value.MinifyJsonText();
      result.Should().Be(string.Empty);
    }

    [Fact]
    public void MinifyJsonText_ShouldReturnMinifiedJsonString()
    {
      var stringBuilder = new StringBuilder()
        .AppendLine("{")
        .AppendLine("\t\"Node1\": {")
        .AppendLine("\t\t\"Node2\": \"Value1\"")
        .AppendLine("\t}")
        .AppendLine("}");

      var result = stringBuilder.ToString().MinifyJsonText();
      result.Should().Be("{\"Node1\":{\"Node2\":\"Value1\"}}");
    }

    #endregion MinifyJson
  }
}