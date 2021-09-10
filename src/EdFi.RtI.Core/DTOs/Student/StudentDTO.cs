using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.DTOs.Student
{
    public class StudentDTO
    {
        public string _Etag { get; set; }
        public IEnumerable<AddressesDTO> Addresses { get; set; }
        public string BirthCity { get; set; }
        public string BirthCountryDescriptor { get; set; }
        public DateTime BirthDate { get; set; }
        public string BirthInternationalProvince { get; set; }
        public string BirthStateAbbreviationType { get; set; }
        public IEnumerable<CharacteristicsDTO> Characteristics { get; set; }
        public string CitizenshipStatusType { get; set; }
        public IEnumerable<CohortYearsDTO> CohortYears { get; set; }
        public string DateEnteredUS { get; set; }
        public IEnumerable<DisabilitiesDTO> Disabilities { get; set; }
        public string DisplacementStatus { get; set; }
        public string EconomicDisadvantaged { get; set; }
        public IEnumerable<ElectronicMailsDTO> ElectronicMails { get; set; }
        public string FirstName { get; set; }
        public string GenerationCodeSuffix { get; set; }
        public bool HispanicLatinoEthnicity { get; set; }
        public string Id { get; set; }
        public IEnumerable<IdentificationCodesDTO> IdentificationCodes { get; set; }
        public IEnumerable<IdentificationDocumentsDTO> IdentificationDocuments { get; set; }
        public IEnumerable<IndicatorsDTO> Indicators { get; set; }
        public IEnumerable<InternationalAddressesDTO> InternationalAddresses { get; set; }
        public IEnumerable<LanguagesDTO> Languages { get; set; }
        public string LastSurname { get; set; }
        public string LearningStyle { get; set; }
        public string LimitedEnglishProficiencyDescriptor { get; set; }
        public string LoginId { get; set; }
        public string MaidenName { get; set; }
        public string MiddleName { get; set; }
        public string MultipleBirthStatus { get; set; }
        public string OldEthnicityType { get; set; }
        public IEnumerable<OtherNamesDTO> OtherNames { get; set; }
        public string PersonalTitlePrefix { get; set; }
        public string ProfileThumbnail { get; set; }
        public IEnumerable<ProgramParticipationsDTO> ProgramParticipations { get; set; }
        public IEnumerable<RacesDTO> Races { get; set; }
        public string SchoolFoodServicesEligibilityDescriptor { get; set; }
        public string SexType { get; set; }
        public string StudentUniqueId { get; set; }
        public IEnumerable<TelephonesDTO> Telephones { get; set; }
        public IEnumerable<VisasDTO> Visas { get; set; }
        public string FullName { get; set; }
    }

    public class AddressesDTO { }

    public class CharacteristicsDTO { }

    public class CohortYearsDTO { }

    public class DisabilitiesDTO { }

    public class ElectronicMailsDTO { }

    public class IdentificationCodesDTO {
        public string AssigningOrganizationIdentificationCode { get; set; }
        public string IdentificationCode { get; set; }
        public string StudentIdentificationSystemDescriptor { get; set; }
    }

    public class IdentificationDocumentsDTO { }

    public class IndicatorsDTO { }

    public class InternationalAddressesDTO { }

    public class LanguagesDTO { }

    public class OtherNamesDTO { } 

    public class ProgramParticipationsDTO { }

    public class RacesDTO { }

    public class TelephonesDTO { }

    public class VisasDTO { } 
}
