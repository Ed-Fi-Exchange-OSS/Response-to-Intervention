using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.Models
{
    public class AssessmentContentStandardv3
    {
        public IEnumerable<AssessmentContentStandardAuthorv3> Authors { get; set; }
        public string BeginDate { get; set; }
        public string EndDate { get; set; }
        public EducationOrganizationReferencev3 MandatingEducationOrganizationReference { get; set; }
        public string PublicationStatusDescriptor { get; set; }
        public string PublicationDate { get; set; }
        public int PublicationYear { get; set; }
        public string Title { get; set; }
        public string Uri { get; set; }
        public string Version { get; set; }
    }
}
