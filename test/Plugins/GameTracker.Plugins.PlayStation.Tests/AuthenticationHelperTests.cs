using GameTracker.Plugins.Common.Interfaces;
using GameTracker.Plugins.PlayStation.Helpers;
using GameTracker.Plugins.PlayStation.Models.Authentication;
using Moq;
using System.Net;
using System.Text.Json;

namespace GameTracker.Plugins.PlayStation.Tests
{
    public class AuthenticationHelperTests
    {
        private readonly Mock<IHttpClientWrapper> _mockHttpClient;
        private readonly AuthenticationHelper _authenticationHelper;

        public AuthenticationHelperTests()
        {
            _mockHttpClient = new Mock<IHttpClientWrapper>();

            var httpClientWrapperFactory = new Mock<IHttpClientWrapperFactory>();
            httpClientWrapperFactory.Setup(x => x.BuildHttpClient())
                .Returns(_mockHttpClient.Object);

            _authenticationHelper = new AuthenticationHelper(httpClientWrapperFactory.Object);
        }

        [Fact]
        public async Task ExchangeNpssoForCode_GivenNPSSOToken_CallsAPIToExchangeCode()
        {
            // Arrange
            var npsso = "abcd1234";
            var code = "defg5678";

            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.Redirect
            };

            response.Headers.Location = new Uri($"http://localhost?code={code}");

            _mockHttpClient.Setup(x => x.SendAsync(It.Is<HttpRequestMessage>(m => m.Headers.Any(h => h.Key == "Cookie" && h.Value.Any(v => v.Contains(npsso))))))
                .ReturnsAsync(response)
                .Verifiable();

            // Act
            var result = await _authenticationHelper.ExchangeNpssoForCode(npsso);

            // Assert
            Assert.Equal(code, result);
            _mockHttpClient.Verify();
        }

        [Fact]
        public async Task ExchangeNpssoForCode_IfAPICallFails_Throws()
        {
            // Arrange
            var npsso = "abcd1234";
            var code = "defg5678";

            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.InternalServerError
            };

            response.Headers.Location = new Uri($"http://localhost?code={code}");

            _mockHttpClient.Setup(x => x.SendAsync(It.Is<HttpRequestMessage>(m => m.Headers.Any(h => h.Key == "Cookie" && h.Value.Any(v => v.Contains(npsso))))))
                .ReturnsAsync(response)
                .Verifiable();

            // Act / Assert
            await Assert.ThrowsAsync<ApplicationException>(() => _authenticationHelper.ExchangeNpssoForCode(npsso));
            _mockHttpClient.Verify();
        }

        [Fact]
        public async Task ExchangeNpssoForCode_IfAPIDoesNotReturnToken_Throws()
        {
            // Arrange
            var npsso = "abcd1234";
            var code = "defg5678";

            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.Redirect
            };

            _mockHttpClient.Setup(x => x.SendAsync(It.Is<HttpRequestMessage>(m => m.Headers.Any(h => h.Key == "Cookie" && h.Value.Any(v => v.Contains(npsso))))))
                .ReturnsAsync(response)
                .Verifiable();

            // Act / Assert
            await Assert.ThrowsAsync<ApplicationException>(() => _authenticationHelper.ExchangeNpssoForCode(npsso));
            _mockHttpClient.Verify();
        }

        [Fact]
        public async Task ExchangeCodeForToken_GivenAuthenticationCode_QueriesAPIForJWTToken()
        {
            // Arrange
            var code = "defg5678";
            var token = new TokenExchangeResponse
            {
                AccessToken = "hijk9101"
            };
            var response = new HttpResponseMessage 
            { 
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonSerializer.Serialize(token))
            };

            _mockHttpClient.Setup(x => x.SendAsync(It.IsAny<HttpRequestMessage>()))
                .ReturnsAsync(response)
                .Verifiable();

            // Act
            var result = await _authenticationHelper.ExchangeCodeForToken(code);

            // Assert
            Assert.Equal(token.AccessToken, result);
            _mockHttpClient.Verify();
        }

        [Fact]
        public async Task ExchangeCodeForToken_IfAPICallFails_Throws()
        {
            // Arrange
            var code = "defg5678";
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.InternalServerError
            };

            _mockHttpClient.Setup(x => x.SendAsync(It.IsAny<HttpRequestMessage>()))
                .ReturnsAsync(response)
                .Verifiable();

            // Act / Assert
            await Assert.ThrowsAsync<ApplicationException>(() => _authenticationHelper.ExchangeCodeForToken(code));
            _mockHttpClient.Verify();
        }
    }
}