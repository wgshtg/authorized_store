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
        private const string ResDir = "Resources";

        public static IServiceCollection AddFakeModule(this IServiceCollection services)
        {
            var fileProvider = new PhysicalFileProvider(AppContext.BaseDirectory);
            var categories = fileProvider.DeserializeFromFile<Category>("categories.json");
            var stores = fileProvider.DeserializeFromFile<Store>("stores.json");

            services.AddSingleton<ICategoryDao>(_ => new CategoryDao(categories));
            services.AddSingleton<ICategoryService, CategoryService>();
            services.AddSingleton<IStoreDao>(_ => new StoreDao(stores));
            services.AddSingleton<IStoreService, StoreService>();

            return services;
        }

        public static IList<T> DeserializeFromFile<T>(this IFileProvider fileProvider, string filePath)
        {
            filePath = Path.Combine(ResDir, filePath);
            var fileInfo = fileProvider.GetFileInfo(filePath);
            if (fileInfo.Exists)
            {
                using var stream = fileInfo.CreateReadStream();
                using var reader = new StreamReader(stream);
                return JsonSerializer.Deserialize<IList<T>>(reader.ReadToEnd(), new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }

            return new List<T>();
        }
    }
}
