﻿using System;
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
            Validate(criteria);

            criteria.PageIndex = criteria.PageIndex <= 0 ? 1 : criteria.PageIndex;
            criteria.PageSize = criteria.PageSize <= 0 ? -1 : criteria.PageSize;

            return await _categoryDao.GetListAsync(criteria);
        }

        public async Task<Category> CreateAsync(Category category)
        {
            await ValidateAsync(category);

            return await _categoryDao.CreateAsync(category);
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

        private async Task ValidateAsync(Category category)
        {
            if (string.IsNullOrWhiteSpace(category?.Name))
            {
                throw new ArgumentNullException(nameof(Category.Name), "Category name is required.");
            }

            ValidateIfNameIsInvalid(category.Name);

            var criteria = new CategoryCriteria
            {
                FullName = category.Name,
                PageIndex = 1,
                PageSize = 1
            };
            var categories = await _categoryDao.GetListAsync(criteria);
            if (categories.Count > 0)
            {
                throw new DuplicateNameException("The category with this name has already existed.");
            }
        }

        private void ValidateIfNameIsInvalid(string name)
        {
            if (Array.Exists(_invalidNameKeywords, (k) => name.Contains(k)))
            {
                throw new ArgumentException("Category name is invalid.", nameof(Category.Name));
            }
        }
    }
}
