using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.Models
{
    public class StaffPersonalIdentificationDocumentv3
    {
        public string IdentificationDocumentUseDescriptor { get; set; }
        public string PersonalInformationVerificationDescriptor { get; set; }
        public string IssuerCountryDescriptor { get; set; }
        public string DocumentExpirationDate { get; set; }
        public string DocumentTitle { get; set; }
        public string IssuerDocumentIdentificationCode { get; set; }
        public string IssuerName { get; set; }
    }
}
