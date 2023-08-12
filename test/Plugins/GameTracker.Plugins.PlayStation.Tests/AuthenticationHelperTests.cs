using GameTracker.Plugins.Common.Interfaces;
using GameTracker.Plugins.PlayStation.Helpers;
using GameTracker.Plugins.PlayStation.Models.Authentication;
using System.Net;
using System.Text.Json;

namespace GameTracker.Plugins.PlayStation.Tests
{
    public class AuthenticationHelperTests
    {
        private readonly IHttpClientWrapper _mockHttpClient;
        private readonly AuthenticationHelper _authenticationHelper;

        public AuthenticationHelperTests()
        {
            _mockHttpClient = Substitute.For<IHttpClientWrapper>();

            var httpClientWrapperFactory = Substitute.For<IHttpClientWrapperFactory>();
            httpClientWrapperFactory.BuildHttpClient()
                .Returns(_mockHttpClient);

            _authenticationHelper = new AuthenticationHelper(httpClientWrapperFactory);
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

            _mockHttpClient.SendAsync(Arg.Is<HttpRequestMessage>(m => m.Headers.Any(h => h.Key == "Cookie" && h.Value.Any(v => v.Contains(npsso)))))
                .Returns(response);

            // Act
            var result = await _authenticationHelper.ExchangeNpssoForCode(npsso);

            // Assert
            Assert.Equal(code, result);
            await _mockHttpClient.Received(1).SendAsync(Arg.Any<HttpRequestMessage>());
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

            _mockHttpClient.SendAsync(Arg.Is<HttpRequestMessage>(m => m.Headers.Any(h => h.Key == "Cookie" && h.Value.Any(v => v.Contains(npsso)))))
                .Returns(response);

            // Act / Assert
            await Assert.ThrowsAsync<ApplicationException>(() => _authenticationHelper.ExchangeNpssoForCode(npsso));
            await _mockHttpClient.Received(1).SendAsync(Arg.Any<HttpRequestMessage>());
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

            _mockHttpClient.SendAsync(Arg.Is<HttpRequestMessage>(m => m.Headers.Any(h => h.Key == "Cookie" && h.Value.Any(v => v.Contains(npsso)))))
                .Returns(response);

            // Act / Assert
            await Assert.ThrowsAsync<ApplicationException>(() => _authenticationHelper.ExchangeNpssoForCode(npsso));
            await _mockHttpClient.Received(1).SendAsync(Arg.Any<HttpRequestMessage>());
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

            _mockHttpClient.SendAsync(Arg.Any<HttpRequestMessage>())
                .Returns(response);

            // Act
            var result = await _authenticationHelper.ExchangeCodeForToken(code);

            // Assert
            Assert.Equal(token.AccessToken, result);
            await _mockHttpClient.Received(1).SendAsync(Arg.Any<HttpRequestMessage>());
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

            _mockHttpClient.SendAsync(Arg.Any<HttpRequestMessage>())
                .Returns(response);

            // Act / Assert
            await Assert.ThrowsAsync<ApplicationException>(() => _authenticationHelper.ExchangeCodeForToken(code));
            await _mockHttpClient.Received(1).SendAsync(Arg.Any<HttpRequestMessage>());
        }
    }
}