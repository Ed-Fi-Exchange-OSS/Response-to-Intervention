using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EdFi.Ods.Api.Client.Models;
using EdFi.RtI.Core.DomainServiceProvider;
using EdFi.RtI.Core.DTOs.Composed;
using EdFi.RtI.Core.Providers.Cache;
using EdFi.RtI.Core.RequestBodies;
using EdFi.RtI.Core.Services.Cache;
using EdFi.RtI.Core.Services.Me;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EdFi.RtI.Api.Controllers
{
    [Route("cache")]
    [ApiController]
    public class CacheController : ControllerBase
    {
        private readonly ICacheStore _cacheProvider;
        private readonly ICacheService _cacheService;
        private readonly IDomainServiceProvider _domainServiceProvider;

        public CacheController(ICacheStore cacheProvider, ICacheService service, IDomainServiceProvider domainServiceProvider)
        {
            _cacheProvider = cacheProvider;
            _cacheService = service;
            _domainServiceProvider = domainServiceProvider;
        }

        private IMeService MeService
        {
            get {
                return _domainServiceProvider.GetService<IMeService>();
            }
        }

        [HttpGet("clear")]
        public async Task<ActionResult> Clear()
        {
            await _cacheProvider.Clear();
            return Ok();
        }

        [HttpPost("scoring/assessmentsAll")] // TODO Move this to CacheRefreshController
        public async Task<ActionResult> CacheScoringAssessmentsAll([FromBody] AssessmentScoringFiltersAndStudentAssessmentsDTO body)
        {
            await _cacheService.CacheScoringAssessmentsAll(body.SearchParams, body.Scorings);
            return Ok();
        }

        [HttpPost("scoring/interventions")]
        public async Task<ActionResult> CacheScoringInterventions([FromBody] ScoringInterventionSearchFilters filters)
        {
            await _cacheService.CacheScoringInterventions(filters);
            return Ok();
        }

        [HttpGet("key/{key}"), AllowAnonymous]
        public async Task<object> Get(string key)
        {
            return await _cacheProvider.Get<object>(key);
        }

        [HttpGet("keys"), AllowAnonymous]
        public IList<string> GetKeys()
        {
            return _cacheProvider.GetKeys();
        }

        [HttpGet("assessments")]
        public async Task<ActionResult<IEnumerable<Assessment>>> GetCachedAssessments()
        {
            var result = await _cacheProvider.Get <IEnumerable<Assessment>>(CacheKeys.Assessments);
            return Ok(result);
        }

        [HttpGet("interventions")]
        public async Task<ActionResult<IEnumerable<Intervention>>> GetCachedInterventions()
        {
            var result = await _cacheProvider.Get<IEnumerable<Intervention>>(CacheKeys.Interventions);
            return Ok(result);
        }

        [HttpGet("schools")]
        public async Task<ActionResult<IEnumerable<School>>> GetCachedSchools()
        {
            var result = await _cacheProvider.Get<IEnumerable<School>>(CacheKeys.Schools);
            return Ok(result);
        }

        [HttpGet("scoringAssessments/{sectionUniqueId}")]
        public async Task<ActionResult<IEnumerable<ScoringAssessmentDTO>>> GetCachedScoringAssessments(string sectionUniqueId)
        {
            string key = CacheKeys.Composed(CacheKeys.AssessmentScoringsBySectionUniqueId, sectionUniqueId);
            var result = await _cacheProvider.Get<IEnumerable<ScoringAssessmentDTO>>(key);
            return Ok(result);
        }

        [HttpGet("scoringInterventions/{sectionUniqueId}")]
        public async Task<ActionResult<IEnumerable<StudentInterventionsDTO>>> GetCachedScoringInterventions(string sectionUniqueId)
        {
            string key = CacheKeys.Composed(CacheKeys.InterventionScoringsBySectionUniqueId, sectionUniqueId);
            var result = await _cacheProvider.Get<IEnumerable<StudentInterventionsDTO>>(key);
            return Ok(result);
        }

        [HttpGet("sections")]
        public async Task<ActionResult<IEnumerable<Section>>> GetCachedSections()
        {
            var result = await _cacheProvider.Get<IEnumerable<Section>>(CacheKeys.Sections);
            return Ok(result);
        }

        [HttpGet("sectionsByStaff")]
        public async Task<ActionResult<IEnumerable<SectionsByStaffDTO>>> GetCachedSectionsByStaff()
        {
            var result = await _cacheProvider.Get<IEnumerable<SectionsByStaffDTO>>(CacheKeys.SectionsByStaff);
            return Ok(result);
        }

        [HttpGet("staffs")]
        public async Task<ActionResult<IEnumerable<Staff>>> GetCachedStaffs()
        {
            var result = await _cacheProvider.Get<IEnumerable<Staff>>(CacheKeys.Staffs);
            return Ok(result);
        }

        [HttpGet("staffsBySchool")]
        public async Task<ActionResult<IEnumerable<StaffsBySchoolDTO>>> GetCachedStaffsBySchool()
        {
            var result = await _cacheProvider.Get<IEnumerable<StaffsBySchoolDTO>>(CacheKeys.StaffsBySchool);
            return Ok(result);
        }

        [HttpGet("staffsBySchoolDetails")]
        public async Task<ActionResult<IEnumerable<StaffsBySchoolDTO>>> GetCachedStaffsBySchoolDetails()
        {
            // TODO Is this method still being used?

            /*var staffsBySchool = await _cacheProvider.Get<IEnumerable<StaffsBySchoolDTO>>(CacheKeys.StaffsBySchool);
            var staffsBySchoolDetails = new List<StaffsBySchoolDetails>();

            foreach (var item in staffsBySchool)
            {
                var rawSchool = await _resources.ResourcesClient.GetSchoolsByIdAsync(item.SchoolId);
                var school = EdFiMapper.Map<School>(rawSchool);

                staffsBySchoolDetails.Add(new StaffsBySchoolDetails
                {
                    School = school,
                    Staffs = item.Staffs,
                });
            }

            return Ok(staffsBySchoolDetails);
            */

            throw new NotImplementedException();
        }

        [HttpGet("studentsBySectionDetails")]
        public async Task<ActionResult<IEnumerable<StudentsBySectionDTO>>> GetCachedStudentsBySectionDetails()
        {
            // TODO Is this method still being used?

            /*var studentsBySection = await _cacheProvider.GetOrDefault<IEnumerable<StudentsBySectionDTO>>(CacheKeys.StudentsBySection);
                
            if (studentsBySection == null)
                return Ok(new List<IEnumerable<StudentsBySectionDTO>>());

            var studentsBySectionDetails = new List<StudentsBySectionDetails>();

            foreach (var item in studentsBySection)
            {
                var rawSection = await _resources.ResourcesClient.GetSectionsByIdAsync(item.SectionId);
                var section = EdFiMapper.Map<Section>(rawSection);

                studentsBySectionDetails.Add(new StudentsBySectionDetails
                {
                    Section = section,
                    Students = item.Students,
                });
            }

            return Ok(studentsBySectionDetails);
            */

            throw new NotImplementedException();
        }

        [HttpGet("staffEmailIdPairs")]
        public async Task<ActionResult<Dictionary<string, string>>> GetCachedStaffEmailIdPairs()
        {
            var pairs = await GetStaffEmailIdPairs();
            return Ok(pairs);
        }

        [HttpGet("students")]
        public async Task<ActionResult<IEnumerable<Student>>> GetCachedStudents()
        {
            var result = await _cacheProvider.GetOrDefault<IEnumerable<Student>>(CacheKeys.Students);

            if (result == null)

                return Ok(new List<IEnumerable<Student>>());

            return Ok(result);
        }

        [HttpGet("studentsBySection")]
        public async Task<ActionResult<IEnumerable<StudentsBySectionDTO>>> GetCachedStudentsBySection()
        {
            var result = await _cacheProvider.Get<IEnumerable<StudentsBySectionDTO>>(CacheKeys.StudentsBySection);
            return Ok(result);
        }

        [HttpGet("userProfiles")]
        public async Task<ActionResult<IEnumerable<UserSessionProfile>>> GetCachedUserProfiles()
        {
            var result = await _cacheProvider.Get<IEnumerable<UserSessionProfile>>(CacheKeys.Profiles);
            return Ok(result);
        }

        [HttpGet("userProfiles/staffEmailIdPairs")]
        public async Task<ActionResult<IEnumerable<UserSessionProfile>>> GetCachedUserProfilesByStaffEmailIdPairs()
        {
            var pairs = await GetStaffEmailIdPairs();
            var profiles = new List<UserSessionProfile>();

            foreach (var pair in pairs)
            {
                var staffId = pair.Value;
                var userProfile = await MeService.GetUserByStaffId(staffId);
                profiles.Add(userProfile);
            }

            return Ok(profiles);
        }

        [HttpDelete("{key}"), AllowAnonymous]
        public async Task RemoveKey(string key)
        {
            await _cacheProvider.Remove(key);
        }

        private async Task<Dictionary<string, string>> GetStaffEmailIdPairs()
        {
            // TODO Is this method still being used?

            /*
            int pageIndex = 1;
            int pageSize = 100;
            int maxRecordCount = 3000;
            int currentRecordCount = 0;
            var pairs = new Dictionary<string, string>();

            while (true)
            {
                var rawStaffs = await _resources.ResourcesClient.GetStaffsAllAsync(
                    EdFiRequestHelper.GetOffset(pageIndex, pageSize),
                    EdFiRequestHelper.GetLimit(pageSize)
                );

                var parsedStaffs = EdFiMapper.Map<IEnumerable<Staff>>(rawStaffs);

                foreach (var staff in parsedStaffs)
                {
                    var emails = staff.ElectronicMails
                        .Select(email => email.ElectronicMailAddress.Trim().ToLower())
                        .ToList();

                    foreach (var email in emails)
                    {
                        var staffIdByEmailKey = CacheKeys.Composed(CacheKeys.StaffIdByEmail, email);
                        var staffUniqueIdByEmailKey = CacheKeys.Composed(CacheKeys.StaffUniqueIdByEmail, email);

                        try
                        {
                            string key = CacheKeys.Composed(CacheKeys.StaffIdByEmail, email);
                            bool keyExists = await _cacheProvider.HasKey(key);

                            if (keyExists)
                            {
                                string staffId = await _cacheProvider.Get<string>(key);
                                pairs[key] = staffId;
                            }
                            else
                            {
                                // User is not cached
                            }
                        }
                        catch (Exception ex)
                        {
                            // Prevent this exception from terminating loop
                        }
                    }
                }

                currentRecordCount += parsedStaffs.Count();

                if (pageIndex == 1 && currentRecordCount < pageSize)
                    break;

                pageIndex++;

                if (currentRecordCount > maxRecordCount)
                    break;
            }

            return pairs;
            */

            throw new NotImplementedException();
        }
    }
}
