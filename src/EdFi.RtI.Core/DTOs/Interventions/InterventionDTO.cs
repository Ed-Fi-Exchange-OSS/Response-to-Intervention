using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.DTOs.Interventions
{
    public class InterventionDTO
    {
        public string _Etag { get; set; }

        public IEnumerable<AppropriateGradeLevelsDTO> AppropriateGradeLevels { get; set; }

        public IEnumerable<AppropriateSexesDTO> AppropriateSexes { get; set; }

        public object BeginDate { get; set; }

        public string ClassType { get; set; }

        public string DeliveryMethodType { get; set; }

        public IEnumerable<DiagnosesDTO> Diagnoses { get; set; }

        public IEnumerable<EducationContentsDTO> EducationContents { get; set; }

        public EducationOrganizationReferenceDTO EducationOrganizationReference { get; set; }

        public object EndDate { get; set; }

        public string Id { get; set; }

        public string IdentificationCode { get; set; }

        public IEnumerable<InterventionPrescriptionsDTO> InterventionPrescriptions { get; set; }

        public IEnumerable<LearningResourceMetadataURIsDTO> LearningResourceMetadataURIs { get; set; }

        public IEnumerable<MeetingTimesDTO> MeetingTimes { get; set; }

        public IEnumerable<PopulationServedsDTO> PopulationServeds { get; set; }

        public IEnumerable<StaffsDTO> Staffs { get; set; }

        public IEnumerable<UrisDTO> Uris { get; set; }
    }

    public class AppropriateGradeLevelsDTO { }

    public class AppropriateSexesDTO { }

    public class DiagnosesDTO { }

    public class EducationContentsDTO { }

    public class EducationOrganizationReferenceDTO
    {
        public int EducationOrganizationId { get; set; }
        public LinkDTO Link { get; set; }
    }

    public class InterventionPrescriptionsDTO { }

    public class LearningResourceMetadataURIsDTO { }

    public class MeetingTimesDTO { }

    public class PopulationServedsDTO { }

    public class StaffsDTO { }

    public class UrisDTO { }

    public class LinkDTO
    { 
        public string Href { get; set; }

        public string Rel { get; set; }

    }
}
