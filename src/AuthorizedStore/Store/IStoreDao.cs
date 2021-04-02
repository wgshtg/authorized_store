using System;
using System.Threading.Tasks;
using X.PagedList;

namespace AuthorizedStore
{
    public interface IStoreDao
    {
        Task<IPagedList<Store>> FindAllAsync(SearchCriteria criteria);

        Task<Store> GetAsync(int id);
    }
}
