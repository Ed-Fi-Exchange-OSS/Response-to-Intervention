using EdFi.Ods.Api.Client.Models;
using EdFi.RtI.Core.DomainServiceProvider;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EdFi.RtI.Core.Providers.Staffs
{
    public interface IStaffsProvider : IDomainService
    {
        Task<Staff> GetStaffMemberByEmail(string email);
        Task<IList<string>> GetStaffClassificationDescriptorsByEmail(string email);
    }
}
