using System.Threading.Tasks;
using X.PagedList;

namespace AuthorizedStore.Abstractions
{
    public interface ICategoryService
    {
        Task<Category> GetAsync(int id);

        Task<IPagedList<Category>> GetListAsync(CategoryCriteria criteria);

        Task<Category> CreateAsync(Category category);
    }
}
