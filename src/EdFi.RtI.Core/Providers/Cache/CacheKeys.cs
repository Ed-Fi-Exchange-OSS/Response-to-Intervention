using EdFi.Ods.Api.Client.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.Providers.Cache
{
    public class CacheKeys
    {
        // Arrays (Client DTO arrays are stored with these keys)
        public static readonly string Assessments = "assessments";
        public static readonly string Interventions = "interventions";
        public static readonly string Categories = "categories";
        public static readonly string Profiles = "profiles";
        public static readonly string Schools = "schools";
        public static readonly string Sections = "sections";
        public static readonly string SectionsByStaff = "sectionsByStaff";
        public static readonly string Staffs = "staffs";
        public static readonly string StaffsBySchool = "staffsBySchool";
        public static readonly string Students = "students";
        public static readonly string StudentsBySection = "studentsBySection";
        public static readonly string AssessmentPeriodDescriptors = "assessmentPeriodDescriptors";
        public static readonly string AssessmentResult = "assessmentResult";
        public static readonly string AssessmentReportingMethodTypes = "assessmentReportingMethodTypes";
        public static readonly string DeliveryMethod = "deliveryMethod";
        public static readonly string InterventionClass = "interventionClass";
        public static readonly string AssessmentFamily = "assessmentFamily";

        // Composite (custom DTOs are made for the objects stored with these keys, so these will typically be joined with another value using the Composite method below)
        public static readonly string AssessmentByIdentifier = "assessmentByIdentifier";
        public static readonly string AssessmentScoringsByStudentUniqueId = "assessmentScoringsByStudentUniqueId";
        public static readonly string AssessmentScoringsBySectionUniqueId = "assessmentScoringsBySectionId";
        
        public static readonly string InterventionByIdentificationCode = "interventionByIdentificationCode";
        public static readonly string InterventionScoringsByStudentUniqueId = "interventionScoringsByStudentUniqueId";
        public static readonly string InterventionScoringsBySectionUniqueId = "interventionScoringsBySectionId";
        
        public static readonly string SchoolsByStaff = "schoolsByStaff";
        public static readonly string StudentByUniqueId = "studentByUniqueId";
        
        public static readonly string StaffIdByEmail = "staffIdByEmail"; // Use like "staffIdByEmail-example@address.com = staffId"
        public static readonly string StaffUniqueIdByEmail = "staffUniqueIdByEmail"; // Use like "staffUniqueIdByEmail-example@address.com = staffUniqueId"

        // Settings
        public static readonly string FeaturesSettings = "featuresSettings";
        public static readonly string OdsApiSettings = "odsApiSettings";
        public static readonly string EdFiVersion = "edFiVersion";
        public static readonly string UserEmailMappings = "userEmailMappings";
        public static readonly string UserRoleAdminMappings = "userRoleAdminMappings";
        public static readonly string UserRoleTeacherMappings = "userRoleTeacherMappings";

        public static class RefreshDates
        {
            // Arrays (Client DTO arrays are stored with these keys)
            public static readonly string Assessments = "assessments.lastRefreshDate";
            public static readonly string Interventions = "interventions.lastRefreshDate";
            public static readonly string Profiles = "profiles.lastRefreshDate";
            public static readonly string ProfilesStaffEmailIdPairs = "profilesStaffEmailIdPairs.lastRefreshDate";
            public static readonly string Schools = "schools.lastRefreshDate";
            public static readonly string Sections = "sections.lastRefreshDate";
            public static readonly string SectionsByStaff = "sectionsByStaff.lastRefreshDate";
            public static readonly string Staffs = "staffs.lastRefreshDate";
            public static readonly string StaffsBySchool = "staffsBySchool.lastRefreshDate";
            public static readonly string Students = "students.lastRefreshDate";
            public static readonly string StudentsBySection = "studentsBySection.lastRefreshDate";

            // Composite (custom DTOs are made for the objects stored with these keys, so these will typically be joined with another value using the Composite method below)
            public static readonly string AssessmentByIdentifier = "assessmentByIdentifier.lastRefreshDate";
            public static readonly string AssessmentScoringsByStudentUniqueId = "assessmentScoringsByStudentUniqueId.lastRefreshDate";
            public static readonly string AssessmentScoringsBySectionUniqueId = "assessmentScoringsBySectionId.lastRefreshDate";
            public static readonly string InterventionByIdentificationCode = "interventionByIdentificationCode.lastRefreshDate";
            public static readonly string InterventionScoringsByStudentUniqueId = "interventionScoringsByStudentUniqueId.lastRefreshDate";
            public static readonly string InterventionScoringsBySectionUniqueId = "interventionScoringsBySectionId.lastRefreshDate";
            public static readonly string SchoolsByStaff = "schoolsByStaff.lastRefreshDate";
            public static readonly string StudentByUniqueId = "studentByUniqueId.lastRefreshDate";
            public static readonly string StaffIdByEmail = "staffIdByEmail.lastRefreshDate";
            public static readonly string StaffUniqueIdByEmail = "staffUniqueIdByEmail.lastRefreshDate";
        }

        public static string Composed(params string[] keys)
        {
            return string.Join('-', keys);
        }
    }
}
