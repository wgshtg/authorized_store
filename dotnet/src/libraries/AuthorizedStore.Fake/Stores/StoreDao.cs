using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using AuthorizedStore.Abstractions;
using X.PagedList;

namespace AuthorizedStore.Fake
{
    public class StoreDao : IStoreDao
    {
        private readonly IList<Store> _stores;

        public StoreDao(IList<Store> stores) => _stores = stores;

        public async Task<Store> CreateAsync(Store entity)
        {
            var stores = await Task.Run(() => _stores);
            entity.Id = stores.Max(x => x.Id) + 1;
            _stores.Add(entity);

            return entity;
        }

        public async Task DeleteAsync(int id)
        {
            var stores = await Task.Run(() => _stores);
            var store = stores.FirstOrDefault(x => x.Id == id);
            if (store != null)
            {
                stores.Remove(store);
            }
        }

        public async Task<Store> GetAsync(int id)
        {
            var stores = await Task.Run(() => _stores);
            return stores.FirstOrDefault(x => x.Id == id);
        }

        public async Task<Store> GetDuplicateAsync(string name, int? excludedId = null)
        {
            var stores = await Task.Run(() => _stores);
            if (string.IsNullOrWhiteSpace(name))
            {
                return null;
            }

            var result = stores.Where(x => x.Name.Equals(name, System.StringComparison.Ordinal));
            result = excludedId.HasValue
                ? result.Where(x => x.Id != excludedId.Value)
                : result;

            return result.FirstOrDefault();
        }

        public async Task<IPagedList<Store>> FindAllAsync(StoreCriteria criteria)
        {
            var stores = await Task.Run(() => _stores);
            var result = string.IsNullOrWhiteSpace(criteria.Name)
                ? stores
                : stores.Where(c => c.Name.Contains(criteria.Name));
            var count = result.Count();
            var pi = criteria.PageIndex <= 0 ? 1 : criteria.PageIndex;
            var ps = criteria.PageSize <= 0 ? 10 : criteria.PageSize;
            result = result.Skip((pi - 1) * ps).Take(ps);

            return new StaticPagedList<Store>(result, pi, ps, count);
        }

        public async Task<Store> UpdateAsync(int id, Store entity)
        {
            var stores = await Task.Run(() => _stores);
            var store = stores.FirstOrDefault(x => x.Id == id);
            if (store == null)
            {
                return null;
            }

            store.Name = entity.Name;
            store.Category = entity.Category;
            store.Phone = entity.Phone;
            store.Address = entity.Address;
            store.Website = entity.Website;
            store.ContractContent = entity.ContractContent;
            store.ContractStartDate = entity.ContractStartDate;
            store.ContractEndDate = entity.ContractEndDate;
            return store;
        }
    }
}
