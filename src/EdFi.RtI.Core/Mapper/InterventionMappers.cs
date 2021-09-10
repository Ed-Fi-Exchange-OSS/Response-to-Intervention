using EdFi.Ods.Api.Client.Models;
using EdFi.RtI.Core.DTOs.Interventions;
using EdFi.RtI.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EdFi.RtI.Core.Mapper
{
    public static class InterventionMappers
    {
        public static Intervention MapToInterventionV2(this InterventionModelv3 a)
        {
            return new Intervention
            {
                AppropriateGradeLevels = a.AppropriateGradeLevels?.Select(b => b.MapToInterventionAppropriateGradeLevelsItemV2()).ToList(),
                AppropriateSexes = a.AppropriateSexes?.Select(b => b.MapToInterventionAppropriateSexesItemV2()).ToList(),
                BeginDate = !string.IsNullOrWhiteSpace(a.BeginDate) ? DateTime.Parse(a.BeginDate) : new DateTime(),
                ClassType = a.InterventionClassDescriptor?.MapToInterventionClassTypeV2(),
                DeliveryMethodType = a.DeliveryMethodDescriptor?.MapToDeliveryMethodTypeV2(),
                Diagnoses = a.Diagnoses?.Select(b => b.MapToInterventionDiagnosesItemV2()).ToList(),
                EducationContents = a.EducationContents?.Select(b => b.MapToInterventionEducationContentsItemV2()).ToList(),
                EducationOrganizationReference = a.EducationOrganizationReference?.MapToInterventionEducationOrganizationReference(),
                EndDate = !string.IsNullOrWhiteSpace(a.EndDate) ? DateTime.Parse(a.EndDate) : new DateTime(),
                Id = a.Id,
                IdentificationCode = a.InterventionIdentificationCode,
                InterventionPrescriptions = a.InterventionPrescriptions?.Select(b => b.MapToInterventionInterventionPrescriptionsItem()).ToList(),
                LearningResourceMetadataURIs = a.LearningResourceMetadataURIs?.Select(b => b.MapToInterventionLearningResourceMetadataURIsItem()).ToList(),
                MeetingTimes = a.MeetingTimes?.Select(b => b.MapToInterventionMeetingTimesItem()).ToList(),
                PopulationServeds = a.PopulationServeds?.Select(b => b.MapToInterventionPopulationServedsItem()).ToList(),
                Staffs = a.Staffs?.Select(b => b.MapToInterventionStaffsItem()).ToList(),
                Uris = a.Uris?.Select(b => b.MapToInterventionUrisItem()).ToList(),
                _etag = a._etag,
            };
        }

        public static InterventionModelv3 MapToInterventionModelv3(this Intervention a)
        {
            return new InterventionModelv3
            {
                AppropriateGradeLevels = a.AppropriateGradeLevels?.Select(b => b.MapToGradeLevelDescriptorV3()),
                AppropriateSexes = a.AppropriateSexes?.Select(b => b.MapToSexDescriptorV3()),
                BeginDate = a.BeginDate.ToString("yyyy-MM-dd"),
                EndDate = a.EndDate?.ToString("yyyy-MM-dd"),
                DeliveryMethodDescriptor = a.DeliveryMethodType?.MapToDeliveryMethodDescriptorV3(),
                Diagnoses = a.Diagnoses?.Select(b => b.MapToDiagnosisDescriptorV3()),
                EducationContents = a.EducationContents?.Select(b => b.MapToEducationContentV3()),
                EducationOrganizationReference = a.EducationOrganizationReference?.MapToEducationOrganizationReferencev3(),
                Id = a.Id,
                InterventionClassDescriptor = a.ClassType?.MapToInterventionClassDescriptorTypeV3(),
                InterventionIdentificationCode = a.IdentificationCode,
                InterventionPrescriptions = a.InterventionPrescriptions?.Select(b => b.MapToInterventionPrescriptionv3()),
                LearningResourceMetadataURIs = a.LearningResourceMetadataURIs?.Select(b => b.MapToLearningResourceMetadataURIv3()),
                //MaxDosage = -1, // TODO Check
                //MinDosage = -1, // TODO Check
                MeetingTimes = a.MeetingTimes?.Select(b => b.MapToMeetingTimev3()),
                PopulationServeds = a.PopulationServeds?.Select(b => b.MapToPopulationServedDescriptorv3()),
                Staffs = a.Staffs?.Select(b => b.MapToStaffItemv3()),
                Uris = a.Uris?.Select(b => b.MapToUriv3()),
                _etag = a._etag,
            };
        }

        public static InterventionDTO MapToDto(this Intervention a)
        {
            // TODO Map missing fields

            return new InterventionDTO
            {
                //AppropriateGradeLevels = a.AppropriateGradeLevels,
                //AppropriateSexes = a.AppropriateSexes,
                BeginDate = a.BeginDate,
                ClassType = a.ClassType,
                DeliveryMethodType = a.DeliveryMethodType,
                //Diagnoses = a.Diagnoses,
                //EducationContents = a.EducationContents,
                //EducationOrganizationReference = a.EducationOrganizationReference,
                EndDate = a.EndDate,
                Id = a.Id,
                IdentificationCode = a.IdentificationCode,
                //InterventionPrescriptions = a.InterventionPrescriptions,
                //LearningResourceMetadataURIs = a.LearningResourceMetadataURIs,
                //MeetingTimes = a.MeetingTimes,
                //PopulationServeds = a.PopulationServeds,
                //Staffs = a.Staffs,
                //Uris = a.Uris,
                _Etag = a._etag,
            };
        }

        public static InterventionDTO MapToDto(this InterventionModelv3 a)
        {
            // TODO Map missing fields

            return new InterventionDTO
            {
                //AppropriateGradeLevels = a.AppropriateGradeLevels,
                //AppropriateSexes = a.AppropriateSexes,
                BeginDate = a.BeginDate,
                EndDate = a.EndDate,
                ClassType = a.InterventionClassDescriptor?.MapToInterventionClassTypeV2(),
                DeliveryMethodType = a.DeliveryMethodDescriptor?.MapToDeliveryMethodTypeV2(),
                //Diagnoses = a.Diagnoses,
                //EducationContents = a.EducationContents,
                //EducationOrganizationReference = a.EducationOrganizationReference,
                //EndDate = a.BeginDate, // TODO Check if v3 has EndDate
                Id = a.Id,
                IdentificationCode = a.InterventionIdentificationCode,
                //InterventionPrescriptions = a.InterventionPrescriptions,
                //LearningResourceMetadataURIs = a.LearningResourceMetadataURIs,
                //MeetingTimes = a.MeetingTimes,
                //PopulationServeds = a.PopulationServeds,
                //Staffs = a.Staffs,
                //Uris = a.Uris,
                _Etag = a._etag,
            };
        }
    }
}
