using EdFi.Ods.Api.Client.Models;
using EdFi.RtI.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EdFi.RtI.Core.Mapper
{
    public static class SchoolMappers
    {
        public static School MapToSchool (this Schoolv3 a)
        {
            return new School
            {
                Id = a.Id,
                Addresses = a.Addresses.Select(x => x.MapToSchoolAddressesItem()).ToList(),
                AdministrativeFundingControlDescriptor = null,
                CharterApprovalAgencyType = null,
                CharterApprovalSchoolYearTypeReference = null,
                CharterStatusType = null,
                EducationOrganizationCategories = a.EducationOrganizationCategories.Select(x => x.MapToSchoolEducationOrganizationCategoriesItem()).ToList(),
                GradeLevels = a.GradeLevels.Select(x => x.MapToSchoolGradeLevelsItem()).ToList(),
                IdentificationCodes = a.IdentificationCodes.Select(x => x.MapToSchoolIdentificationCodesItem()).ToList(),
                InstitutionTelephones = a.InstitutionTelephones.Select(x => x.MapToSchoolInstitutionTelephonesItem()).ToList(),
                InternationalAddresses = null,
                InternetAccessType = null,
                LocalEducationAgencyReference = a.LocalEducationAgencyReference?.MapToSchoolLocalEducationAgencyReference(),
                MagnetSpecialProgramEmphasisSchoolType = null,
                NameOfInstitution = a.NameOfInstitution,
                OperationalStatusType = null,
                SchoolCategories = a.SchoolCategories.Select(x => x.MapToSchoolSchoolCategoriesItem()).ToList(),
                SchoolId = a.SchoolId,
                ShortNameOfInstitution = a.ShortNameOfInstitution,
                StateOrganizationId = null,
                TitleIPartASchoolDesignationType = null,
                Type = a.Type,
                WebSite = a.WebSite,
                _etag = a._etag
            };
        }

        public static Schoolv3 MapToSchool(this School a)
        {
            return new Schoolv3
            {
                Id = a.Id,
                Addresses = a.Addresses.Select(x => x.MapToSchoolAddressesItem()).ToList(),
                EducationOrganizationCategories = a.EducationOrganizationCategories.Select(x => x.MapToSchoolEducationOrganizationCategoriesItem()).ToList(),
                GradeLevels = a.GradeLevels.Select(x => x.MapToSchoolGradeLevelsItem()).ToList(),
                IdentificationCodes = a.IdentificationCodes.Select(x => x.MapToSchoolIdentificationCodesItem()).ToList(),
                InstitutionTelephones = a.InstitutionTelephones.Select(x => x.MapToSchoolInstitutionTelephonesItem()).ToList(),
                LocalEducationAgencyReference = a.LocalEducationAgencyReference.MapToSchoolLocalEducationAgencyReference(),
                NameOfInstitution = a.NameOfInstitution,
                SchoolCategories = a.SchoolCategories.Select(x => x.MapToSchoolSchoolCategoriesItem()).ToList(),
                SchoolId = a.SchoolId,
                ShortNameOfInstitution = a.ShortNameOfInstitution,
                Type = a.Type,
                WebSite = a.WebSite,
                _etag = a._etag
            };
        }
    }
}
