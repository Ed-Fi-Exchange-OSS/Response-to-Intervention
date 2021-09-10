using EdFi.Ods.Api.Client.Models;
using EdFi.RtI.Core.DomainServiceProvider;
using EdFi.RtI.Core.DTOs.Composed;
using EdFi.RtI.Core.RequestBodies;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EdFi.RtI.Core.Services.ScoringInterventions
{
    public interface IScoringInterventionsService : IDomainService
    {
        Task<StudentInterventionAssociation> CreateAssociation(ScoringInterventionPostBody body);
        Task DeleteAssociation(ScoringInterventionDeleteBody body);
        Task<IEnumerable<StudentInterventionsDTO>> GetStudentInterventionsDTO(ScoringInterventionSearchFilters filters, bool getFromCache = true, bool storeInCache = true);
        Task<StudentInterventionsDTO> GetStudentInterventionsDTOByStudent(string studentUniqueId);
    }
}
