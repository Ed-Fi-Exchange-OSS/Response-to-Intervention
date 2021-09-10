using EdFi.Ods.Api.Client.Models;
using EdFi.RtI.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.Mapper
{
    public static class LinkReferenceMappers
    {
        public static InterventionEducationOrganizationReferenceLink MapToInterventionEducationOrganizationReferenceLink(this LinkReferencev3 a)
        {
            return new InterventionEducationOrganizationReferenceLink
            {
                Href = a.Href,
                Rel = a.Rel,
            };
        }

        public static InterventionEducationContentsItemEducationContentReferenceLink MapToInterventionEducationContentsItemEducationContentReferenceLink(this LinkReferencev3 a)
        {
            return new InterventionEducationContentsItemEducationContentReferenceLink
            {
                Href = a.Href,
                Rel = a.Rel,
            };
        }

        public static InterventionInterventionPrescriptionsItemInterventionPrescriptionReferenceLink MapToInterventionInterventionPrescriptionsItemInterventionPrescriptionReferenceLink(this LinkReferencev3 a)
        {
            return new InterventionInterventionPrescriptionsItemInterventionPrescriptionReferenceLink
            {
                Href = a.Href,
                Rel = a.Rel,
            };
        }

        public static LinkReferencev3 MapToLinkReferenvev3(this InterventionEducationOrganizationReferenceLink a)
        {
            return new LinkReferencev3
            {
                Href = a.Href,
                Rel = a.Rel,
            };
        }

        public static LinkReferencev3 MapToLinkReferencev3(this InterventionEducationContentsItemEducationContentReferenceLink a)
        {
            return new LinkReferencev3
            {
                Href = a.Href,
                Rel = a.Rel,
            };
        }

        public static LinkReferencev3 MapToLinkReferencev3(this InterventionInterventionPrescriptionsItemInterventionPrescriptionReferenceLink a)
        {
            return new LinkReferencev3
            {
                Href = a.Href,
                Rel = a.Rel,
            };
        }

        public static AssessmentContentStandardMandatingEducationOrganizationReferenceLink MapToAssessmentContentStandardMandatingEducationOrganizationReferenceLink(this LinkReferencev3 a)
        {
            return new AssessmentContentStandardMandatingEducationOrganizationReferenceLink
            {
                Href = a.Href,
                Rel = a.Rel,
            };
        }

        public static LinkReferencev3 MapToLinkReferencev3(this AssessmentContentStandardMandatingEducationOrganizationReferenceLink a)
        {
            return new LinkReferencev3
            {
                Href = a.Href,
                Rel = a.Rel,
            };
        }

        public static AssessmentEducationOrganizationReferenceLink MapToAssessmentEducationOrganizationReferenceLink(this LinkReferencev3 a)
        {
            return new AssessmentEducationOrganizationReferenceLink
            {
                Href = a.Href,
                Rel = a.Rel,
            };
        }

        public static LinkReferencev3 MapToLinkReferencev3(this AssessmentEducationOrganizationReferenceLink a)
        {
            return new LinkReferencev3
            {
                Href = a.Href,
                Rel = a.Rel,
            };
        }

        public static AssessmentProgramsItemProgramReferenceLink MapToAssessmentProgramsItemProgramReferenceLink(this LinkReferencev3 a)
        {
            return new AssessmentProgramsItemProgramReferenceLink
            {
                Href = a.Href,
                Rel = a.Rel,
            };
        }

        public static LinkReferencev3 MapToLinkReferencev3(this AssessmentProgramsItemProgramReferenceLink a)
        {
            return new LinkReferencev3
            {
                Href = a.Href,
                Rel = a.Rel,
            };
        }

        public static AssessmentSectionsItemSectionReferenceLink MapToAssessmentSectionsItemSectionReferenceLink(this LinkReferencev3 a)
        {
            return new AssessmentSectionsItemSectionReferenceLink
            {
                Href = a.Href,
                Rel = a.Rel,
            };
        }

        public static LinkReferencev3 MapToLinkReferencev3(this AssessmentSectionsItemSectionReferenceLink a)
        {
            return new LinkReferencev3
            {
                Href = a.Href,
                Rel = a.Rel,
            };
        }

        public static StudentAssessmentAssessmentReferenceLink MapToStudentAssessmentAssessmentReferenceLink(this LinkReferencev3 a)
        {
            return new StudentAssessmentAssessmentReferenceLink
            {
                Href = a.Href,
                Rel = a.Rel,
            };
        }

        public static LinkReferencev3 MapToLinkReferencev3(this StudentAssessmentAssessmentReferenceLink a)
        {
            return new LinkReferencev3
            {
                Href = a.Href,
                Rel = a.Rel,
            };
        }

        public static StudentAssessmentItemsItemAssessmentItemReferenceLink MapToStudentAssessmentItemsItemAssessmentItemReferenceLink(this LinkReferencev3 a)
        {
            return new StudentAssessmentItemsItemAssessmentItemReferenceLink
            {
                Href = a.Href,
                Rel = a.Rel,
            };
        }

        public static LinkReferencev3 MapToLinkReferencev3(this StudentAssessmentItemsItemAssessmentItemReferenceLink a)
        {
            return new LinkReferencev3
            {
                Href = a.Href,
                Rel = a.Rel,
            };
        }

        public static StudentAssessmentStudentObjectiveAssessmentsItemObjectiveAssessmentReferenceLink MapToStudentAssessmentStudentObjectiveAssessmentsItemObjectiveAssessmentReferenceLink(this LinkReferencev3 a)
        {
            return new StudentAssessmentStudentObjectiveAssessmentsItemObjectiveAssessmentReferenceLink
            {
                Href = a.Href,
                Rel = a.Rel,
            };
        }

        public static LinkReferencev3 MapToLinkReferencev3(this StudentAssessmentStudentObjectiveAssessmentsItemObjectiveAssessmentReferenceLink a)
        {
            return new LinkReferencev3
            {
                Href = a.Href,
                Rel = a.Rel,
            };
        }

        public static StudentInterventionAssociationCohortReferenceLink MapToStudentInterventionAssociationCohortReferenceLink(this LinkReferencev3 a)
        {
            return new StudentInterventionAssociationCohortReferenceLink
            {
                Href = a.Href,
                Rel = a.Rel,
            };
        }

        public static LinkReferencev3 MapToLinkReferencev3(this StudentInterventionAssociationCohortReferenceLink a)
        {
            return new LinkReferencev3
            {
                Href = a.Href,
                Rel = a.Rel,
            };
        }

        public static StudentInterventionAssociationInterventionReferenceLink MapToStudentInterventionAssociationInterventionReferenceLink(this LinkReferencev3 a)
        {
            return new StudentInterventionAssociationInterventionReferenceLink
            {
                Href = a.Href,
                Rel = a.Rel,
            };
        }

        public static LinkReferencev3 MapToLinkReferencev3(this StudentInterventionAssociationStudentReferenceLink a)
        {
            return new LinkReferencev3
            {
                Href = a.Href,
                Rel = a.Rel,
            };
        }

        public static StudentInterventionAssociationStudentReferenceLink MapToStudentInterventionAssociationStudentReferenceLink(this LinkReferencev3 a)
        {
            return new StudentInterventionAssociationStudentReferenceLink
            {
                Href = a.Href,
                Rel = a.Rel,
            };
        }

        public static LinkReferencev3 MapToLinkReferencev3(this StudentInterventionAssociationInterventionReferenceLink a)
        {
            return new LinkReferencev3
            {
                Href = a.Href,
                Rel = a.Rel,
            };
        }

        public static SectionClassPeriodReferenceLink MapToSectionClassPeriodReferenceLink(this LinkReferencev3 a)
        {
            return new SectionClassPeriodReferenceLink
            {
                Href = a.Href,
                Rel = a.Rel,
            };
        }

        public static LinkReferencev3 MapToLinkReferencev3(this SectionClassPeriodReferenceLink a)
        {
            return new LinkReferencev3
            {
                Href = a.Href,
                Rel = a.Rel,
            };
        }
    }
}
