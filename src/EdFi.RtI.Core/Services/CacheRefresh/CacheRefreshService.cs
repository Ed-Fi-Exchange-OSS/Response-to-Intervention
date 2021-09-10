using EdFi.Ods.Api.Client;
using EdFi.Ods.Api.Client.Models;
using EdFi.RtI.Core.DomainServiceProvider;
using EdFi.RtI.Core.DTOs.Composed;
using EdFi.RtI.Core.Infrastructure;
using EdFi.RtI.Core.Mapper;
using EdFi.RtI.Core.OdsApi;
using EdFi.RtI.Core.Providers.Cache;
using EdFi.RtI.Core.RequestBodies;
using EdFi.RtI.Core.Services.Assessments;
using EdFi.RtI.Core.Services.Catalogs;
using EdFi.RtI.Core.Services.Interventions;
using EdFi.RtI.Core.Services.Me;
using EdFi.RtI.Core.Services.ScoringAssessments;
using EdFi.RtI.Core.Services.ScoringInterventions;
using EdFi.RtI.Core.Services.Students;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EdFi.RtI.Core.Services.CacheRefresh
{
    public class CacheRefreshService : ICacheRefreshService
    {
        private readonly ICacheStore _cache;
        private readonly IDomainServiceProvider _domainServiceProvider;

        public CacheRefreshService(ICacheStore cache, IDomainServiceProvider domainServiceProvider)
        {
            _cache = cache;
            _domainServiceProvider = domainServiceProvider;
        }

        private IAssessmentService _assessments => _domainServiceProvider.GetService<IAssessmentService>();
        private ICatalogService _catalog => _domainServiceProvider.GetService<ICatalogService>();
        private IInterventionService _interventions => _domainServiceProvider.GetService<IInterventionService>();
        private IEdFiMapper _mapper => _domainServiceProvider.GetService<IEdFiMapper>();
        private IMeService _me => _domainServiceProvider.GetService<IMeService>();
        private IScoringAssessmentsService _scoringAssessments => _domainServiceProvider.GetService<IScoringAssessmentsService>();
        private IScoringInterventionsService _scoringInterventions => _domainServiceProvider.GetService<IScoringInterventionsService>();
        private IStudentService _students => _domainServiceProvider.GetService<IStudentService>();

        public async Task<string> GetLastRefreshedDateLocal(string key)
        {
            string cachedDateStr = await GetLastRefreshedDateUTC(key);
            var cachedDate = DateTime.Parse(cachedDateStr);
            var dateUtc = DateTime.SpecifyKind(cachedDate, DateTimeKind.Utc);
            var dateLocal = dateUtc.ToLocalTime();
            return dateLocal.ToString();
        }

        public async Task<string> GetLastRefreshedDateUTC(string key)
        {
            string date = await _cache.GetOrDefault<string>(key);

            if (date == null)
                throw new Exception("Date not cached with key " + key);

            return date;
        }

        public async Task RefreshAssessments()
        {
            var assessments = await _assessments.Search(new AssessmentSearchParams
            {
                GetFromCache = false,
                StoreInCache = false,
            });

            await _cache.TrySet(CacheKeys.Assessments, assessments);
            await UpdateLastRefreshedDate(CacheKeys.RefreshDates.Assessments);
        }

        public async Task RefreshInterventions()
        {
            var interventions = await _interventions.Search(new InterventionSearchParams
            {
                GetFromCache = false,
                StoreInCache = false,
            });

            await _cache.TrySet(CacheKeys.Interventions, interventions);
            await UpdateLastRefreshedDate(CacheKeys.RefreshDates.Interventions);
        }

        public async Task RefreshSchools()
        {
            var schools = await _catalog.GetSchoolsAll(getFromCache: false, storeInCache: false);
            await _cache.TrySet(CacheKeys.Schools, schools);
            await UpdateLastRefreshedDate(CacheKeys.RefreshDates.Schools);
        }

        public async Task RefreshScoringAssessments()
        {
            var sections = await _catalog.GetSectionsAll(getFromCache: false, storeInCache: false);

            foreach (var section in sections)
            {
                var scoringAssessmentsBySection = await _scoringAssessments.Search(new ScoringAssessmentSearchParams
                {
                    UniqueSectionCode = section.UniqueSectionCode,
                    GetFromCache = false,
                    StoreInCache = false,
                });

                string key = CacheKeys.Composed(CacheKeys.AssessmentScoringsBySectionUniqueId, section.UniqueSectionCode);
                await _cache.TrySet(key, scoringAssessmentsBySection);

                string refreshDateKey = CacheKeys.Composed(CacheKeys.RefreshDates.AssessmentScoringsBySectionUniqueId, section.UniqueSectionCode);
                await UpdateLastRefreshedDate(refreshDateKey);
            }
        }

        public async Task RefreshScoringInterventions()
        {
            var sections = await _catalog.GetSectionsAll(getFromCache: false, storeInCache: false);

            foreach (var section in sections)
            {
                var filters = new ScoringInterventionSearchFilters(section.Id);
                var studentInterventionAssociations = await _scoringInterventions.GetStudentInterventionsDTO(filters, getFromCache: false, storeInCache: false);

                string key = CacheKeys.Composed(CacheKeys.InterventionScoringsBySectionUniqueId, section.Id);
                await _cache.TrySet(key, studentInterventionAssociations);

                string refreshDateKey = CacheKeys.Composed(CacheKeys.RefreshDates.InterventionScoringsBySectionUniqueId, section.Id);
                await UpdateLastRefreshedDate(refreshDateKey);
            }
        }

        public async Task RefreshSections()
        {
            var sections = await _catalog.GetSectionsAll(getFromCache: false, storeInCache: false);
            await _cache.TrySet(CacheKeys.Sections, sections);
            await UpdateLastRefreshedDate(CacheKeys.RefreshDates.Sections);
        }

        /// <summary>
        /// Uses StaffsBySchool. For optimal use, refresh RefreshStaffsBySchool first.
        /// </summary>
        public async Task RefreshSectionsByStaff()
        {
            var staffsBySchool = await _cache.GetOrDefault<IEnumerable<StaffsBySchoolDTO>>(CacheKeys.StaffsBySchool);

            if (staffsBySchool == null || staffsBySchool.Count() == 0)
                throw new Exception("StaffsBySchool are not cached. Use RefreshStaffsBySchool first.");

            var sectionsByStaff = new List<SectionsByStaffDTO>();

            foreach (var staffsBySchoolDto in staffsBySchool)
            {
                foreach (var staff in staffsBySchoolDto.Staffs)
                {
                    var sections = await _catalog.GetSectionsByStaff(staff.Id);

                    sectionsByStaff.Add(new SectionsByStaffDTO
                    {
                        StaffId = staff.Id,
                        Sections = sections,
                    });
                }
            }

            await _cache.TrySet(CacheKeys.SectionsByStaff, sectionsByStaff);
            await UpdateLastRefreshedDate(CacheKeys.RefreshDates.SectionsByStaff);
        }

        public async Task RefreshStaffEmailIdPairs()
        {
            // TODO Is this method still being used? Should we remove it?

            /*
            int currentPageIndex = 1;
            int pageSize = 100;
            int maxRecordCount = 3000;
            int currentRecordCount = 0;

            while (true)
            {
                var rawStaffs = await _resources.ResourcesClient.GetStaffsAllAsync(
                    EdFiRequestHelper.GetOffset(currentPageIndex, pageSize),
                    EdFiRequestHelper.GetLimit(pageSize)
                );

                var parsedStaffs = _mapper.Map<IEnumerable<Staff>>(rawStaffs);

                foreach (var staff in parsedStaffs)
                {
                    var emails = staff.ElectronicMails
                        .Select(email => email.ElectronicMailAddress.Trim().ToLower())
                        .ToList();

                    foreach (var email in emails)
                    {
                        string staffIdByEmailKey = CacheKeys.Composed(CacheKeys.StaffIdByEmail, email);
                        string staffUniqueIdByEmailKey = CacheKeys.Composed(CacheKeys.StaffUniqueIdByEmail, email);

                        await _cache.TrySet(staffIdByEmailKey, staff.Id);
                        await _cache.TrySet(staffUniqueIdByEmailKey, staff.StaffUniqueId);

                        string staffIdByEmailDateRefreshKey = CacheKeys.Composed(CacheKeys.RefreshDates.StaffIdByEmail, email);
                        string staffUniqueIdByEmailDateRefreshKey = CacheKeys.Composed(CacheKeys.RefreshDates.StaffUniqueIdByEmail, email);

                        await UpdateLastRefreshedDate(staffIdByEmailDateRefreshKey);
                        await UpdateLastRefreshedDate(staffUniqueIdByEmailDateRefreshKey);
                    }
                }

                currentRecordCount += parsedStaffs.Count();

                if (currentPageIndex == 1 && currentRecordCount < pageSize) // already got all records
                    break;

                currentPageIndex++;

                if (currentRecordCount > maxRecordCount) // max record count has been reached
                    break;
            }

            await UpdateLastRefreshedDate(CacheKeys.RefreshDates.ProfilesStaffEmailIdPairs);
            */
        }

        public async Task RefreshStaffs()
        {
            var staffs = await _catalog.GetStaffsAll(getFromCache: false, storeInCache: false);
            await _cache.TrySet(CacheKeys.Staffs, staffs);
            await UpdateLastRefreshedDate(CacheKeys.RefreshDates.Staffs);
        }

        public async Task RefreshStaffsBySchool()
        {
            var schools = await _catalog.GetSchoolsAll(getFromCache: false, storeInCache: false);
            var staffsBySchool = new List<StaffsBySchoolDTO>();

            foreach (var school in schools)
            {
                var staffs = await _catalog.GetStaffsBySchool(school.Id, false, false);

                staffsBySchool.Add(new StaffsBySchoolDTO
                {
                    SchoolId = school.Id,
                    Staffs = staffs,
                });
            }

            bool cachedSuccessfully = await _cache.TrySet(CacheKeys.StaffsBySchool, staffsBySchool);
            await UpdateLastRefreshedDate(CacheKeys.RefreshDates.StaffsBySchool);
        }

        public async Task RefreshStudents()
        {
            var students = await _students.GetStudentsAll(getFromCache: false, storeInCache: false);
            await _cache.TrySet(CacheKeys.Students, students);
            await UpdateLastRefreshedDate(CacheKeys.RefreshDates.Students);
        }

        /// <summary>
        /// Uses SectionsByStaff. For optimal use refresh, RefreshSectionsByStaff first.
        /// </summary>
        public async Task RefreshStudentsBySection()
        {
            var sectionsByStaff = await _cache.GetOrDefault<IEnumerable<SectionsByStaffDTO>>(CacheKeys.SectionsByStaff);

            if (sectionsByStaff == null || sectionsByStaff.Count() == 0)
                throw new Exception("SectionsByStaff are not cached. Use RefreshSectionsByStaff first.");

            var studentsBySection = new List<StudentsBySectionDTO>();
            int currentCount = 0; // for debugging purposes

            foreach (var staff in sectionsByStaff)
            {
                foreach (var section in staff.Sections)
                {
                    var students = await _students.GetStudentsBySectionId(section.Id, getFromCache: false, storeInCache: false);

                    studentsBySection.Add(new StudentsBySectionDTO
                    {
                        SectionId = section.Id,
                        Students = students,
                    });

                    currentCount += 1;
                }
            }

            await _cache.TrySet(CacheKeys.StudentsBySection, studentsBySection);
            await UpdateLastRefreshedDate(CacheKeys.RefreshDates.StudentsBySection);
        }

        public async Task RefreshUserProfiles()
        {
            var users = await _me.SearchUsers(new UserSessionProfileSearchParams
            {
                GetFromCache = false,
                StoreInCache = false,
            });

            await _cache.TrySet(CacheKeys.Profiles, users);
            await UpdateLastRefreshedDate(CacheKeys.RefreshDates.Profiles);
        }
        
        private async Task UpdateLastRefreshedDate(string key)
        {
            var now = DateTime.Now;
            var nowStr = now.ToUniversalTime().ToString();
            await _cache.TrySet(key, nowStr);
        }
    }
}
