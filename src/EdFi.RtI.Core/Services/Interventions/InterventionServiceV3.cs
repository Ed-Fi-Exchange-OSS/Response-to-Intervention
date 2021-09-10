using EdFi.Ods.Api.Client.Models;
using EdFi.RtI.Core.DTOs.Interventions;
using EdFi.RtI.Core.Mapper;
using EdFi.RtI.Core.Models;
using EdFi.RtI.Core.OdsApi;
using EdFi.RtI.Core.Providers.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EdFi.RtI.Core.Services.Interventions
{
    public class InterventionServiceV3 : IInterventionService
    {
        private readonly ICacheStore _cache;
        private readonly IOdsApiClientProvider _odsApiClientProvider;

        public InterventionServiceV3(ICacheStore cache, IOdsApiClientProvider odsApiClientProvider)
        {
            _cache = cache;
            _odsApiClientProvider = odsApiClientProvider;
        }

        public async Task<object> Create(Intervention intervention)
        {
            var odsApi = await _odsApiClientProvider.NewResourcesClient();

            var interventionv3 = intervention.MapToInterventionModelv3();
            interventionv3.Id = null;

            var response = await odsApi.Post("interventions", interventionv3);
            var createdInterventionId = await odsApi.HandleHttpResponseGetCreatedResourceId(response);

            var createdInterventionv3 = await odsApi.Get<InterventionModelv3>($"interventions/{createdInterventionId}");
            var createdInterventionv2 = createdInterventionv3.MapToInterventionV2();

            var cachedInterventions = await _cache.GetOrDefault<IList<Intervention>>(CacheKeys.Interventions);

            if (cachedInterventions != null)
            {
                cachedInterventions.Add(createdInterventionv2);
                await _cache.TrySet(CacheKeys.Interventions, cachedInterventions);
            }

            return createdInterventionv2;
        }

        public async Task<Intervention> Update(string interventionId, Intervention intervention)
        {
            var odsApi = await _odsApiClientProvider.NewResourcesClient();

            var interventionv3 = intervention.MapToInterventionModelv3();
            interventionv3.Id = null;

            var response = await odsApi.Put($"interventions/{interventionId}", interventionv3);
            await odsApi.HandleHttpResponse(response);

            var updatedInterventionv2 = interventionv3.MapToInterventionV2();
            updatedInterventionv2.Id = interventionId;

            var cachedInterventions = await _cache.GetOrDefault<IList<Intervention>>(CacheKeys.Interventions);

            if (cachedInterventions != null)
            {
                cachedInterventions = cachedInterventions
                    .Where(i => i.Id != interventionId)
                    .ToList();

                cachedInterventions.Add(updatedInterventionv2);

                await _cache.TrySet(CacheKeys.Interventions, cachedInterventions);
            }

            return updatedInterventionv2;
        }

        public async Task<InterventionDTO> GetById(string interventionId)
        {
            var odsApi = await _odsApiClientProvider.NewResourcesClient();
            var intervention = await odsApi.Get<InterventionModelv3>($"interventions/{interventionId}");
            return intervention.MapToDto();
        }

        public async Task<Intervention> GetByIdentificationCode(string identificationCode)
        {
            var key = CacheKeys.Composed(CacheKeys.InterventionByIdentificationCode, identificationCode);

            if (await _cache.TryHasKey(key))
                return await _cache.Get<Intervention>(key);

            var odsApi = await _odsApiClientProvider.NewResourcesClient();

            var interventionsv3 = await odsApi.Get<IList<InterventionModelv3>>("interventions", new Dictionary<string, string>
            {
                { "interventionIdentificationCode", identificationCode },
            });

            var interventionv2 = interventionsv3.First().MapToInterventionV2();

            await _cache.Set(key, interventionv2);

            return interventionv2;
        }

        public async Task<IEnumerable<InterventionDTO>> Delete(string interventionId)
        {
            var odsApi = await _odsApiClientProvider.NewResourcesClient();

            var response = await odsApi.Delete($"interventions/{interventionId}");

            await odsApi.HandleHttpResponse(response);

            var cachedInterventions = await _cache.GetOrDefault<IList<Intervention>>(CacheKeys.Interventions);

            if (cachedInterventions != null)
            {
                cachedInterventions = cachedInterventions
                    .Where(i => i.Id != interventionId)
                    .ToList();

                await _cache.Set(CacheKeys.Interventions, cachedInterventions);

                return cachedInterventions.Select(c => c.MapToDto());
            }

            var interventionsFromApi = await Search(new InterventionSearchParams
            {
                GetFromCache = false,
                StoreInCache = false,
            });

            return interventionsFromApi.Select(c => c.MapToDto());
        }

        public async Task<IEnumerable<Intervention>> Search(SearchParams search)
        {
            if (search == null)
                throw new ArgumentNullException(nameof(search));

            var ods = await _odsApiClientProvider.NewResourcesClient();
            var interventionsv3 = await ods.Get<IList<InterventionModelv3>>("interventions");
            var interventionsv2 = interventionsv3.Select(i => i.MapToInterventionV2());
            return ApplySearchParams(interventionsv2, search);
        }

        private IEnumerable<Intervention> ApplySearchParams(IEnumerable<Intervention> interventions, SearchParams searchParams)
        {
            interventions = InterventionSortFields.IsSortFieldValid(searchParams.SortField)
                ? ApplySorting(interventions, searchParams.SortField, searchParams.SortDesc)
                : ApplySorting(interventions, InterventionSortFields.Default, searchParams.SortDesc);

            interventions = !string.IsNullOrWhiteSpace(searchParams.SearchValue)
                ? ApplySearchValue(interventions, searchParams.SearchValue)
                : interventions;

            interventions = searchParams.PageIndex.HasValue && searchParams.PageSize.HasValue
                ? ApplyPagination(interventions, searchParams.PageIndex.Value, searchParams.PageSize.Value)
                : interventions;

            return interventions;
        }

        private IEnumerable<Intervention> ApplySorting(IEnumerable<Intervention> interventions, string sortField, bool sortDesc)
        {
            if (sortField == InterventionSortFields.IdentificationCode)
            {
                interventions = sortDesc
                    ? interventions.OrderByDescending(i => i.IdentificationCode)
                    : interventions.OrderBy(i => i.IdentificationCode);
            }
            else if (sortField == InterventionSortFields.BeginDate)
            {
                interventions = sortDesc
                    ? interventions.OrderByDescending(i => i.BeginDate)
                    : interventions.OrderBy(i => i.BeginDate);
            }
            else if (sortField == InterventionSortFields.EndDate)
            {
                interventions = sortDesc
                    ? interventions.OrderByDescending(i => i.EndDate)
                    : interventions.OrderBy(i => i.EndDate);
            }

            return interventions;
        }

        private IEnumerable<Intervention> ApplySearchValue(IEnumerable<Intervention> interventions, string searchValue)
        {
            string normalizedSearchValue = searchValue.Trim().ToLower();

            return interventions
                .Where(i => i.IdentificationCode.Trim().ToLower().Contains(searchValue))
                .ToList();
        }

        private IEnumerable<Intervention> ApplyPagination(IEnumerable<Intervention> interventions, int pageIndex, int pageSize)
        {
            int offset = pageSize * (pageIndex - 1);

            return interventions
                .Skip(offset)
                .Take(pageSize)
                .ToList();
        }
    }
}
