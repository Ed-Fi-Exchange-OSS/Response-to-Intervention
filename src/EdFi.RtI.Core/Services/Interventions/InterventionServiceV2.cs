using EdFi.Ods.Api.Client.Models;
using EdFi.RtI.Core.DomainServiceProvider;
using EdFi.RtI.Core.DTOs.Interventions;
using EdFi.RtI.Core.Mapper;
using EdFi.RtI.Core.OdsApi;
using EdFi.RtI.Core.Providers.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EdFi.RtI.Core.Services.Interventions
{
    public class InterventionServiceV2 : IInterventionService
    {
        private readonly ICacheStore _cacheProvider;
        private readonly IDomainServiceProvider _domainServiceProvider;
        private readonly IOdsApiClientProvider _odsApiClientProvider;

        public InterventionServiceV2(ICacheStore cacheProvider, IDomainServiceProvider domainServiceProvider, IOdsApiClientProvider odsApiClientProvider)
        {
            _cacheProvider = cacheProvider;
            _domainServiceProvider = domainServiceProvider;
            _odsApiClientProvider = odsApiClientProvider;
        }

        private IEdFiMapper _mapper => _domainServiceProvider.GetService<IEdFiMapper>();

        public async Task<object> Create(Intervention intervention)
        {
            var ods = await _odsApiClientProvider.NewResourcesClient();
            var response = await ods.Post("interventions", intervention);
            var createdInterventionId = await ods.HandleHttpResponseGetCreatedResourceId(response);
            var createdIntervention = await ods.Get<Intervention>($"interventions/{createdInterventionId}");

            var cachedInterventions = await _cacheProvider.Get<IList<Intervention>>(CacheKeys.Interventions);
            cachedInterventions.Add(createdIntervention);
            await _cacheProvider.TrySet(CacheKeys.Interventions, cachedInterventions);

            return createdIntervention;
        }

        public async Task<InterventionDTO> GetById(string interventionId)
        {
            var ods = await _odsApiClientProvider.NewResourcesClient();
            var intervention = await ods.Get<object>($"interventions/{interventionId}");
            return _mapper.MapInterventionDTO(intervention);
        }

        public async Task<Intervention> GetByIdentificationCode(string identificationCode)
        {
            var key = CacheKeys.Composed(CacheKeys.InterventionByIdentificationCode, identificationCode);

            if (await _cacheProvider.TryHasKey(key))
                return await _cacheProvider.Get<Intervention>(key);

            var ods = await _odsApiClientProvider.NewResourcesClient();

            var interventions = await ods.Get<IList<Intervention>>("interventions", new Dictionary<string, string>
            {
                ["identificationCode"] = identificationCode,
            });

            var intervention = interventions.FirstOrDefault();

            if (intervention == null)
                throw new InterventionNotFoundException(identificationCode);

            await _cacheProvider.TrySet(key, intervention);

            return intervention;
        }

        public async Task<Intervention> Update(string interventionId, Intervention intervention)
        {
            var ods = await _odsApiClientProvider.NewResourcesClient();
            var response = await ods.Put($"interventions/{interventionId}", intervention);
            await ods.HandleHttpResponse(response);

            var cachedInterventions = await _cacheProvider.Get<List<Intervention>>(CacheKeys.Interventions);
            var foundInterventionIndex = cachedInterventions.FindIndex(x => x.Id == interventionId);

            if (foundInterventionIndex == -1)
                cachedInterventions.Add(intervention);
            else
                cachedInterventions[foundInterventionIndex] = intervention;

            await _cacheProvider.TrySet(CacheKeys.Interventions, cachedInterventions);

            return intervention;
        }

        public async Task<IEnumerable<InterventionDTO>> Delete(string interventionId)
        {
            var ods = await _odsApiClientProvider.NewResourcesClient();
            var response = await ods.Delete($"interventions/{interventionId}");
            await ods.HandleHttpResponse(response);

            var cachedInterventions = await _cacheProvider.GetOrDefault<IList<Intervention>>(CacheKeys.Interventions);

            if (cachedInterventions != null)
            {
                var filteredInterventions = cachedInterventions
                    .Where(x => x.Id != interventionId)
                    .ToList();

                await _cacheProvider.Set(CacheKeys.Interventions, filteredInterventions);

                return _mapper.MapInterventionDTOALL(filteredInterventions);
            }
            else
            {
                var interventionsFromApi = await ods.Get<IList<Intervention>>("interventions", new Dictionary<string, string>
                {
                    ["limit"] = "100",
                });

                await _cacheProvider.TrySet(CacheKeys.Interventions, interventionsFromApi);

                return _mapper.MapInterventionDTOALL(interventionsFromApi);
            }
        }

        public async Task<IEnumerable<Intervention>> Search(SearchParams search)
        {
            if (search == null)
                throw new ArgumentNullException(nameof(search));

            var ods = await _odsApiClientProvider.NewResourcesClient();
            var interventions = await ods.Get<IList<Intervention>>("interventions");
            return ApplySearchParams(interventions, search);
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
