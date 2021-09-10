using EdFi.Ods.Api.Client;
using EdFi.RtI.Core.Infrastructure;
using EdFi.RtI.Core.Services.Settings;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.OdsApi
{
    public interface IOdsApiClientFactory
    {
        OdsApiClient NewCompositesClient(OdsApiSettings settings);
        OdsApiClient NewResourcesClient(OdsApiSettings settings);
    }

    public class OdsApiClientFactory : IOdsApiClientFactory
    {
        private readonly IOdsTokenProvider _odsTokenRetreiver;

        public OdsApiClientFactory(IOdsTokenProvider odsTokenRetreiver)
        {
            _odsTokenRetreiver = odsTokenRetreiver;
        }

        public OdsApiClient NewCompositesClient(OdsApiSettings settings)
        {
            EdFiOdsResourcesClient client;

            if (settings.Version == EdFiVersion.v2)
                client = BuildEdFiOdsV2Client(settings, settings.ResourcesUrl);

            else if (settings.Version == EdFiVersion.v3)
                client = BuildEdFiOdsV3Client(settings, settings.CompositesUrl);

            else
                throw new ArgumentException(nameof(settings.Version));

            return new OdsApiClient(client);
        }

        public OdsApiClient NewResourcesClient(OdsApiSettings settings)
        {
            EdFiOdsResourcesClient client;

            if (settings.Version == EdFiVersion.v2)
                client = BuildEdFiOdsV2Client(settings, settings.ResourcesUrl);

            else if (settings.Version == EdFiVersion.v3)
                client = BuildEdFiOdsV3Client(settings, settings.ResourcesUrl);

            else
                throw new ArgumentException(nameof(settings.Version));

            return new OdsApiClient(client);
        }

        private EdFiOdsResourcesClient BuildEdFiOdsV2Client(OdsApiSettings settings, string baseUrl)
        {
            var edFiOdsClient = new EdFiOdsResourcesClient(new Uri(baseUrl, UriKind.Absolute));
            var accessToken = _odsTokenRetreiver.GetAccessToken(settings.AsOdsTokenSettings());
            edFiOdsClient.HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");
            return edFiOdsClient;
        }

        private EdFiOdsResourcesClient BuildEdFiOdsV3Client(OdsApiSettings settings, string baseUrl)
        {
            var edFiOdsClient = new EdFiOdsResourcesClient(new Uri(baseUrl, UriKind.Absolute));
            var accessToken = _odsTokenRetreiver.GetAccessToken(settings.AsOdsTokenSettings());
            edFiOdsClient.HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");
            return edFiOdsClient;
        }

    }

    static class OdsApiSettingsExtensions
    {
        public static OdsTokenSettings AsOdsTokenSettings(this OdsApiSettings settings) => new OdsTokenSettings
        {
            AuthUrl = settings.AuthUrl,
            ClientId = settings.ClientId,
            ClientSecret = settings.ClientSecret,
            TokenUrl = settings.TokenUrl,
            Version = settings.Version,
        };
    }
}
