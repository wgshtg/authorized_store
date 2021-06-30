using System;
using System.Data;
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

        public async Task<Category> GetAsync(int id)
            => await _categoryDao.GetAsync(id);

        public async Task<IPagedList<Category>> GetListAsync(CategoryCriteria criteria)
        {
            if (criteria != null)
            {
                if (!string.IsNullOrWhiteSpace(criteria.Name))
                {
                    ValidateIfNameIsInvalid(criteria.Name);
                }

                criteria.PageIndex = criteria.PageIndex <= 0 ? 1 : criteria.PageIndex;
                criteria.PageSize = criteria.PageSize <= 0 ? 10 : criteria.PageSize;
            }

            return await _categoryDao.GetListAsync(criteria);
        }

        public async Task<Category> CreateAsync(Category category)
        {
            await ValidateAsync(category);

            return await _categoryDao.CreateAsync(category);
        }

        public async Task<Category> UpdateAsync(int id, Category entity)
        {
            var category = await _categoryDao.GetAsync(id);
            if (category == null)
            {
                // TODO: throw specific exception.
                return null;
            }

            await ValidateAsync(entity, id);

            entity.Id = id;
            entity.Name = entity.Name;

            return await _categoryDao.UpdateAsync(entity);
        }

        public async Task DeleteAsync(int id)
            => await _categoryDao.DeleteAsync(id);

        private async Task ValidateAsync(Category category, int? notId = null)
        {
            if (string.IsNullOrWhiteSpace(category?.Name))
            {
                throw new ArgumentNullException(nameof(Category.Name), "Name is required.");
            }

            ValidateIfNameIsInvalid(category.Name);

            var criteria = new CategoryCriteria
            {
                NotId = notId,
                FullName = category.Name,
                PageIndex = 1,
                PageSize = 1
            };
            var categories = await _categoryDao.GetListAsync(criteria);
            if (categories.Count > 0)
            {
                throw new DuplicateNameException("Name has already been existed.");
            }
        }

        private void ValidateIfNameIsInvalid(string name)
        {
            if (Array.Exists(_invalidNameKeywords, (k) => name.Contains(k)))
            {
                throw new ArgumentException("Name is invalid.", nameof(Category.Name));
            }
        }
    }
}
