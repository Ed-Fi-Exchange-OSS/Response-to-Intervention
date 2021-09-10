using EdFi.Ods.Api.Client.Models;
using EdFi.RtI.Core.DomainServiceProvider;
using EdFi.RtI.Core.Mapper;
using EdFi.RtI.Core.Models;
using EdFi.RtI.Core.OdsApi;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EdFi.RtI.Core.Services.Catalogs
{
    public class CatalogServiceV3 : ICatalogService
    {
        private readonly IDomainServiceProvider _domainServiceProvider;
        private readonly IOdsApiClientProvider _odsApiClientProvider;

        public CatalogServiceV3(IDomainServiceProvider domainServiceProvider, IOdsApiClientProvider odsApiClientProvider)
        {
            _domainServiceProvider = domainServiceProvider;
            _odsApiClientProvider = odsApiClientProvider;
        }

        private IEdFiMapper _mapper => _domainServiceProvider.GetService<IEdFiMapper>();

        public async Task<School> GetSchoolBySchoolId(int schoolId, bool getFromCache = true)
        {
            var odsApi = await _odsApiClientProvider.NewResourcesClient();

            return await odsApi.Get<School>("schools", new Dictionary<string, string>
            {
                { "schoolId", schoolId.ToString() },
            });
        }

        public async Task<IEnumerable<School>> GetSchoolsAll(bool getFromCache = true, bool storeInCache = true) 
        {
            var odsApi = await _odsApiClientProvider.NewCompositesClient();
            var schoolsFromApi = await odsApi.Get<IList<Schoolv3>>("enrollment/Schools");
            return schoolsFromApi.Select(a => a.MapToSchool()).ToList();
        }

        public async Task<IEnumerable<School>> GetSchoolsByStaff(int staffUniqueId)
        {
            var odsApi = await _odsApiClientProvider.NewResourcesClient();

            var associations = await odsApi.Get<IEnumerable<StaffSchoolAssociation>>("staffSchoolAssociations", new Dictionary<string, string>
            {
                { "staffUniqueId", staffUniqueId.ToString() },
            });

            var schools = new List<School>();

            foreach (var association in associations)
            {
                var schoolsFromApi = await odsApi.Get<IList<School>>("schools", new Dictionary<string, string>
                {
                    { "schoolId", association.SchoolReference.SchoolId.ToString() },
                });

                var foundSchool = schoolsFromApi.FirstOrDefault();

                if (foundSchool != null)
                    schools.Add(foundSchool);
            }

            return schools;
        }

        public async Task<IEnumerable<Section>> GetSectionsAll(bool getFromCache = true, bool storeInCache = true)
        {
            var odsApi = await _odsApiClientProvider.NewResourcesClient();
            var sectionsFromApi = await odsApi.Get<IEnumerable<Sectionv3>>("sections");
            return sectionsFromApi.Select(a => a.MapToSection()).ToList();
        }

        public async Task<IEnumerable<Section>> GetSectionsByStaff(string staffId, bool getFromCache = true, bool storeInCache = true)
        {
            var odsApi = await _odsApiClientProvider.NewCompositesClient();
            var sectionsFromApiv3 = await odsApi.Get<IList<EnrollmentSectionv3>>($"enrollment/staffs/{staffId}/sections");
            return sectionsFromApiv3.Select(a => a.MapToSection()).ToList();
        }

        public async Task<IEnumerable<Staff>> GetStaffsAll(bool getFromCache = true, bool storeInCache = true)
        {
            var odsApi = await _odsApiClientProvider.NewResourcesClient();
            var staffsFromApi = await odsApi.Get<IList<Staffv3>>("staffs");
            return staffsFromApi.Select(a => a.MapToStaff()).ToList();
        }

        public async Task<IEnumerable<Staff>> GetStaffsBySchool(string schoolId, bool getFromCache = true, bool storeInCache = false)
        {
            var odsApi = await _odsApiClientProvider.NewCompositesClient();
            var staffsFromApi = await odsApi.Get<IList<Staff>>($"enrollment/schools/{schoolId}/staffs");
            return _mapper.Secured(staffsFromApi);
        }
    }
}
