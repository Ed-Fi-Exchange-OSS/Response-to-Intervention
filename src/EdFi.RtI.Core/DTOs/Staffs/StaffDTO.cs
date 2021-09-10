using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.DTOs.Staffs
{
    public class StaffDTO
    {
        public IEnumerable<AddressDTO> Addresses { get; set; }
        public string BirthDate { get; set; }
        public object CitizenshipStatusType { get; set; }
        public IEnumerable<CredentialDTO> Credentials { get; set; }
        public IEnumerable<ElectronicMailDTO> ElectronicMails { get; set; }
        public string FirstName { get; set; }
        public object GenerationCodeSuffix { get; set; }
        public object HighestCompletedLevelOfEducationDescriptor { get; set; }
        public object HighlyQualifiedTeacher { get; set; }
        public bool HispanicLatinoEthnicity { get; set; }
        public string Id { get; set; }
        public IEnumerable<IdentificationCodeDTO> IdentificationCodes { get; set; }
        public IEnumerable<IdentificationDocumentDTO> IdentificationDocuments { get; set; }
        public IEnumerable<InternationalAddressDTO> InternationalAddresses { get; set; }
        public IEnumerable<LanguageDTO> Languages { get; set; }
        public string LastSurname { get; set; }
        public object LoginId { get; set; }
        public object MaidenName { get; set; }
        public string MiddleName { get; set; }
        public string OldEthnicityType { get; set; }
        public IEnumerable<object> OtherNames { get; set; }
        public string PersonalTitlePrefix { get; set; }
        public IEnumerable<RaceDTO> Races { get; set; }
        public string SexType { get; set; }
        public string StaffUniqueId { get; set; }
        public IEnumerable<TelephoneDTO> Telephones { get; set; }
        public IEnumerable<VisaDTO> Visas { get; set; }
        public double? YearsOfPriorProfessionalExperience { get; set; }
        public double? YearsOfPriorTeachingExperience { get; set; }
        public string _etag { get; set; }

        // Calculated fields
        public string FullName { get; set; }
    }

    public class VisaDTO
    {

    }

    public class TelephoneDTO
    {

    }

    public class RaceDTO
    {
        public string RaceType { get; set; }
    }

    public class LanguageDTO
    {

    }

    public class InternationalAddressDTO
    {

    }

    public class IdentificationDocumentDTO
    {
        public object DocumentExpirationDate { get; set; }
        public object DocumentTitle { get; set; }
        public string IdentificationDocumentUseType { get; set; }
        public object IssuerCountryDescriptor { get; set; }
        public object IssuerDocumentIdentificationCode { get; set; }
        public object IssuerName { get; set; }
        public string PersonalInformationVerificationType { get; set; }
    }

    public class IdentificationCodeDTO
    {
        public object AssigningOrganizationIdentificationCode { get; set; }
        public int IdentificationCode { get; set; }
        public string StaffIdentificationSystemDescriptor { get; set; }
    }

    public class ElectronicMailDTO
    {
        public string ElectronicMailAddress { get; set; }
        public string ElectronicMailType { get; set; }
        public object PrimaryEmailAddressIndicator { get; set; }
    }

    public class CredentialDTO
    {

    }

    public class AddressDTO
    {

    }
}
