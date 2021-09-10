using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.Models
{
    public class InterventionModelv3
    {
        public IEnumerable<GradeLevelDescriptorv3> AppropriateGradeLevels { get; set; }
        public IEnumerable<SexDescriptorv3> AppropriateSexes { get; set; }
        public string BeginDate { get; set; }
        public string DeliveryMethodDescriptor { get; set; }
        public IEnumerable<DiagnosisDescriptorv3> Diagnoses { get; set; }
        public IEnumerable<EducationContentv3> EducationContents { get; set; }
        public EducationOrganizationReferencev3 EducationOrganizationReference { get; set; }
        public string EndDate { get; set; }
        public string Id { get; set; }
        public string InterventionClassDescriptor { get; set; }
        public string InterventionIdentificationCode { get; set; }
        public IEnumerable<InterventionPrescriptionv3> InterventionPrescriptions { get; set; }
        public IEnumerable<LearningResourceMetadataURIv3> LearningResourceMetadataURIs { get; set; }
        public int MaxDosage { get; set; }
        public IEnumerable<MeetingTimev3> MeetingTimes { get; set; }
        public int MinDosage { get; set; }
        public IEnumerable<PopulationServedDescriptorv3> PopulationServeds { get; set; }
        public IEnumerable<StaffItemv3> Staffs { get; set; }
        public IEnumerable<Uriv3> Uris { get; set; }
        public string _etag { get; set; }
    }
}
