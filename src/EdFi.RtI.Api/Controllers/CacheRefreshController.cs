using EdFi.RtI.Core.Providers.Cache;
using EdFi.RtI.Core.Services.CacheRefresh;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EdFi.RtI.Api.Controllers
{
    [Route("cacheRefresh")]
    [ApiController]
    public class CacheRefreshController : ControllerBase
    {
        private readonly ICacheRefreshService _cacheRefreshService;

        public CacheRefreshController(ICacheRefreshService cacheRefreshService)
        {
            _cacheRefreshService = cacheRefreshService;
        }

        [HttpGet("assessments/date")]
        public async Task<ActionResult<string>> GetAssessmentsLastRefreshedDate()
        {
            return await GetLastRefreshedDate(CacheKeys.RefreshDates.Assessments);
        }

        [HttpGet("interventions/date")]
        public async Task<ActionResult<string>> GetInterventionsLastRefreshedDate()
        {
            return await GetLastRefreshedDate(CacheKeys.RefreshDates.Interventions);
        }

        [HttpGet("schools/date")]
        public async Task<ActionResult<string>> GetSchoolsLastRefreshedDate()
        {
            return await GetLastRefreshedDate(CacheKeys.RefreshDates.Schools);
        }

        [HttpGet("scoringAssessments/date/{sectionUniqueCode}")]
        public async Task<ActionResult<string>> GetScoringAssessmentsLastRefreshedDate(string sectionUniqueCode)
        {
            string refreshDateKey = CacheKeys.Composed(CacheKeys.RefreshDates.AssessmentScoringsBySectionUniqueId, sectionUniqueCode);
            return await GetLastRefreshedDate(refreshDateKey);
        }

        [HttpGet("scoringInterventions/date/{sectionUniqueCode}")]
        public async Task<ActionResult<string>> GetScoringInterventionsLastRefreshedDate(string sectionUniqueCode)
        {
            string refreshDateKey = CacheKeys.Composed(CacheKeys.RefreshDates.InterventionScoringsBySectionUniqueId, sectionUniqueCode);
            return await GetLastRefreshedDate(refreshDateKey);
        }

        [HttpGet("sections/date")]
        public async Task<ActionResult<string>> GetSectionsLastRefreshedDate()
        {
            return await GetLastRefreshedDate(CacheKeys.RefreshDates.Sections);
        }

        [HttpGet("sectionsByStaff/date")]
        public async Task<ActionResult<string>> GetSectionsByStaffLastRefreshedDate()
        {
            return await GetLastRefreshedDate(CacheKeys.RefreshDates.SectionsByStaff);
        }

        [HttpGet("staffs/date")]
        public async Task<ActionResult<string>> GetStaffsLastRefreshedDate()
        {
            return await GetLastRefreshedDate(CacheKeys.RefreshDates.Staffs);
        }

        [HttpGet("staffsBySchool/date")]
        public async Task<ActionResult<string>> GetStaffsBySchoolLastRefreshedDate()
        {
            return await GetLastRefreshedDate(CacheKeys.RefreshDates.StaffsBySchool);
        }

        [HttpGet("staffEmailIdPairs/date/{email}")]
        public async Task<ActionResult<string>> GetStaffEmailIdPairsLastRefreshedDate(string email)
        {
            string staffIdByEmailDateRefreshKey = CacheKeys.Composed(CacheKeys.RefreshDates.StaffIdByEmail, email);
            return await GetLastRefreshedDate(staffIdByEmailDateRefreshKey);
        }

        [HttpGet("students/date")]
        public async Task<ActionResult<string>> GetStudentsLastRefreshedDate()
        {
            return await GetLastRefreshedDate(CacheKeys.RefreshDates.Students);
        }

        [HttpGet("studentsBySection/date")]
        public async Task<ActionResult<string>> GetStudentsBySectionLastRefreshedDate()
        {
            return await GetLastRefreshedDate(CacheKeys.RefreshDates.StudentsBySection);
        }

        [HttpGet("userProfiles/date")]
        public async Task<ActionResult<string>> GetUserProfilesLastRefreshedDate()
        {
            return await GetLastRefreshedDate(CacheKeys.RefreshDates.Profiles);
        }

        [HttpGet("userProfiles/date/staffEmailIdPairs")]
        public async Task<ActionResult<string>> GetUserProfilesStaffEmailIdPairsLastRefreshedDate()
        {
            return await GetLastRefreshedDate(CacheKeys.RefreshDates.ProfilesStaffEmailIdPairs);
        }

        [HttpGet("assessments")]
        public async Task<ActionResult> RefreshAssessments()
        {
            await _cacheRefreshService.RefreshAssessments();
            return Ok();
        }

        [HttpGet("interventions")]
        public async Task<ActionResult> RefreshInterventions()
        {
            await _cacheRefreshService.RefreshInterventions();
            return Ok();
        }

        [HttpGet("schools")]
        public async Task<ActionResult> RefreshSchools()
        {
            await _cacheRefreshService.RefreshSchools();
            return Ok();
        }

        [HttpGet("scoringAssessments")]
        public async Task<ActionResult> RefreshScoringAssessments()
        {
            await _cacheRefreshService.RefreshScoringAssessments();
            return Ok();
        }

        [HttpGet("scoringInterventions")]
        public async Task<ActionResult> RefreshScoringInterventions()
        {
            await _cacheRefreshService.RefreshScoringInterventions();
            return Ok();
        }

        [HttpGet("sections")]
        public async Task<ActionResult> RefreshSections()
        {
            await _cacheRefreshService.RefreshSections();
            return Ok();
        }

        [HttpGet("sectionsByStaff")]
        public async Task<ActionResult> RefreshSectionsByStaff()
        {
            await _cacheRefreshService.RefreshSectionsByStaff();
            return Ok();
        }

        [HttpGet("staffEmailIdPairs")]
        public async Task<ActionResult> RefreshStaffEmailIdPairs()
        {
            await _cacheRefreshService.RefreshStaffEmailIdPairs();
            return Ok();
        }

        [HttpGet("staffs")]
        public async Task<ActionResult> RefreshStaffs()
        {
            await _cacheRefreshService.RefreshStaffs();
            return Ok();
        }

        [HttpGet("staffsBySchool")]
        public async Task<ActionResult> RefreshStaffsBySchool()
        {
            await _cacheRefreshService.RefreshStaffsBySchool();
            return Ok();
        }

        [HttpGet("students")]
        public async Task<ActionResult> RefreshStudents()
        {
            await _cacheRefreshService.RefreshStudents();
            return Ok();
        }

        [HttpGet("studentsBySection")]
        public async Task<ActionResult> RefreshStudentsBySection()
        {
            await _cacheRefreshService.RefreshStudentsBySection();
            return Ok();
        }

        [HttpGet("userProfiles")]
        public async Task<ActionResult> RefreshUserProfiles()
        {
            await _cacheRefreshService.RefreshUserProfiles();
            return Ok();
        }

        private async Task<ActionResult<string>> GetLastRefreshedDate(string key)
        {
            string date = await _cacheRefreshService.GetLastRefreshedDateUTC(key);
            return Ok(date);
        }
    }
}
