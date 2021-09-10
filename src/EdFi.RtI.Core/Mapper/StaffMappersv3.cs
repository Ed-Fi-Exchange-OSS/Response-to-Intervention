using EdFi.Ods.Api.Client.Models;
using EdFi.RtI.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EdFi.RtI.Core.Mapper
{
    public static class StaffMappersv3
    {
        public static Staff MapToStaff(this Staffv3 a)
        {
            return new Staff
            {
                Addresses = null,
                BirthDate = a.BirthDate,
                CitizenshipStatusType = a.CitizenshipStatusType,
                Credentials = null,
                ElectronicMails = a.ElectronicMails?.Select(a => a?.MapToStaffElectronicMailsItemV2()).ToList(),
                FirstName = a.FirstName,
                GenerationCodeSuffix = a.GenerationCodeSuffix,
                HighestCompletedLevelOfEducationDescriptor = a.HighestCompletedLevelOfEducationDescriptor,
                HighlyQualifiedTeacher = a.HighlyQualifiedTeacher,
                HispanicLatinoEthnicity = a.HispanicLatinoEthnicity,
                Id = a.Id,
                IdentificationCodes = null,
                IdentificationDocuments = null,
                InternationalAddresses = null,
                Languages = null,
                LastSurname = a.LastSurname,
                LoginId = a.LoginId,
                MaidenName = a.MaidenName,
                MiddleName = a.MiddleName,
                OldEthnicityType = a.OldEthnicityType,
                OtherNames = null,
                PersonalTitlePrefix = a.PersonalTitlePrefix,
                Races = null,
                SexType = a.SexType,
                StaffUniqueId = a.StaffUniqueId,
                Telephones = null,
                Visas = null,
                YearsOfPriorProfessionalExperience = a.YearsOfPriorProfessionalExperience,
                YearsOfPriorTeachingExperience = a.YearsOfPriorTeachingExperience,
                _etag = a._etag,
            };
        }

        public static Staffv3 MapToStaff(this Staff a)
        {
            return new Staffv3
            {
                Addresses = null,
                BirthDate = a.BirthDate,
                CitizenshipStatusType = a.CitizenshipStatusType,
                Credentials = null,
                ElectronicMails = null,
                FirstName = a.FirstName,
                GenerationCodeSuffix = a.GenerationCodeSuffix,
                HighestCompletedLevelOfEducationDescriptor = a.HighestCompletedLevelOfEducationDescriptor,
                HighlyQualifiedTeacher = a.HighlyQualifiedTeacher,
                HispanicLatinoEthnicity = a.HispanicLatinoEthnicity,
                Id = a.Id,
                IdentificationCodes = null,
                IdentificationDocuments = null,
                InternationalAddresses = null,
                Languages = null,
                LastSurname = a.LastSurname,
                LoginId = a.LoginId,
                MaidenName = a.MaidenName,
                MiddleName = a.MiddleName,
                OldEthnicityType = a.OldEthnicityType,
                OtherNames = null,
                PersonalTitlePrefix = a.PersonalTitlePrefix,
                Races = null,
                SexType = a.SexType,
                StaffUniqueId = a.StaffUniqueId,
                Telephones = null,
                Visas = null,
                YearsOfPriorProfessionalExperience = a.YearsOfPriorProfessionalExperience,
                YearsOfPriorTeachingExperience = a.YearsOfPriorTeachingExperience,
                _etag = a._etag,
                AncestryEthnicOrigins = null,
                PersonalIdentificationDocuments = null,
                PersonReference = null,
                Recognitions = null,
                TribalAffiliations = null,
                _ext = null
            };
        }
    }
}
