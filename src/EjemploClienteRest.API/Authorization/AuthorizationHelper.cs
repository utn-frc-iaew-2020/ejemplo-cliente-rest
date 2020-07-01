using System.Threading.Tasks;
using Tiny.RestClient;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

namespace ClienteRestAPI.Authorization
{
    public class AuthorizationHelper
    {
        private static OAuthAccessToken accessToken;

        private static ILogger<AuthorizationHelper> _logger;
        private static IConfiguration _config;
        public AuthorizationHelper(ILogger<AuthorizationHelper> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }
        public async static Task<string> ObtenerAccessToken()
        {
            if (accessToken  == null)
            {
                var clientOAuth = new TinyRestClient(new HttpClient(), _config["OAuth0::UrlAuth0"]);

                var request = new OAuthRequest()
                {
                    audience = _config["OAuth0::Audience"],
                    client_id = _config["OAuth0::ClientId"],
                    client_secret = _config["OAuth0::ClientSecret"],
                    grant_type = "client_credentials"
                };

                accessToken = await clientOAuth.PostRequest("token", request).
                                    ExecuteAsync<OAuthAccessToken>();

            }

            return accessToken.access_token;

        }
    }
}