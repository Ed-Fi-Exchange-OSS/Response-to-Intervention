using EdFi.Ods.Api.Client.Models;
using EdFi.RtI.Core.Mapper;
using EdFi.RtI.Core.Models;
using EdFi.RtI.Core.OdsApi;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EdFi.RtI.Core.Providers.Staffs
{
    public class StaffsProviderV3 : IStaffsProvider
    {
        private readonly IOdsApiClientProvider _odsApiClientProvider;
        private readonly IOdsApiSettingsProvider _odsApiSettingsProvider;

        public StaffsProviderV3(IOdsApiClientProvider odsApiClientProvider, IOdsApiSettingsProvider odsApiSettingsProvider)
        {
            _odsApiClientProvider = odsApiClientProvider;
            _odsApiSettingsProvider = odsApiSettingsProvider;
        }

        public async Task<IList<string>> GetStaffClassificationDescriptorsByEmail(string email)
        {
            var staff = await GetStaffMemberByEmail(email);
            var ods = await _odsApiClientProvider.NewResourcesClient();

            // TODO Should this use StaffEducationOrganizationAssignmentAssociation for v3? Are the models the same in both versions?
            var associations = await ods.Get<IList<StaffEducationOrganizationAssignmentAssociation>>("staffEducationOrganizationAssignmentAssociations", new Dictionary<string, string>
            {
                ["staffUniqueId"] = staff.StaffUniqueId,
            });

            return associations
                .Select(association => association.StaffClassificationDescriptor.MapToStaffClassificationDescriptorv2())
                .ToList();
        }

        public async Task<Staff> GetStaffMemberByEmail(string email)
        {
            var edFiVersion = await _odsApiSettingsProvider.GetEdFiVersion();
            var ods = await _odsApiClientProvider.NewResourcesClient();

            var staffs = await ods.Get<IList<Staffv3>>("staffs", new Dictionary<string, string>
            {
                ["limit"] = "100",
            });

            var foundStaff = staffs
                .Where(staff => staff.ElectronicMails != null)
                .Where(staff => staff.ElectronicMails.Any(staffEmail => staffEmail.ElectronicMailAddress.Trim().ToLower().Contains(email.Trim().ToLower())))
                .FirstOrDefault();

            if (foundStaff == null)
                throw new StaffNotFoundException(email);

            return foundStaff.MapToStaff();
        }
    }
}
