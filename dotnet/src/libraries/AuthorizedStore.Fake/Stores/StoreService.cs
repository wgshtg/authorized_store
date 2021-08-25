using System;
using System.Data;
using System.Threading.Tasks;
using AuthorizedStore.Abstractions;
using AuthorizedStore.Exceptions;
using AuthorizedStore.Extensions;
using X.PagedList;

namespace AuthorizedStore.Fake
{
    public class StoreService : IStoreService
    {
        private readonly IStoreDao _stores;
        private readonly ICategoryService _categoryService;

        public StoreService(IStoreDao stores, ICategoryService categoryService)
        {
            _stores = stores;
            _categoryService = categoryService;
        }

        public async Task<Store> CreateAsync(Store entity)
        {
            StringExtensions.CheckNullOrWhiteSpace(entity?.Name, nameof(Store.Name));
            StringExtensions.CheckNullOrWhiteSpace(entity?.ContractContent, nameof(Store.ContractContent));
            CheckInvalidName(entity?.Name);
            await CheckDuplicate(entity?.Name);
            entity.Category = entity.Category?.Id != null
                ? await _categoryService.GetAsync(entity.Category.Id)
                : default;

            return await _stores.CreateAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            var store = _stores.GetAsync(id)
                ?? throw new ResourceNotFoundException(nameof(Store), id);

            await _stores.DeleteAsync(id);
        }

        public async Task<IPagedList<Store>> GetListAsync(StoreCriteria criteria)
        {
            if (criteria == null)
            {
                return await _stores.GetListAsync(new StoreCriteria());
            }

            CheckInvalidName(criteria.Name);
            if (criteria.ContractStartDate > criteria.ContractEndDate)
            {
                throw new ArgumentException(
                    $"{nameof(criteria.ContractStartDate)} should be earlier than {nameof(criteria.ContractEndDate)}");
            }

            criteria.PageIndex = criteria.PageIndex <= 0 ? 1 : criteria.PageIndex;
            criteria.PageSize = criteria.PageSize <= 0 ? 10 : criteria.PageSize;

            return await _stores.GetListAsync(criteria);
        }

        public async Task<Store> GetAsync(int id)
        {
            return await _stores.GetAsync(id);
        }

        public async Task<Store> UpdateAsync(int id, Store entity)
        {
            var store = await _stores.GetAsync(id)
                ?? throw new ResourceNotFoundException(nameof(Store), id);
            StringExtensions.CheckNullOrWhiteSpace(entity?.Name, nameof(Store.Name));
            StringExtensions.CheckNullOrWhiteSpace(entity?.ContractContent, nameof(Store.ContractContent));
            CheckInvalidName(entity?.Name);
            await CheckDuplicate(entity?.Name, id);
            entity.Category = entity.Category?.Id != null
                ? await _categoryService.GetAsync(entity.Category.Id)
                : default;

            return await _stores.UpdateAsync(id, entity);
        }

        private async Task CheckDuplicate(string name, int? excludedId = null)
        {
            if (await _stores.GetDuplicateAsync(name, excludedId) != null)
            {
                throw new DuplicateNameException("Name has already been existed.");
            }
        }

        private void CheckInvalidName(string name)
        {
            if (!string.IsNullOrWhiteSpace(name) && name.ContainsInvalidKeywords())
            {
                throw new ArgumentException("Name is invalid.", nameof(Store.Name));
            }
        }
    }
}
