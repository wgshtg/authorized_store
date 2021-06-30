using System.Threading.Tasks;
using X.PagedList;

namespace AuthorizedStore.Abstractions
{
    public interface ICategoryDao
    {
        Task<Category> GetAsync(int id);

        Task<Category> GetDuplicateAsync(string name, int? excludedId = null);

        Task<IPagedList<Category>> GetListAsync(CategoryCriteria criteria);

        Task<Category> CreateAsync(Category category);

        Task<Category> UpdateAsync(Category category);

        Task DeleteAsync(int id);
    }
}
