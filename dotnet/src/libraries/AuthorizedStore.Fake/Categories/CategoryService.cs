using System;
using System.Data;
using System.Threading.Tasks;
using AuthorizedStore.Abstractions;
using AuthorizedStore.Extensions;
using X.PagedList;

namespace AuthorizedStore.Fake
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryDao _categoryDao;

        public CategoryService(ICategoryDao categoryDao)
            => _categoryDao = categoryDao;

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

        private async Task ValidateAsync(Category category, int? excludedId = null)
        {
            if (string.IsNullOrWhiteSpace(category?.Name))
            {
                throw new ArgumentNullException(nameof(Category.Name), "Name is required.");
            }

            ValidateIfNameIsInvalid(category.Name);
            await ValidateIfDuplicateCategoryExistsAsync(category.Name, excludedId);
        }

        private void ValidateIfNameIsInvalid(string name)
        {
            if (name.ContainsInvalidKeywords())
            {
                throw new ArgumentException("Name is invalid.", nameof(Category.Name));
            }
        }

        private async Task ValidateIfDuplicateCategoryExistsAsync(string name, int? excludedId = null)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return;
            }

            var category = await _categoryDao.GetDuplicateAsync(name, excludedId);
            if (category != null)
            {
                throw new DuplicateNameException("Name has already been existed.");
            }
        }
    }
}
