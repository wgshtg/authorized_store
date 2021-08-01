using System;
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
        private static IList<Store> _stores;
        
        public StoreDao(IList<Store> stores) => _stores = stores;

        public async Task<IPagedList<Store>> FindAllAsync(SearchCriteria criteria)
        {
            var stores = await Task.Run(() => _stores);
            var pi = criteria.PageIndex <= 0 ? 1 : criteria.PageIndex;
            var ps = criteria.PageSize <= 0
                ? stores.Count > 0
                    ? stores.Count
                    : 1
                : criteria.PageSize;
            var result = stores.Skip((pi - 1) * ps).Take(ps);
            return new StaticPagedList<Store>(result, pi, ps, stores.Count);
        }

        public async Task<Store> GetAsync(int id)
        {
            var stores = await Task.Run(() => _stores);
            return stores.FirstOrDefault(x => x.Id == id);
        }

        public async Task<Store> CreateAsync(Store entity)
        {
            if (string.IsNullOrWhiteSpace(entity.Name)
                || string.IsNullOrWhiteSpace(entity.ContractContent))
            {
                return null;
            }

            var stores = await Task.Run(() => _stores);
            if (stores.Any(x => x.Name.Equals(entity.Name, StringComparison.Ordinal)))
            {
                throw new DuplicateNameException();
            }

            entity.Id = stores.Max(x => x.Id) + 1;
            return await Task.Run(() =>
            {
                _stores.Add(entity);
                return entity;
            });
        }

        public async Task<Store> UpdateAsync(int id, Store entity)
        {
            if (string.IsNullOrWhiteSpace(entity.Name)
                || string.IsNullOrWhiteSpace(entity.ContractContent))
            {
                return null;
            }

            var stores = await Task.Run(() => _stores);
            var store = stores.FirstOrDefault(x => x.Id == id);
            if (store == null)
            {
                return null;
            }

            if (!store.Name.Equals(entity.Name, StringComparison.Ordinal)
                && stores.Count(x => x.Name.Equals(entity.Name, StringComparison.Ordinal)) != 0)
            {
                return null;
            }

            return await Task.Run(() =>
            {
                store.Name = entity.Name;
                store.Category = entity.Category;
                store.Phone = entity.Phone;
                store.Address = entity.Address;
                store.Website = entity.Website;
                store.ContractContent = entity.ContractContent;
                store.ContractStartDate = entity.ContractStartDate;
                store.ContractEndDate = entity.ContractEndDate;
                return store;
            });
        }

        public async Task DeleteAsync(int id)
        {
            var stores = await Task.Run(() => _stores);
            var store = stores.FirstOrDefault(x => x.Id == id);
            if (store != null)
            {
                await Task.Run(() => stores.Remove(store));
            }
        }
    }
}
