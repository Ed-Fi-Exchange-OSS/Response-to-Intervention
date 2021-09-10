using EdFi.Ods.Api.Client.Models;
using EdFi.RtI.Core.DomainServiceProvider;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EdFi.RtI.Core.Services.Students
{
    public interface IStudentService : IDomainService
    {
        Task<IEnumerable<Student>> GetStudentsAll(bool getFromCache = true, bool storeInCache = true);

        /// <summary>
        /// This methos uses the section id, NOT the section unique code
        /// </summary>
        /// <param name="sectionId"></param>
        /// <param name="getFromCache"></param>
        /// <param name="storeInCache"></param>
        /// <returns></returns>
        Task<IEnumerable<Student>> GetStudentsBySectionId(string sectionId, bool getFromCache = true, bool storeInCache = true);
        
        Task<Student> GetStudentByUniqueId(string studentUniqueId);
    }
}
