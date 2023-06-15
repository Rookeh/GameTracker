using GameTracker.Plugins.Common.Helpers;

namespace GameTracker.Plugins.Common.Tests
{
    public class UriHelperTests
    {
        [Fact]
        public void BuildQueryString_GivenBaseUrlAndParameters_ConstructsQueryString()
        {
            // Arrange
            var baseUrl = "http://localhost";
            var queryParams = new Dictionary<string, string>
            {
                ["foo"] = "bar",
                ["baz"] = "quux"
            };

            // Act
            var result = UriHelper.BuildQueryString(baseUrl, queryParams);

            // Assert
            Assert.Equal("http://localhost/?foo=bar&baz=quux", result.ToString());
        }

        [Fact]
        public void ExtractQueryString_GivenURL_ReturnsDictionaryOfQueryStringParameters()
        {
            // Arrange
            var url = "http://localhost/?foo=bar&baz=quux";

            // Act
            var result = UriHelper.ExtractQueryString(url);

            // Assert
            Assert.True(result.ContainsKey("foo"));
            Assert.Equal("bar", result["foo"]);
            Assert.True(result.ContainsKey("baz"));
            Assert.Equal("quux", result["baz"]);
        }
    }
}