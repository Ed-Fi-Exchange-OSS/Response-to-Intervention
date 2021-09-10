using EdFi.Ods.Api.Client.Models;
using EdFi.RtI.Core.DomainServiceProvider;
using EdFi.RtI.Core.DTOs.Composed;
using EdFi.RtI.Core.DTOs.StudentSectionAssociation;
using EdFi.RtI.Core.RequestBodies;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EdFi.RtI.Core.Services.ScoringAssessments
{
    public interface IScoringAssessmentsService : IDomainService
    {
        Task<ScoringAssessmentDTO> CreateScoring(ScoringAssessmentDTO scoring);
        Task<StudentAssessment> CreateAssociation(ScoringAssessmentPostBody body);
        Task DeleteStudentAssessment(string studentAssessmentId, string uniqueSectionCode, string identifier);
        Task DeleteStudentAssessmentAll(string uniqueSectionCode, string assessmentId, string date);
        Task<IEnumerable<ScoringAssessmentDTO>> Search(ScoringAssessmentSearchParams searchParams);
        Task UpdateScorings(IEnumerable<ScoringAssessmentDTO> scorings);
    }
}
