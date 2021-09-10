using EdFi.Ods.Api.Client.Models;
using EdFi.RtI.Core.DomainServiceProvider;
using EdFi.RtI.Core.Mapper;
using EdFi.RtI.Core.OdsApi;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EdFi.RtI.Core.Services.Catalogs
{
    public class CatalogServiceV2 : ICatalogService
    {
        private readonly IDomainServiceProvider _domainServiceProvider;
        private readonly IOdsApiClientProvider _odsApiClientProvider;

        public CatalogServiceV2(IDomainServiceProvider domainServiceProvider, IOdsApiClientProvider odsApiClientProvider)
        {
            _domainServiceProvider = domainServiceProvider;
            _odsApiClientProvider = odsApiClientProvider;
        }

        private IEdFiMapper _mapper => _domainServiceProvider.GetService<IEdFiMapper>();

        public async Task<IEnumerable<School>> GetSchoolsAll(bool getFromCache = true, bool storeInCache = true)
        {
            var ods = await _odsApiClientProvider.NewResourcesClient();
            return await ods.Get<IList<School>>("schools");
        }

        public async Task<IEnumerable<School>> GetSchoolsByStaff(int staffUniqueId)
        {
            var ods = await _odsApiClientProvider.NewResourcesClient();

            var staffSchoolAssociations = await ods.Get<IList<StaffSchoolAssociation>>("staffSchoolAssociations", new Dictionary<string, string>
            {
                { "staffUniqueId", staffUniqueId.ToString() },
            });

            var schools = new List<School>();

            foreach (var association in staffSchoolAssociations)
            {
                var foundSchool = await ods.Get<School>("schools", new Dictionary<string, string>
                {
                    { "schoolId", association.SchoolReference.SchoolId.ToString() },
                });

                schools.Add(foundSchool);
            }

            return schools;
        }

        public async Task<School> GetSchoolBySchoolId(int schoolId, bool getFromCache = true)
        {
            var ods = await _odsApiClientProvider.NewResourcesClient();

            var schools = await ods.Get<IList<School>>($"schools", new Dictionary<string, string>
            {
                ["schoolId"] = schoolId.ToString(),
            });

            var school = schools.FirstOrDefault();

            if (school == null)
                throw new SchoolNotFoundException(schoolId.ToString());

            return school;
        }
        
        public async Task<IEnumerable<Section>> GetSectionsAll(bool getFromCache = true, bool storeInCache = true)
        {
            var ods = await _odsApiClientProvider.NewResourcesClient();
            return await ods.Get<IList<Section>>("sections");
        }

        public async Task<IEnumerable<Section>> GetSectionsByStaff(string staffId, bool getFromCache = true, bool storeInCache = true)
        {
            var ods = await _odsApiClientProvider.NewResourcesClient();
            return await ods.Get<IList<Section>>($"enrollment/staffs/{staffId}/sections");
        }

        public async Task<IEnumerable<Staff>> GetStaffsAll(bool getFromCache = true, bool storeInCache = true)
        {
            var ods = await _odsApiClientProvider.NewResourcesClient();
            return await ods.Get<IList<Staff>>("enrollment/staffs");
        }

        public async Task<IEnumerable<Staff>> GetStaffsBySchool(string schoolId, bool getFromCache = true, bool storeInCache = false)
        {
            var ods = await _odsApiClientProvider.NewResourcesClient();
            var staffsFromApi = await ods.Get<IEnumerable<Staff>>($"enrollment/schools/{schoolId}/staffs");
            return _mapper.Secured(staffsFromApi);
        }
    }
}
