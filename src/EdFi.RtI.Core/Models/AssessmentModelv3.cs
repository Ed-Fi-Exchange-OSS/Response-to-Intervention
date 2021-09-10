using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.Models
{
    public class AssessmentModelv3
    {
        public IEnumerable<AcademicSubjectDescriptorv3> AcademicSubjects { get; set; }
        public bool AdaptiveAssessment { get; set; }
        public IEnumerable<GradeLevelDescriptorv3> AssessedGradeLevels { get; set; }
        public string AssessmentCategoryDescriptor { get; set; }
        public string AssessmentFamily { get; set; }
        public string AssessmentForm { get; set; }
        public string AssessmentIdentifier { get; set; }
        public string AssessmentTitle { get; set; }
        public int AssessmentVersion { get; set; }
        public AssessmentContentStandardv3 ContentStandard { get; set; }
        public EducationOrganizationReferencev3 EducationOrganizationReference { get; set; }
        public string Id { get; set; }
        public IEnumerable<AssessmentIdentificationCodev3> IdentificationCodes { get; set; }
        public IEnumerable<AssessmentLanguagev3> Languages { get; set; }
        public double MaxRawScore { get; set; }
        public string Nomenclature { get; set; }
        public string Namespace { get; set; }
        public IEnumerable<AssessmentPerformanceLevelv3> PerformanceLevels { get; set; }
        public AssessmentPeriodv3 Period { get; set; }
        public IEnumerable<AssessmentPlatformTypev3> PlatformTypes { get; set; }
        public IEnumerable<AssessmentProgramv3> Programs { get; set; }
        public string RevisionDate { get; set; }
        public IEnumerable<AssessmentScorev3> Scores { get; set; }
        public IEnumerable<AssessmentSectionv3> Sections { get; set; }
        public string _etag { get; set; }
    }
}
