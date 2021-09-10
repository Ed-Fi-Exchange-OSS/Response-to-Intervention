using EdFi.Ods.Api.Client.Models;
using EdFi.RtI.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.Mapper
{
    public static class StudentInterventionAssociationInterventionEffectivenessMappers
    {
        public static StudentInterventionAssociationInterventionEffectivenessesItem MapToStudentInterventionAssociationInterventionEffectivenessesItem(this StudentInterventionAssociationInterventionEffectivenessv3 a)
        {
            return new StudentInterventionAssociationInterventionEffectivenessesItem
            {
                DiagnosisDescriptor = a.DiagnosisDescriptor?.MapToDiagnosisDescriptorv2(),
                GradeLevelDescriptor = a.GradeLevelDescriptor?.MapToGradeLevelDescriptorv2(),
                ImprovementIndex = a.ImprovementIndex,
                InterventionEffectivenessRatingType = a.InterventionEffectivenessRatingDescriptor?.MapToInterventionEffectivenessRatingDescriptorv2(),
                PopulationServedType = a.PopulationServedDescriptor?.MapToPopulationServedDescriptorv2(),
            };
        }

        public static StudentInterventionAssociationInterventionEffectivenessv3 MapToStudentInterventionAssociationInterventionEffectivenessv3(this StudentInterventionAssociationInterventionEffectivenessesItem a)
        {
            return new StudentInterventionAssociationInterventionEffectivenessv3
            {
                DiagnosisDescriptor = a.DiagnosisDescriptor?.MapToDiagnosisDescriptorv3(),
                GradeLevelDescriptor = a.GradeLevelDescriptor?.MapToGradeLevelDescriptorv3(),
                ImprovementIndex = a.ImprovementIndex ?? 0,
                InterventionEffectivenessRatingDescriptor = a.InterventionEffectivenessRatingType?.MapToInterventionEffectivenessRatingDescriptorv3(),
                PopulationServedDescriptor = a.PopulationServedType?.MapToPopulationServedDescriptorv3(),
            };
        }
    }
}
