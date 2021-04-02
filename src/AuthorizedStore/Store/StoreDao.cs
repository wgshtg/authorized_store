using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace AuthorizedStore
{
    public class StoreDao : IStoreDao
    {
        private static IList<Store> _stores;
        private readonly Func<Task<IList<Store>>> _getStores = () => Task.FromResult(_stores);

        public StoreDao(List<Store> stores) => _stores = stores;

        public async Task<IPagedList<Store>> FindAllAsync(SearchCriteria criteria)
        {
            var stores = await _getStores.Invoke();
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
            var stores = await _getStores.Invoke();
            return _stores.FirstOrDefault(x => x.Id == id);
        }
    }
}
