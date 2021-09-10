using EdFi.Ods.Api.Client.Models;
using EdFi.RtI.Core.DomainServiceProvider;
using EdFi.RtI.Core.DTOs.Composed;
using EdFi.RtI.Core.DTOs.Student;
using EdFi.RtI.Core.Mapper;
using EdFi.RtI.Core.Models;
using EdFi.RtI.Core.OdsApi;
using EdFi.RtI.Core.Providers.Cache;
using EdFi.RtI.Core.RequestBodies;
using EdFi.RtI.Core.Services.Interventions;
using EdFi.RtI.Core.Services.Students;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EdFi.RtI.Core.Services.ScoringInterventions
{
    public class ScoringInterventionsServiceV3 : IScoringInterventionsService
    {
        private readonly ICacheStore _cache;
        private readonly IDomainServiceProvider _domainServiceProvider;
        private readonly IOdsApiClientProvider _odsApiClientProvider;

        public ScoringInterventionsServiceV3(ICacheStore cache, IDomainServiceProvider domainServiceProvider, IOdsApiClientProvider odsApiClientProvider)
        {
            _cache = cache;
            _domainServiceProvider = domainServiceProvider;
            _odsApiClientProvider = odsApiClientProvider;
        }

        private IEdFiMapper _mapper => _domainServiceProvider.GetService<IEdFiMapper>();
        private IInterventionService _interventions => _domainServiceProvider.GetService<IInterventionService>();
        private IStudentService _students => _domainServiceProvider.GetService<IStudentService>();

        public async Task<StudentInterventionAssociation> CreateAssociation(ScoringInterventionPostBody body)
        {
            var odsApi = await _odsApiClientProvider.NewResourcesClient();

            var interventionv3 = await odsApi.Get<InterventionModelv3>($"interventions/{body.InterventionId}");

            var studentIntervention = new StudentInterventionAssociationv3
            {
                InterventionEffectivenesses = null,
                InterventionReference = new InterventionReferencev3
                {
                    EducationOrganizationId = interventionv3.EducationOrganizationReference.EducationOrganizationId,
                    InterventionIdentificationCode = interventionv3.InterventionIdentificationCode,
                },
                StudentReference = new StudentReferencev3
                {
                    StudentUniqueId = body.StudentUniqueId,
                },
            };

            var response = await odsApi.Post("studentInterventionAssociations", studentIntervention);
            await odsApi.HandleHttpResponse(response);

            // TODO Store in cache

            return studentIntervention.MapToStudentInterventionAssociation();
        }

        public async Task DeleteAssociation(ScoringInterventionDeleteBody body)
        {
            string studentInterventionAssociationId;

            if (string.IsNullOrWhiteSpace(body.StudentInterventionAssociationId))
            {
                // Association has just been added from the frontend, so we have to get
                // the created association from the API to be able to get the id

                if (string.IsNullOrWhiteSpace(body.StudentUniqueId))
                    return;

                if (string.IsNullOrWhiteSpace(body.InterventionIdentificationCode))
                    return;

                var foundAssociation = await GetStudentInterventionAssociationByStudentAndIntervention(body.StudentUniqueId, body.InterventionIdentificationCode);

                if (foundAssociation == null)
                    return;

                studentInterventionAssociationId = foundAssociation.Id;
            }
            else
                studentInterventionAssociationId = body.StudentInterventionAssociationId;

            var odsApi = await _odsApiClientProvider.NewResourcesClient();
            var response = await odsApi.Delete($"studentInterventionAssociations/{studentInterventionAssociationId}");
            //await odsApi.HandleHttpResponse(response); // TODO Uncomment later
        }

        public async Task<IEnumerable<StudentInterventionsDTO>> GetStudentInterventionsDTO(ScoringInterventionSearchFilters filters, bool getFromCache = true, bool storeInCache = true)
        {
            if (getFromCache)
            {
                try
                {
                    var fromCache = await GetStudentInterventionsDTOFromCacheOrDefaultOrThrowJsonReaderException(filters.SectionId);

                    if (fromCache != null && fromCache.Count() > 0)
                        return fromCache;

                    storeInCache = true;
                }
                catch (JsonReaderException ex)
                {
                    storeInCache = true;
                }
            }

            var associations = new List<StudentInterventionsDTO>();
            var studentsBySection = await _students.GetStudentsBySectionId(filters.SectionId);

            foreach (var student in studentsBySection)
            {
                var association = await GetStudentInterventionsDTOByStudent(student.StudentUniqueId);
                associations.Add(association);
            }

            if (storeInCache)
            {
                try
                {
                    string key = CacheKeys.Composed(CacheKeys.InterventionScoringsBySectionUniqueId, filters.SectionId);
                    await _cache.Set(key, associations);
                }
                catch (Exception ex)
                {
                    // Sometimes caching takes too long so request timeout will expire
                    // causing the method th throw an exception. Catch the error to return
                    // the scorings back to the frontend even though this error happens
                }
            }

            return associations
                .OrderBy(student => student.Student.LastSurname)
                .ToList();
        }

        public async Task<StudentInterventionsDTO> GetStudentInterventionsDTOByStudent(string studentUniqueId)
        {
            var student = await _students.GetStudentByUniqueId(studentUniqueId);
            var securedStudentDto = _mapper.SecuredStudentDTO(student);
            var interventionScorings = await GetStudentInterventionsDTOByStudent(securedStudentDto);
            return interventionScorings;
        }

        private async Task<StudentInterventionAssociation> GetStudentInterventionAssociationByStudentAndIntervention(string studentUniqueId, string interventionIdentificationCode)
        {
            var odsApi = await _odsApiClientProvider.NewResourcesClient();

            var studentInterventionsv3 = await odsApi.Get<IList<StudentInterventionAssociationv3>>("studentInterventionAssociations", new Dictionary<string, string>
            {
                { "studentUniqueId", studentUniqueId },
            });

            return studentInterventionsv3
                .FirstOrDefault(association => association.InterventionReference.InterventionIdentificationCode == interventionIdentificationCode)
                .MapToStudentInterventionAssociation();
        }

        private async Task<StudentInterventionsDTO> GetStudentInterventionsDTOByStudent(StudentDTO student)
        {
            var odsApi = await _odsApiClientProvider.NewResourcesClient();

            var studentInterventionsv3 = await odsApi.Get<IList<StudentInterventionAssociationv3>>("studentInterventionAssociations", new Dictionary<string, string>
            {
                { "studentUniqueId", student.StudentUniqueId },
            });

            var associationDTO = new StudentInterventionsDTO()
            {
                Student = student,
                Associations = new List<StudentInterventionsDTO.StudentInterventionAssociationDTO>(),
            };

            foreach (var studentIntervention in studentInterventionsv3)
            {
                var intervention = await _interventions.GetByIdentificationCode(studentIntervention.InterventionReference.InterventionIdentificationCode);

                associationDTO.Associations = associationDTO.Associations.Append(new StudentInterventionsDTO.StudentInterventionAssociationDTO
                {
                    Intervention = intervention,
                    AssociationModel = studentIntervention.MapToStudentInterventionAssociation(),
                });
            }

            return associationDTO;
        }

        private async Task<IEnumerable<StudentInterventionsDTO>> GetStudentInterventionsDTOFromCacheOrDefaultOrThrowJsonReaderException(string uniqueSectionCode)
        {
            string key = CacheKeys.Composed(CacheKeys.InterventionScoringsBySectionUniqueId, uniqueSectionCode);

            if (!(await _cache.HasKey(key)))
                return null;

            var studentInterventionsFromCache = await _cache.Get<IEnumerable<StudentInterventionsDTO>>(key);

            studentInterventionsFromCache = studentInterventionsFromCache.Select(dto =>
            {
                dto.Student = _mapper.SecuredStudentDTO(dto.Student);
                return dto;
            }).ToList();

            return studentInterventionsFromCache
                .OrderBy(student => student.Student.LastSurname)
                .ToList();
        }
    }
}
