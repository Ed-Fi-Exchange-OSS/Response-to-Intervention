using EdFi.Ods.Api.Client.Models;
using EdFi.RtI.Core.DomainServiceProvider;
using EdFi.RtI.Core.Mapper;
using EdFi.RtI.Core.OdsApi;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EdFi.RtI.Core.Services.Students
{
    public class StudentServiceV2 : IStudentService
    {
        private readonly IDomainServiceProvider _domainServiceProvider;
        private readonly IOdsApiClientProvider _odsApiClientProvider;

        public StudentServiceV2(IDomainServiceProvider domainServiceProvider, IOdsApiClientProvider odsApiClientProvider)
        {
            _domainServiceProvider = domainServiceProvider;
            _odsApiClientProvider = odsApiClientProvider;
        }

        private IEdFiMapper _mapper => _domainServiceProvider.GetService<IEdFiMapper>();

        public async Task<IEnumerable<Student>> GetStudentsAll(bool getFromCache = true, bool storeInCache = true)
        {
            var ods = await _odsApiClientProvider.NewResourcesClient();
            return await ods.Get<IList<Student>>("enrollment/students");
        }

        public async Task<IEnumerable<Student>> GetStudentsBySectionId(string sectionId, bool getFromCache = true, bool storeInCache = true)
        {
            IEnumerable<Student> parseForReturn(IEnumerable<Student> students)
            {
                var sorted = students.OrderBy(student => student.LastSurname);
                var secured = _mapper.Secured(sorted);
                return secured.ToList();
            }

            var ods = await _odsApiClientProvider.NewResourcesClient();
            var studentsFromApi = await ods.Get<IList<Student>>($"enrollment/sections/{sectionId}/students");
            return parseForReturn(studentsFromApi);
        }

        public async Task<Student> GetStudentByUniqueId(string studentUniqueId)
        {
            var ods = await _odsApiClientProvider.NewResourcesClient();

            var student = await ods.Get<Student>("enrollment/students", new Dictionary<string, string>
            {
                { "studentUniqueId", studentUniqueId },
            });

            if (student == null)
                throw new StudentNotFoundException(studentUniqueId);

            _mapper.SecuredStudentDTO(_mapper.MapStudentDTO(student));

            return student;
        }
    }
}
