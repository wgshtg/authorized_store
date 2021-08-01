using System.Threading.Tasks;
using X.PagedList;

namespace AuthorizedStore.Abstractions
{
    public interface IStoreDao
    {
        Task<Store> CreateAsync(Store entity);

        Task DeleteAsync(int id);

        Task<IPagedList<Store>> GetListAsync(StoreCriteria criteria);

        Task<Store> GetAsync(int id);

        Task<Store> GetDuplicateAsync(string name, int? excludedId = null);

        Task<Store> UpdateAsync(int id, Store entity);
    }
}
