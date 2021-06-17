using System.Collections.Generic;
using AuthorizedStore.Abstractions;

namespace AuthorizedStore.Fake
{
    public class CategoryDao : ICategoryDao
    {
        private readonly IList<Category> _categories;

        public CategoryDao(IList<Category> categories)
            => _categories = categories;
    }
}
