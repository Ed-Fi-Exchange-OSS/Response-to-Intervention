﻿using EdFi.Ods.Api.Client;
using EdFi.Ods.Api.Client.Models;
using EdFi.RtI.Core.DomainServiceProvider;
using EdFi.RtI.Core.DTOs.Composed;
using EdFi.RtI.Core.DTOs.Staffs;
using EdFi.RtI.Core.Infrastructure;
using EdFi.RtI.Core.Mapper;
using EdFi.RtI.Core.OdsApi;
using EdFi.RtI.Core.Providers.Cache;
using EdFi.RtI.Core.Providers.Staffs;
using EdFi.RtI.Core.Services.Catalogs;
using EdFi.RtI.Core.UserRoleMappings;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EdFi.RtI.Core.Services.Me
{
    public class MeServiceV2 : IMeService
    {
        private readonly ICacheStore _cache;
        private readonly IDomainServiceProvider _domainServiceProvider;
        private readonly IOdsApiClientProvider _odsApiClientProvider;
        private readonly IUserRoleMappingsFacade _userRoleMappingsProvider;
        private readonly ISessionContext _sessionContext;

        public MeServiceV2(ICacheStore cache, IDomainServiceProvider domainServiceProvider, IOdsApiClientProvider odsApiClientProvider, IUserRoleMappingsFacade userRoleMappingsProvider, ISessionContext sessionContext)
        {
            _cache = cache;
            _domainServiceProvider = domainServiceProvider;
            _odsApiClientProvider = odsApiClientProvider;
            _userRoleMappingsProvider = userRoleMappingsProvider;
            _sessionContext = sessionContext;
        }

        private ICatalogService _catalog => _domainServiceProvider.GetService<ICatalogService>();
        private IEdFiMapper _mapper => _domainServiceProvider.GetService<IEdFiMapper>();
        private IStaffsProvider _staffsProvider => _domainServiceProvider.GetService<IStaffsProvider>();

        public async Task<IEnumerable<UserSessionProfile>> GetUserAllAdmins(bool getFromCache = true, bool storeInCache = true)
        {
            var searchParams = new UserSessionProfileSearchParams
            {
                GetFromCache = getFromCache,
                StoreInCache = storeInCache,
                Roles = await _userRoleMappingsProvider.GetUserRoleAdminMappings(),
            };

            return await SearchUsers(searchParams);
        }

        public async Task<IEnumerable<UserSessionProfile>> GetUserAllTeachers(bool getFromCache = true, bool storeInCache = true)
        {
            var searchParams = new UserSessionProfileSearchParams
            {
                GetFromCache = getFromCache,
                StoreInCache = storeInCache,
                Roles = await _userRoleMappingsProvider.GetUserRoleTeacherMappings(),
            };

            return await SearchUsers(searchParams);
        }

        public async Task<UserSessionProfile> GetUserByEmail(string email)
        {
            string normalizedEmail = email.Trim().ToLower();
            string cacheKey = CacheKeys.Composed(CacheKeys.StaffIdByEmail, normalizedEmail);
            bool keyExists = await _cache.TryHasKey(cacheKey);

            if (!keyExists)
                throw new Exception($"Staff member not cached with key {cacheKey}");

            string staffId = await _cache.GetOrDefault<string>(cacheKey);

            if (staffId == null)
                throw new Exception($"Failed to get cached StaffId with key {cacheKey}");

            return await GetUserByStaffId(staffId);
        }

        public async Task<UserSessionProfile> GetUserByStaffId(string staffId)
        {
            var ods = await _odsApiClientProvider.NewResourcesClient();
            var staff = await ods.Get<StaffDTO>($"enrollment/staffs/{staffId}");
            var association = (await GetRolesOrDefault(staff.StaffUniqueId)).First();
            var school = await GetSchoolOrDefault(association.EducationOrganizationReference.EducationOrganizationId);

            return new UserSessionProfile
            {
                Staff = staff,
                StaffEducationOrganizationAssignmentAssociation = association,
                School = school,
            };
        }

        public async Task<IEnumerable<UserSessionProfile>> SearchUsers(UserSessionProfileSearchParams searchParams)
        {
            var users = await GetUsersFromCacheOrApi(searchParams.GetFromCache, searchParams.StoreInCache);
            return ApplyFilters(users, searchParams);
        }

        private IEnumerable<UserSessionProfile> ApplyFilters(IEnumerable<UserSessionProfile> users, UserSessionProfileSearchParams searchParams)
        {
            if (searchParams.Roles != null && searchParams.Roles.Count() > 0)
            {
                var roles = new HashSet<string>(searchParams.Roles);
                users = ApplyFilterRoles(users, roles);
            }

            users = ApplyFilterSecure(users);
            users = ApplyFilterSort(users);

            return users.ToList();
        }

        private IEnumerable<UserSessionProfile> ApplyFilterRoles(IEnumerable<UserSessionProfile> users, HashSet<string> roles)
        {
            return users.Where(profile => roles.Contains(profile.StaffEducationOrganizationAssignmentAssociation.StaffClassificationDescriptor));
        }

        private IEnumerable<UserSessionProfile> ApplyFilterSecure(IEnumerable<UserSessionProfile> users)
        {
            return users.Select(dto =>
            {
                dto.Staff = _mapper.Secured(dto.Staff);
                return dto;
            });
        }

        private IEnumerable<UserSessionProfile> ApplyFilterSort(IEnumerable<UserSessionProfile> users)
        {
            return users.OrderBy(profile => profile.Staff.FullName);
        }

        private async Task<IEnumerable<UserSessionProfile>> GetUsersFromCacheOrApi(bool getFromCache = true, bool storeInCache = true)
        {
            if (getFromCache)
            {
                bool usersAreCached = await _cache.HasKey(CacheKeys.Profiles);

                if (usersAreCached)
                {
                    var cachedUsers = await _cache.GetOrDefault<IEnumerable<UserSessionProfile>>(CacheKeys.Profiles);

                    if (cachedUsers != null && cachedUsers.Count() > 0)
                        return cachedUsers;
                }
            }

            var ods = await _odsApiClientProvider.NewResourcesClient();
            var staffs = await ods.Get<IList<StaffDTO>>("enrollment/staffs");
            var profiles = new List<UserSessionProfile>();

            foreach (var staff in staffs)
            {
                try
                {
                    staff.FullName = ((staff.FirstName ?? "") + " " + (staff.LastSurname ?? "")).Trim();

                    // Users can have multiple roles
                    var roles = await GetRolesOrDefault(staff.StaffUniqueId);

                    foreach (var role in roles)
                    {
                        var school = await GetSchoolOrDefault(role.EducationOrganizationReference.EducationOrganizationId, getFromCache);

                        profiles.Add(new UserSessionProfile
                        {
                            Staff = staff,
                            StaffEducationOrganizationAssignmentAssociation = role,
                            School = school,
                        });
                    }
                }
                catch (Exception ex)
                {
                    /* Could not find profile. Skip and keep attempting to add next */
                }
            }

            if (storeInCache)
                await _cache.TrySet(CacheKeys.Profiles, profiles);

            return profiles;
        }

        private async Task<IEnumerable<StaffEducationOrganizationAssignmentAssociation>> GetRolesOrDefault(string staffUniqueId)
        {
            try
            {
                return await GetStaffEducationOrganizationAssignmentAssociationsByStaffUniqueId(staffUniqueId);
            }
            catch (Exception ex)
            {
                return new List<StaffEducationOrganizationAssignmentAssociation>
                {
                    new StaffEducationOrganizationAssignmentAssociation
                    {
                        Id = "-1",
                        StaffClassificationDescriptor = "Unknown",
                    },
                };
            }
        }

        private async Task<School> GetSchoolOrDefault(long educationOrganizationId, bool getSchoolFromCache = true)
        {
            try
            {
                return await _catalog.GetSchoolBySchoolId((int)educationOrganizationId, getSchoolFromCache);
            }
            catch (Exception ex)
            {
                return new School
                {
                    Id = "-1",
                    NameOfInstitution = "Unkown",
                };
            }
        }

        private async Task<IEnumerable<StaffEducationOrganizationAssignmentAssociation>> GetStaffEducationOrganizationAssignmentAssociationsByStaffUniqueId(string staffUniqueId)
        {
            var ods = await _odsApiClientProvider.NewResourcesClient();

            return await ods.Get<IList<StaffEducationOrganizationAssignmentAssociation>>("staffEducationOrganizationAssignmentAssociations", new Dictionary<string, string>
            {
                ["limit"] = "100",
            });
        }

        public async Task<UserSessionProfile> GetMyUserProfile()
        {
            var staff = await _staffsProvider.GetStaffMemberByEmail(_sessionContext.Email);
            var staffEducationOrganizationAssignmentAssociation = await GetRolesOrDefault(staff.StaffUniqueId);
            var role = staffEducationOrganizationAssignmentAssociation.First();

            role.StaffClassificationDescriptor = role.StaffClassificationDescriptor.MapToStaffClassificationDescriptorv2();

            var school = await GetSchoolOrDefault(role.EducationOrganizationReference.EducationOrganizationId, getSchoolFromCache: false);

            var staffJson = JsonConvert.SerializeObject(staff);
            var staffDto = JsonConvert.DeserializeObject<StaffDTO>(staffJson);

            return new UserSessionProfile
            {
                Staff = staffDto,
                StaffEducationOrganizationAssignmentAssociation = role,
                School = school,
            };
        }
    }
}
