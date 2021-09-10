using EdFi.Ods.Api.Client;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EdFi.RtI.Core.OdsApi
{
    public class OdsApiClient
    {
        private readonly EdFiOdsResourcesClient _edFiOdsResourcesClient;

        public OdsApiClient(EdFiOdsResourcesClient edFiOdsResourcesClient)
        {
            _edFiOdsResourcesClient = edFiOdsResourcesClient;
        }

        private HttpClient HttpClient
        {
            get {
                return _edFiOdsResourcesClient.HttpClient;
            }
        }

        public async Task<HttpResponseMessage> Delete(string resourceName)
        {
            var url = BuildRequestUrl(resourceName);
            return await HttpClient.DeleteAsync(url);
        }

        public async Task<HttpResponseMessage> Get(string resourceName, IEnumerable<KeyValuePair<string, string>> queryParams = null)
        {
            var url = BuildRequestUrl(resourceName, queryParams);
            return await HttpClient.GetAsync(url);
        }

        public async Task<T> Get<T>(string resourceName, IEnumerable<KeyValuePair<string, string>> queryParams = null)
        {
            var url = BuildRequestUrl(resourceName, queryParams);
            var response = await HttpClient.GetAsync(url);
            return await HandleHttpResponseGetContent<T>(response);
        }

        public async Task HandleHttpResponse(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                throw new Exception($"Request failed with status code {(int)response.StatusCode} {response.ReasonPhrase}:\n{content}");
            }
        }

        public async Task<T> HandleHttpResponseGetContent<T>(HttpResponseMessage response)
        {
            await HandleHttpResponse(response);
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(content);
        }

        public async Task<string> HandleHttpResponseGetCreatedResourceId(HttpResponseMessage response)
        {
            await HandleHttpResponse(response);
            var createdResourcePath = response.Headers.Location.AbsoluteUri;
            var createdResourcePathParts = createdResourcePath.Split('/');
            return createdResourcePathParts[createdResourcePathParts.Length - 1];
        }

        public async Task<HttpResponseMessage> Post(string resourceName, object body)
        {
            var url = BuildRequestUrl(resourceName);
            var json = JsonConvert.SerializeObject(body);
            return await HttpClient.PostAsync(url, new StringContent(json, Encoding.UTF8, "application/json"));
        }

        public async Task<HttpResponseMessage> Put(string resourcePath, object body)
        {
            var url = BuildRequestUrl(resourcePath);
            var json = JsonConvert.SerializeObject(body);
            return await HttpClient.PutAsync(url, new StringContent(json, Encoding.UTF8, "application/json"));
        }

        private string BuildQueryParamsString(IEnumerable<KeyValuePair<string, string>> queryParams)
        {
            var parameters = queryParams.Select(entry => $"{entry.Key}={entry.Value}");
            return string.Join('&', parameters);
        }

        private string BuildRequestUrl(string resourceName, IEnumerable<KeyValuePair<string, string>> queryParams = null)
        {
            var finalUrl = new StringBuilder();
            var baseUrl = _edFiOdsResourcesClient.BaseUri.AbsoluteUri;

            finalUrl.Append(baseUrl).Append(resourceName);

            if (queryParams != null)
            {
                var paramString = BuildQueryParamsString(queryParams);
                finalUrl.Append('?').Append(paramString);
            }

            return finalUrl.ToString();
        }
    }
}
