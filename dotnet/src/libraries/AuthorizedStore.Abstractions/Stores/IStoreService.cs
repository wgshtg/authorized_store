using System.Threading.Tasks;
using X.PagedList;

namespace AuthorizedStore.Abstractions
{
    public interface IStoreService
    {
        Task<Store> CreateAsync(Store entity);

        Task DeleteAsync(int id);

        Task<IPagedList<Store>> FindAllAsync(SearchCriteria criteria);

        Task<Store> GetAsync(int id);

        Task<Store> UpdateAsync(int id, Store entity);
    }
}
