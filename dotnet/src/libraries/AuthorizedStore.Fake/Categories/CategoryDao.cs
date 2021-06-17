﻿using System.Collections.Generic;
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

        public async Task<IPagedList<Category>> GetListAsync(CategoryCriteria criteria)
        {
            var categories = await Task.Run(() => _categories);

            var result = string.IsNullOrWhiteSpace(criteria.Name)
                ? categories
                : categories.Where(c => c.Name.Contains(criteria.Name));

            var ps = criteria.PageSize < 0 ? categories.Count : criteria.PageSize;
            result = result.Skip((criteria.PageIndex - 1) * ps).Take(ps);

            return new StaticPagedList<Category>(
                result,
                criteria.PageIndex,
                ps,
                categories.Count);
        }
    }
}
