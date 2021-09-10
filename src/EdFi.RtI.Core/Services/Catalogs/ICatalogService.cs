using EdFi.Ods.Api.Client.Models;
using EdFi.RtI.Core.DomainServiceProvider;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EdFi.RtI.Core.Services.Catalogs
{
    public interface ICatalogService : IDomainService
    {
        Task<School> GetSchoolBySchoolId(int schoolId, bool getFromCache = true);
        Task<IEnumerable<School>> GetSchoolsAll(bool getFromCache = true, bool storeInCache = true);
        Task<IEnumerable<School>> GetSchoolsByStaff(int staffUniqueId);
        Task<IEnumerable<Section>> GetSectionsAll(bool getFromCache = true, bool storeInCache = true);
        Task<IEnumerable<Section>> GetSectionsByStaff(string staffUniqueId, bool getFromCache = true, bool storeInCache = true);
        Task<IEnumerable<Staff>> GetStaffsAll(bool getFromCache = true, bool storeInCache = true);
        Task<IEnumerable<Staff>> GetStaffsBySchool(string schoolId, bool getFromCache = true, bool storeInCache = false);
    }
}
