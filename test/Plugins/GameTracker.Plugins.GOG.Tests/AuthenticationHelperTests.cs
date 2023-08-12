using GameTracker.Plugins.Common.Interfaces;
using GameTracker.Plugins.GOG.Helpers;
using GameTracker.Plugins.GOG.Models.GOGApi;
using System.Net;
using System.Text.Json;

namespace GameTracker.Plugins.GOG.Tests
{
    public class AuthenticationHelperTests
    {
        private readonly IHttpClientWrapper _mockHttpClient;
        private readonly AuthenticationHelper _authHelper;

        public AuthenticationHelperTests()
        {
            _mockHttpClient = Substitute.For<IHttpClientWrapper>();

            var httpClientFactory = Substitute.For<IHttpClientWrapperFactory>();
            httpClientFactory.BuildHttpClient()
                .Returns(_mockHttpClient);

            _authHelper = new AuthenticationHelper(httpClientFactory);
        }

        [Fact]
        public async Task ExchangeGogAuthCodeForToken_IfApiRequestSucceeds_ReturnsAuthToken()
        {
            // Arrange
            var gogCode = "abcd1234";
            var authToken = new AuthToken
            {
                AccessToken = "defg5678"
            };

            _mockHttpClient.GetAsync(Arg.Is<Uri>(uri => uri.ToString().Contains(Constants.Authentication.AuthenticationBaseUrl)))
                .Returns(new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = new StringContent(JsonSerializer.Serialize(authToken)) });

            // Act
            var result = await _authHelper.ExchangeGogAuthCodeForToken(gogCode);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(authToken.AccessToken, result.AccessToken);
            await _mockHttpClient.Received(1).GetAsync(Arg.Any<Uri>());
        }

        [Fact]
        public async Task ExchangeGogAuthCodeForToken_IfApiRequestFails_Throws()
        {
            // Arrange
            var gogCode = "abcd1234";

            _mockHttpClient.GetAsync(Arg.Is<Uri>(uri => uri.ToString().Contains(Constants.Authentication.AuthenticationBaseUrl)))
                .Returns(new HttpResponseMessage { StatusCode = HttpStatusCode.InternalServerError });

            // Act / Assert
            await Assert.ThrowsAsync<ApplicationException>(() => _authHelper.ExchangeGogAuthCodeForToken(gogCode));
            await _mockHttpClient.Received(1).GetAsync(Arg.Any<Uri>());
        }
    }
}