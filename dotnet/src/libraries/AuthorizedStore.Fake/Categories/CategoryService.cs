using AuthorizedStore.Abstractions;

namespace AuthorizedStore.Fake
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryDao _categoryDao;

        public CategoryService(ICategoryDao categoryDao)
            => _categoryDao = categoryDao;
    }
}
