using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.Models
{
    public class StaffRecognitionv3
    {
        public string RecognitionTypeDescriptor { get; set; }
        public string AchievementCategoryDescriptor { get; set; }
        public string AchievementCategorySystem { get; set; }
        public string AchievementTitle { get; set; }
        public string Criteria { get; set; }
        public string CriteriaURL { get; set; }
        public string EvidenceStatement { get; set; }
        public string ImageURL { get; set; }
        public string IssuerName { get; set; }
        public string IssuerOriginURL { get; set; }
        public string RecognitionAwardDate { get; set; }
        public string RecognitionAwardExpiresDate { get; set; }
        public string RecognitionDescription { get; set; }
    }
}
