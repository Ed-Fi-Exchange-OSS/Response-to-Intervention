using EdFi.RtI.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.Mapper
{
    public static class StringMappers
    {
        private static readonly string EdFiUriPrefix = "uri://ed-fi.org";
        private static readonly string DescriptorDelimeter = "#";

        public static string MapToAcademicSubjectDescriptorV2(this string str)
        {
            return ExtractDescriptorName(str);
        }

        public static string MapToAcademicSubjectDescriptorV3(this string str)
        {
            return BuildDescriptorV3("AcademicSubjectDescriptor", str);
        }

        public static string MapToAccommodationDescriptorv2(this string str)
        {
            return ExtractDescriptorName(str);
        }

        public static string MapToAccommodationDescriptorv3(this string str)
        {
            return BuildDescriptorV3("AccommodationDescriptor", str);
        }

        public static string MapToAdministrationEnvironmentDescriptorv2(this string str)
        {
            return ExtractDescriptorName(str);
        }

        public static string MapToAdministrationLanguageDescriptorv2(this string str)
        {
            return ExtractDescriptorName(str);
        }

        public static string MapToAdministrationLanguageDescriptorv3(this string str)
        {
            return BuildDescriptorV3("AdministrationLanguageDescriptor", str);
        }

        public static string MapToAdministrationEnvironmentDescriptorv3(this string str)
        {
            return BuildDescriptorV3("AdministrationEnvironmentDescriptor", str);
        }

        public static string MapToAssessmentCategoryDescriptorv2(this string str)
        {
            return ExtractDescriptorName(str);
        }

        public static string MapToAssessmentCategoryDescriptorv3(this string str)
        {
            return BuildDescriptorV3("AssessmentCategoryDescriptor", str);
        }

        public static string MapToAssessmentIdentificationSystemDescriptorv2(this string str)
        {
            return ExtractDescriptorName(str);
        }

        public static string MapToAssessmentIdentificationSystemDescriptorv3(this string str)
        {
            return BuildDescriptorV3("AssessmentIdentificationSystemDescriptor", str);
        }

        public static string MapToAssessmentItemResultDescriptorv2(this string str)
        {
            return ExtractDescriptorName(str);
        }

        public static string MapToAssessmentItemResultDescriptorv3(this string str)
        {
            return BuildDescriptorV3("AssessmentItemResultDescriptor", str);
        }

        public static string MapToAssessmentPeriodDescriptorv2(this string str)
        {
            return ExtractDescriptorName(str);
        }

        public static AssessmentPeriodv3 MapToAssessmentPeriodDescriptorV3(this string str)
        {
            // The Period Descriptors of v2 are abbreviated but in v3 they are complete words
            var v2Mappings = new Dictionary<string, string>
            {
                ["BOY"] = "Beginning of Year",
                ["MOY"] = "Middle of Year",
                ["EOY"] = "End of Year",
            };

            var descriptorValue = v2Mappings.GetValueOrDefault(str, str);

            return new AssessmentPeriodv3
            {
                AssessmentPeriodDescriptor = BuildDescriptorV3("AssessmentPeriodDescriptor", str),
                BeginDate = "2000-01-01",
                EndDate = "2000-01-01",
            };
        }

        public static string MapToDiagnosisDescriptorv2(this string str)
        {
            return ExtractDescriptorName(str);
        }

        public static string MapToDiagnosisDescriptorv3(this string str)
        {
            return BuildDescriptorV3("DiagnosisDescriptor", str);
        }

        public static string MapToEducationalEnvironmentDescriptorv2(this string str)
        {
            return ExtractDescriptorName(str);
        }

        public static string MapToEducationalEnvironmentDescriptorv3(this string str)
        {
            return BuildDescriptorV3("EducationalEnvironmentDescriptor", str);
        }

        public static string MapToEventCircumstanceDescriptorv2(this string str)
        {
            return ExtractDescriptorName(str);
        }

        public static string MapToEventCircumstanceDescriptorv3(this string str)
        {
            return BuildDescriptorV3("EventCircumstanceDescriptor", str);
        }

        public static string MapToGradeLevelDescriptorv3(this string str)
        {
            return BuildDescriptorV3("GradeLevelDescriptor", str);
        }

        public static string MapToGradeLevelDescriptorv2(this string str)
        {
            return ExtractDescriptorName(str);
        }

        public static string MapToInterventionClassTypeV2(this string str)
        {
            return ExtractDescriptorName(str);
        }

        public static string MapToInterventionClassDescriptorTypeV3(this string str)
        {
            return BuildDescriptorV3("InterventionClassDescriptor", str);
        }

        public static string MapToInterventionEffectivenessRatingDescriptorv2(this string str)
        {
            return ExtractDescriptorName(str);
        }

        public static string MapToInterventionEffectivenessRatingDescriptorv3(this string str)
        {
            return BuildDescriptorV3("InterventionEffectivenessRatingDescriptor", str);
        }

        public static string MapToDeliveryMethodTypeV2(this string str)
        {
            return ExtractDescriptorName(str);
        }

        public static string MapToDeliveryMethodDescriptorV3(this string str)
        {
            return BuildDescriptorV3("DeliveryMethodDescriptor", str);
        }

        public static string MapToAssessmentReportingMethodDescriptorv2(this string str)
        {
            return ExtractDescriptorName(str);
        }

        public static string MapToAssessmentReportingMethodDescriptorv3(this string str)
        {
            return BuildDescriptorV3("AssessmentReportingMethodDescriptor", str);
        }

        public static string MapToPerformanceLevelDescriptorv2(this string str)
        {
            return ExtractDescriptorName(str);
        }

        public static string MapToPerformanceLevelDescriptorv3(this string str)
        {
            return BuildDescriptorV3("PerformanceLevelDescriptor", str);
        }

        public static string MapToPopulationServedDescriptorv2(this string str)
        {
            return ExtractDescriptorName(str);
        }

        public static string MapToPopulationServedDescriptorv3(this string str)
        {
            return BuildDescriptorV3("PopulationServedDescriptor", str);
        }

        public static string MapToPublicationStatusDescriptorv2(this string str)
        {
            return ExtractDescriptorName(str);
        }

        public static string MapToPublicationStatusDescriptorv3(this string str)
        {
            return BuildDescriptorV3("PublicationStatusDescriptor", str);
        }

        public static string MapToReasonNotTestedDescriptorv2(this string str)
        {
            return ExtractDescriptorName(str);
        }

        public static string MapToReasonNotTestedDescriptorv3(this string str)
        {
            return BuildDescriptorV3("ReasonNotTestedDescriptor", str);
        }

        public static string MapToResponseIndicatorDescriptorv2(this string str)
        {
            return ExtractDescriptorName(str);
        }

        public static string MapToResponseIndicatorDescriptorv3(this string str)
        {
            return BuildDescriptorV3("ResponseIndicatorDescriptor", str);
        }

        public static string MapToResultDatatypeTypeDescriptorv2(this string str)
        {
            return ExtractDescriptorName(str);
        }

        public static string MapToResultDatatypeTypeDescriptorv3(this string str)
        {
            return BuildDescriptorV3("ResultDatatypeTypeDescriptor", str);
        }

        public static string MapToRetestIndicatorDescriptorv2(this string str)
        {
            return ExtractDescriptorName(str);
        }

        public static string MapToRetestIndicatorDescriptorv3(this string str)
        {
            return BuildDescriptorV3("RetestIndicatorDescriptor", str);
        }

        public static string MapToTermDescriptorv2(this string str)
        {
            return ExtractDescriptorName(str);
        }

        public static string MapToTermDescriptorv3(this string str)
        {
            return BuildDescriptorV3("TermDescriptor", str);
        }

        public static string MapToStaffClassificationDescriptorv2(this string str)
        {
            return ExtractDescriptorName(str);
        }

        public static string MapToStaffClassificationDescriptorv3(this string str)
        {
            return BuildDescriptorV3("StaffClassificationDescriptor", str);
        }

        private static string BuildDescriptorV3(string descriptorCategory, string descriptorName)
        {
            if (string.IsNullOrWhiteSpace(descriptorName))
                return descriptorName;

            if (ContainsEdFiUriPrefix(descriptorName))
                return descriptorName;

            return $"{EdFiUriPrefix}/{descriptorCategory}{DescriptorDelimeter}{descriptorName}";
        }

        public static string MapToWhenAssessedGradeLevelDescriptorv2(this string str)
        {
            return ExtractDescriptorName(str);
        }

        public static string MapToWhenAssessedGradeLevelDescriptorv3(this string str)
        {
            return BuildDescriptorV3("WhenAssessedGradeLevelDescriptor", str);
        }

        private static bool ContainsEdFiUriPrefix(string str)
        {
            return str.Contains(EdFiUriPrefix);
        }

        private static string ExtractDescriptorName(string str)
        {
            return str.Contains(DescriptorDelimeter)
                ? str.Split(DescriptorDelimeter)[1]
                : str;
        }
    }
}
