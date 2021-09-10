using System.Threading.Tasks;

namespace EdFi.RtI.Core.OdsApi
{
    public interface IOdsApiClientProvider
    {
        Task<OdsApiClient> NewCompositesClient();
        Task<OdsApiClient> NewResourcesClient();
    }

    public class OdsApiClientProvider : IOdsApiClientProvider
    {
        private readonly IOdsApiClientFactory _odsApiClientFactory;
        private readonly IOdsApiSettingsProvider _odsApiSettingsProvider;

        public OdsApiClientProvider(IOdsApiClientFactory odsApiClientFactory, IOdsApiSettingsProvider odsApiSettingsProvider)
        {
            _odsApiClientFactory = odsApiClientFactory;
            _odsApiSettingsProvider = odsApiSettingsProvider;
        }

        public async Task<OdsApiClient> NewCompositesClient()
        {
            var odsApiSettings = await _odsApiSettingsProvider.GetOdsApiSettings();
            return _odsApiClientFactory.NewCompositesClient(odsApiSettings);
        }
        
        public async Task<OdsApiClient> NewResourcesClient()
        {
            var odsApiSettings = await _odsApiSettingsProvider.GetOdsApiSettings();
            return _odsApiClientFactory.NewResourcesClient(odsApiSettings);
        }
    }
}
