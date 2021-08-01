using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using AuthorizedStore.Abstractions;
using AuthorizedStore.Exceptions;
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
            CheckNullOrWhiteSpace(entity.Name, nameof(entity.Name));
            CheckNullOrWhiteSpace(entity.ContractContent, nameof(entity.ContractContent));
            var criteria = new SearchCriteria
            {
                Name = entity.Name
            };
            var duplicated = await _stores.FindAllAsync(criteria);
            if (duplicated.Count > 1)
            {
                throw new DuplicateNameException($"{entity.Name} has already been existed.");
            }

            return await _stores.CreateAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            var store = _stores.GetAsync(id) ?? throw new ResourceNotFoundException(nameof(Category), id);

            await _stores.DeleteAsync(id);
        }

        public async Task<IPagedList<Store>> FindAllAsync(SearchCriteria criteria)
        {
            if (criteria == null)
            {
                return await _stores.FindAllAsync(default);
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
            var store = await _stores.GetAsync(id) ?? throw new ResourceNotFoundException(nameof(Category), id);
            CheckNullOrWhiteSpace(entity.Name, nameof(entity.Name));
            CheckNullOrWhiteSpace(entity.ContractContent, nameof(entity.ContractContent));
            var criteria = new SearchCriteria
            {
                Name = entity.Name
            };
            var existed = await _stores.FindAllAsync(criteria);
            if (existed.Count > 0 && existed.FirstOrDefault().Id != id)
            {
                throw new DuplicateNameException("Name has already been existed.");
            }

            return await _stores.UpdateAsync(id, entity);
        }

        private bool CheckNullOrWhiteSpace(string value, string property)
        {
            return string.IsNullOrWhiteSpace(value)
                ? throw new ArgumentNullException(property, $"{property} is required.")
                : true;
        }
    }
}
