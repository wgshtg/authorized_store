using System;
using System.Threading.Tasks;
using AuthorizedStore.Abstractions;
using Microsoft.Extensions.Configuration;
using X.PagedList;

namespace AuthorizedStore.Fake
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryDao _categoryDao;
        private readonly string[] _invalidNameKeywords;

        public CategoryService(ICategoryDao categoryDao, IConfiguration configuration)
        {
            _categoryDao = categoryDao;
            _invalidNameKeywords = configuration.GetSection("Categories:InvalidNameKeywords").Get<string[]>();
        }

        public async Task<IPagedList<Category>> GetListAsync(CategoryCriteria criteria)
        {
            Validate(criteria);

            criteria.PageIndex = criteria.PageIndex <= 0 ? 1 : criteria.PageIndex;
            criteria.PageSize = criteria.PageSize <= 0 ? -1 : criteria.PageSize;

            return await _categoryDao.GetListAsync(criteria);
        }

        private void Validate(CategoryCriteria criteria)
        {
            if (criteria == null)
            {
                throw new ArgumentNullException(nameof(criteria), "Criteria to get categories is required.");
            }

            if (!string.IsNullOrWhiteSpace(criteria.Name)
                && Array.Exists(_invalidNameKeywords, (k) => criteria.Name.Contains(k)))
            {
                throw new ArgumentException("Category name is invalid.", nameof(Category.Name));
            }
        }
    }
}
