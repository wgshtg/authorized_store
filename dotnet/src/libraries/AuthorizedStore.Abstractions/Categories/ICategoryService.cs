using System.Threading.Tasks;
using X.PagedList;

namespace AuthorizedStore.Abstractions
{
    public interface ICategoryService
    {
        Task<IPagedList<Category>> GetListAsync(CategoryCriteria criteria);
    }
}
