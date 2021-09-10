using EdFi.Ods.Api.Client.Models;
using EdFi.RtI.Core.DomainServiceProvider;
using EdFi.RtI.Core.Mapper;
using EdFi.RtI.Core.OdsApi;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EdFi.RtI.Core.Services.Students
{
    public class StudentServiceV3 : IStudentService
    {
        private readonly IDomainServiceProvider _domainServiceProvider;
        private readonly IOdsApiClientProvider _odsApiClientProvider;

        public StudentServiceV3(IDomainServiceProvider domainServiceProvider, IOdsApiClientProvider odsApiClientProvider)
        {
            _domainServiceProvider = domainServiceProvider;
            _odsApiClientProvider = odsApiClientProvider;
        }

        private IEdFiMapper _mapper => _domainServiceProvider.GetService<IEdFiMapper>();

        public async Task<Student> GetStudentByUniqueId(string studentUniqueId)
        {
            Student secure(Student s)
            {
                _mapper.SecuredStudentDTO(_mapper.MapStudentDTO(s));
                return s;
            }

            var odsApi = await _odsApiClientProvider.NewResourcesClient();

            var studentsFromApi = await odsApi.Get<IList<Student>>("students", new Dictionary<string, string>
            {
                { "studentUniqueId", studentUniqueId },
            });

            var student = studentsFromApi.First();

            return secure(student);
        }

        public async Task<IEnumerable<Student>> GetStudentsAll(bool getFromCache = true, bool storeInCache = true)
        {
            var odsApi = await _odsApiClientProvider.NewResourcesClient();
            return await odsApi.Get<IList<Student>>("students");
        }

        public async Task<IEnumerable<Student>> GetStudentsBySectionId(string sectionId, bool getFromCache = true, bool storeInCache = true)
        {
            IEnumerable<Student> secure(IEnumerable<Student> students)
            {
                var sorted = students.OrderBy(student => student.LastSurname);
                var secured = _mapper.Secured(sorted);
                return secured.ToList();
            }

            var odsApi = await _odsApiClientProvider.NewCompositesClient();
            var studentsFromApi = await odsApi.Get<IList<Student>>($"enrollment/sections/{sectionId}/students"); // TODO Should this be using Students from v3? Is the model the same in both versions?

            return secure(studentsFromApi);
        }
    }
}
