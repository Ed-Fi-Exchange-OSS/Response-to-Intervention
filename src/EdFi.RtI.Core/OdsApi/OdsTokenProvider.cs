using EdFi.RtI.Core.Infrastructure;
using Newtonsoft.Json;
using RestSharp;
using System;

namespace EdFi.RtI.Core.OdsApi
{
    public interface IOdsTokenProvider
    {
        string GetAccessToken(OdsTokenSettings settings);
    }

    public class OdsTokenProvider : IOdsTokenProvider
    {
        public string GetAccessToken(OdsTokenSettings settings)
        {
            if (settings.Version == EdFiVersion.v2)
                return GetAccessTokenV2(settings);

            if (settings.Version == EdFiVersion.v3)
                return GetAccessTokenV3(settings);

            throw new NotImplementedException($"Access token retreiver not supported for Ed-Fi version {settings.Version}");
        }

        private string GetAccessTokenV2(OdsTokenSettings settings)
        {
            var authCodeResponse = GetAuthCodeResponse(settings);
            var authTokenResponse = GetAuthTokenResponse(settings, authCodeResponse);
            return authTokenResponse.Access_token;
        }

        private string GetAccessTokenV3(OdsTokenSettings settings)
        {
            var authTokenResponse = GetAuthTokenResponse(settings);
            return authTokenResponse.Access_token;
        }

        private AuthCodeResponse GetAuthCodeResponse(OdsTokenSettings settings)
        {
            var client = new RestClient(settings.AuthUrl)
            {
                Timeout = -1
            };

            var request = new RestRequest(Method.POST);
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddParameter("Client_id", settings.ClientId);
            request.AddParameter("Response_type", "code");

            IRestResponse response = client.Execute(request);

            return JsonConvert.DeserializeObject<AuthCodeResponse>(response.Content);
        }

        private TokenResponse GetAuthTokenResponse(OdsTokenSettings settings, AuthCodeResponse result)
        {
            var client = new RestClient(settings.TokenUrl);

            var request = new RestRequest(Method.POST);
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddParameter("Client_id", settings.ClientId);
            request.AddParameter("Client_secret", settings.ClientSecret);
            request.AddParameter("Code", result.Code);
            request.AddParameter("Grant_type", "authorization_code");

            var response = client.Execute(request);

            return JsonConvert.DeserializeObject<TokenResponse>(response.Content);
        }

        private TokenResponse GetAuthTokenResponse(OdsTokenSettings settings)
        {
            var client = new RestClient(settings.TokenUrl);

            var request = new RestRequest(Method.POST);
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddParameter("Client_id", settings.ClientId);
            request.AddParameter("Client_secret", settings.ClientSecret);
            request.AddParameter("Grant_type", "client_credentials");

            var response = client.Execute(request);

            return JsonConvert.DeserializeObject<TokenResponse>(response.Content);
        }

        private class AuthCodeResponse
        {
            public string Code { get; set; }
        }

        private class TokenResponse
        {
            public string Access_token { get; set; }
            public string Token_type { get; set; }
            public int Expires_in { get; set; }
        }
    }
}
