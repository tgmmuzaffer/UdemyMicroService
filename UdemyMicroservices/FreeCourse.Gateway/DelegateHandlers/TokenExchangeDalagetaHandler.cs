using FreeCourse.Gateway.Models;
using IdentityModel.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace FreeCourse.Gateway.DelegateHandlers
{
    public class TokenExchangeDalagetaHandler : DelegatingHandler
    {
        private readonly HttpClient _httpClient;
        private string _accessToken;
        //private readonly AppSetting _appSetting;
        private readonly IConfiguration _configuration;

        public TokenExchangeDalagetaHandler(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            //_appSetting = options.Value;
            _configuration = configuration;
        }
        private async Task<string> GetToken(string requestToken)
        {
            if (!string.IsNullOrEmpty(_accessToken))
            {
                return _accessToken;

            }
            var discoEndpoint = await _httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = _configuration["IdentityServerURL"],
                Policy = new DiscoveryPolicy { RequireHttps = false }
            });

            if (discoEndpoint.IsError)
            {
                throw discoEndpoint.Exception;
            }

            TokenExchangeTokenRequest tokenExchangeTokenRequest = new TokenExchangeTokenRequest
            {
                Address = discoEndpoint.TokenEndpoint,
                ClientId = _configuration["ClientId"],
                ClientSecret = _configuration["ClientSecrets"],
                GrantType = _configuration["TokenGrantType"],
                SubjectToken = requestToken,
                SubjectTokenType= "urn:ietf:params:oauth:token-type:access-token",
                Scope= "openid discount_fullpermission fake_payment_fullpermission"
            };

            var tokenResponse = await _httpClient.RequestTokenExchangeTokenAsync(tokenExchangeTokenRequest);
            if (tokenResponse.IsError)
            {
                throw tokenResponse.Exception;
            }
            _accessToken = tokenResponse.AccessToken;
            return _accessToken;
        }
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var requesttoken = request.Headers.Authorization.Parameter;
            var newToken =await GetToken(requesttoken);
            request.SetBearerToken(newToken);
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
