using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.Models
{
    public class StaffIdentificationDocumentsItemv3
    {
        public DateTime? DocumentExpirationDate { get; set; }
        public string DocumentTitle { get; set; }
        public string IdentificationDocumentUseType { get; set; }
        public string IssuerCountryDescriptor { get; set; }
        public string IssuerDocumentIdentificationCode { get; set; }
        public string IssuerName { get; set; }
        public string PersonalInformationVerificationType { get; set; }
    }
}
