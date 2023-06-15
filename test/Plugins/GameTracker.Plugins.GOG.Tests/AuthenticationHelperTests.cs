using GameTracker.Plugins.Common.Interfaces;
using GameTracker.Plugins.GOG.Helpers;
using GameTracker.Plugins.GOG.Models.GOGApi;
using Moq;
using System.Net;
using System.Text.Json;

namespace GameTracker.Plugins.GOG.Tests
{
    public class AuthenticationHelperTests
    {
        private readonly Mock<IHttpClientWrapper> _mockHttpClient;
        private readonly AuthenticationHelper _authHelper;

        public AuthenticationHelperTests()
        {
            _mockHttpClient = new Mock<IHttpClientWrapper>();

            var httpClientFactory = new Mock<IHttpClientWrapperFactory>();
            httpClientFactory.Setup(x => x.BuildHttpClient())
                .Returns(_mockHttpClient.Object);

            _authHelper = new AuthenticationHelper(httpClientFactory.Object);
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

            _mockHttpClient.Setup(x => x.GetAsync(It.Is<Uri>(uri => uri.ToString().Contains(Constants.Authentication.AuthenticationBaseUrl))))
                .ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = new StringContent(JsonSerializer.Serialize(authToken)) })
                .Verifiable();

            // Act
            var result = await _authHelper.ExchangeGogAuthCodeForToken(gogCode);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(authToken.AccessToken, result.AccessToken);
            _mockHttpClient.Verify();
        }

        [Fact]
        public async Task ExchangeGogAuthCodeForToken_IfApiRequestFails_Throws()
        {
            // Arrange
            var gogCode = "abcd1234";
            _mockHttpClient.Setup(x => x.GetAsync(It.Is<Uri>(uri => uri.ToString().Contains(Constants.Authentication.AuthenticationBaseUrl))))
                .ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.InternalServerError })
                .Verifiable();

            // Act / Assert
            await Assert.ThrowsAsync<ApplicationException>(() => _authHelper.ExchangeGogAuthCodeForToken(gogCode));
            _mockHttpClient.Verify();
        }
    }
}