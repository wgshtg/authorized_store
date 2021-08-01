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

        public StoreService(IStoreDao stores)
        {
            _stores = stores;
        }

        public async Task<Store> CreateAsync(Store entity)
        {
            StringExtensions.CheckNullOrWhiteSpace(entity?.Name, nameof(Store.Name));
            StringExtensions.CheckNullOrWhiteSpace(entity?.ContractContent, nameof(Store.ContractContent));
            CheckInvalidName(entity?.Name);
            await CheckDuplicate(entity?.Name);

            return await _stores.CreateAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            var store = _stores.GetAsync(id)
                ?? throw new ResourceNotFoundException(nameof(Category), id);

            await _stores.DeleteAsync(id);
        }

        public async Task<IPagedList<Store>> FindAllAsync(StoreCriteria criteria)
        {
            if (criteria == null)
            {
                return await _stores.FindAllAsync(default);
            }

            CheckInvalidName(criteria.Name);
            if (criteria.ContractStartDate > criteria.ContractEndDate)
            {
                throw new ArgumentException(
                    $"{nameof(criteria.ContractStartDate)} should be less than {nameof(criteria.ContractEndDate)}");
            }

            criteria.PageIndex = criteria.PageIndex <= 0 ? 1 : criteria.PageIndex;
            criteria.PageSize = criteria.PageSize <= 0 ? 10 : criteria.PageSize;

            return await _stores.FindAllAsync(criteria);
        }

        public async Task<Store> GetAsync(int id)
        {
            return await _stores.GetAsync(id);
        }

        public async Task<Store> UpdateAsync(int id, Store entity)
        {
            var store = await _stores.GetAsync(id)
                ?? throw new ResourceNotFoundException(nameof(Category), id);
            StringExtensions.CheckNullOrWhiteSpace(entity?.Name, nameof(Store.Name));
            StringExtensions.CheckNullOrWhiteSpace(entity?.ContractContent, nameof(Store.ContractContent));
            CheckInvalidName(entity?.Name);
            await CheckDuplicate(entity?.Name, id);

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
