using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using AuthorizedStore.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;

namespace AuthorizedStore.Fake.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddFakeModule(this IServiceCollection services)
        {
            var fileProvider = new PhysicalFileProvider(AppContext.BaseDirectory);
            var filePath = Path.Combine("Categories", "categories.json");
            var fileInfo = fileProvider.GetFileInfo(filePath);

            IList<Category> categoriesSource;
            if (fileInfo.Exists)
            {
                using var stream = fileInfo.CreateReadStream();
                using var reader = new StreamReader(stream);
                categoriesSource = JsonSerializer.Deserialize<IList<Category>>(reader.ReadToEnd(), new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            else
            {
                categoriesSource = new List<Category>();
            }

            return services.AddSingleton<ICategoryDao>(_ => new CategoryDao(categoriesSource))
                .AddSingleton<ICategoryService, CategoryService>();
        }
    }
}
