using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.Json;
using AuthorizedStore;
using AuthorizedStore.Abstractions;
using AuthorizedStore.Fake;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;

namespace WebApp.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMockData(this IServiceCollection services)
        {
            var fileProvider = new EmbeddedFileProvider(Assembly.GetEntryAssembly(), typeof(Startup).Namespace);
            var file = fileProvider.GetFileInfo("stores.json");
            using var stream = file.CreateReadStream();
            using var reader = new StreamReader(stream);
            var storesSource = JsonSerializer.Deserialize<List<Store>>(reader.ReadToEnd(), new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return services.AddSingleton<IStoreDao>(_ => new StoreDao(storesSource));
        }
    }
}
