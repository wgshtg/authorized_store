using System.Threading.Tasks;
using X.PagedList;

namespace AuthorizedStore.Abstractions
{
    public interface ICategoryDao
    {
        Task<IPagedList<Category>> GetListAsync(CategoryCriteria criteria);
    }
}
