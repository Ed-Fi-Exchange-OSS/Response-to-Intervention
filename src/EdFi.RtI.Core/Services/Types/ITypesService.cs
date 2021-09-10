using EdFi.RtI.Core.DomainServiceProvider;
using System.Threading.Tasks;

namespace EdFi.RtI.Core.Services.Types
{
    public interface ITypesService : IDomainService
    {
        Task<object> GetAssessmentItemResults();
        Task<object> GetAssessmentReportingMethods();
        Task<object> GetDeliveryMethods();
        Task<object> GetInterventionClasses();
    }
}
