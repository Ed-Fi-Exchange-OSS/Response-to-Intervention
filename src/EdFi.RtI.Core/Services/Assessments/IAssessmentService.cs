using EdFi.Ods.Api.Client.Models;
using EdFi.RtI.Core.DomainServiceProvider;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EdFi.RtI.Core.Services.Assessments
{
    public interface IAssessmentService : IDomainService
    {
        Task<Assessment> Create(Assessment assessment);
        Task<Assessment> Update(string assessmentId, Assessment assessment);
        Task<Assessment> GetById(string assessmentId);
        Task<Assessment> GetByIdentifier(string identifier);
        Task<object> GetAssessmentFamilies();
        Task<IEnumerable<Assessment>> Delete(string assessmentId);
        Task<IEnumerable<Assessment>> Search(AssessmentSearchParams search);
    }
}
