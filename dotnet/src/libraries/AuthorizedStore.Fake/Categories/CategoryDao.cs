using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthorizedStore.Abstractions;
using X.PagedList;

namespace AuthorizedStore.Fake
{
    public class CategoryDao : ICategoryDao
    {
        private readonly IList<Category> _categories;

        public CategoryDao(IList<Category> categories)
            => _categories = categories;

        public async Task<Category> GetAsync(int id)
        {
            var categories = await Task.Run(() => _categories);

            return categories.FirstOrDefault(c => c.Id == id);
        }

        public async Task<IPagedList<Category>> GetListAsync(CategoryCriteria criteria)
        {
            var categories = await Task.Run(() => _categories);

            var result = string.IsNullOrWhiteSpace(criteria.Name)
                ? categories
                : categories.Where(c => c.Name.Contains(criteria.Name));

            result = string.IsNullOrWhiteSpace(criteria.FullName)
                ? result
                : result.Where(c => c.Name == criteria.FullName);

            result = criteria.NotId.HasValue
                ? result.Where(c => c.Id != criteria.NotId.Value)
                : result;

            var ps = criteria.PageSize < 0 ? categories.Count : criteria.PageSize;
            result = result.Skip((criteria.PageIndex - 1) * ps).Take(ps);

            return new StaticPagedList<Category>(
                result,
                criteria.PageIndex,
                ps,
                categories.Count);
        }

        public async Task<Category> CreateAsync(Category category)
        {
            var categories = await Task.Run(() => _categories);

            category.Id = categories.Max(c => c.Id) + 1;

            categories.Add(category);

            return category;
        }

        public async Task<Category> UpdateAsync(Category category)
        {
            var categories = await Task.Run(() => _categories);

            var entity = categories.FirstOrDefault(c => c.Id == category.Id);
            entity.Name = category.Name;

            return category;
        }
    }
}
