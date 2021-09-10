using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.Models
{
    public class Staffv3
    {
        public string Id { get; set; }
        public string StaffUniqueId { get; set; }
        public StaffPersonReferencev3 PersonReference { get; set; }
        public IList<StaffAddressesItemv3> Addresses { get; set; }//
        public IList<StaffAncestryEthnicOriginv3> AncestryEthnicOrigins { get; set; }
        public DateTime? BirthDate { get; set; }
        public string CitizenshipStatusType { get; set; }
        public IList<object> Credentials { get; set; } //Check with v2
        public IList<StaffElectronicMailsItemv3> ElectronicMails { get; set; }//
        public string FirstName { get; set; }
        public string GenerationCodeSuffix { get; set; }
        public string HighestCompletedLevelOfEducationDescriptor { get; set; }
        public bool? HighlyQualifiedTeacher { get; set; }
        public bool HispanicLatinoEthnicity { get; set; }
        public IList<StaffIdentificationCodesItemv3> IdentificationCodes { get; set; }//
        public IList<StaffInternationalAddressesItemv3> InternationalAddresses { get; set; }//
        public IList<StaffIdentificationDocumentsItemv3> IdentificationDocuments { get; set; }//
        public IList<StaffLanguagesItemv3> Languages { get; set; }//
        public string LastSurname { get; set; }
        public string LoginId { get; set; }
        public string MiddleName { get; set; }
        public string MaidenName { get; set; }
        public string OldEthnicityType { get; set; }
        public IList<StaffOtherNamesItemv3> OtherNames { get; set; }//
        public IList<StaffPersonalIdentificationDocumentv3> PersonalIdentificationDocuments { get; set; }
        public string PersonalTitlePrefix { get; set; }
        public IList<StaffRacesItemv3> Races { get; set; }//
        public IList<StaffRecognitionv3> Recognitions { get; set; }
        public string SexType { get; set; }
        public IList<StaffTelephonesItemv3> Telephones { get; set; }//
        public IList<StaffTribalAffiliationv3> TribalAffiliations { get; set; }
        public IList<StaffVisasItemv3> Visas { get; set; } //
        public double? YearsOfPriorProfessionalExperience { get; set; }
        public double? YearsOfPriorTeachingExperience { get; set; }
        public string _etag { get; set; }
        public StaffExtensionsv3 _ext { get; set; }
    }
}
