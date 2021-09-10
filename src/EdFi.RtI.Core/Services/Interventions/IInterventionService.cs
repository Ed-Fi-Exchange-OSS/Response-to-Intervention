using EdFi.Ods.Api.Client.Models;
using EdFi.RtI.Core.DomainServiceProvider;
using EdFi.RtI.Core.DTOs.Interventions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EdFi.RtI.Core.Services.Interventions
{
    public interface IInterventionService : IDomainService
    {
        Task<object> Create(Intervention intervention);
        Task<InterventionDTO> GetById(string interventionId);
        Task<Intervention> GetByIdentificationCode(string identificationCode);
        Task<IEnumerable<InterventionDTO>> Delete(string interventionId);
        Task<IEnumerable<Intervention>> Search(SearchParams search);
        Task<Intervention> Update(string interventionId, Intervention intervention);
    }
}
