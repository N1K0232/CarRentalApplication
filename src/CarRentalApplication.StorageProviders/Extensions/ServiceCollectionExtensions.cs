using CarRentalApplication.StorageProviders.Azure;
using CarRentalApplication.StorageProviders.FileSystem;
using Microsoft.Extensions.DependencyInjection;

namespace CarRentalApplication.StorageProviders.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAzureStorage(this IServiceCollection services, Action<AzureStorageSettings> configuration)
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));
        ArgumentNullException.ThrowIfNull(configuration, nameof(configuration));

        var settings = new AzureStorageSettings();
        configuration.Invoke(settings);

        services.AddSingleton(settings);
        services.AddScoped<IStorageProvider, AzureStorageProvider>();

        return services;
    }

    public static IServiceCollection AddFileSystemStorage(this IServiceCollection services, Action<FileSystemStorageSettings> configuration)
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));
        ArgumentNullException.ThrowIfNull(configuration, nameof(configuration));

        var settings = new FileSystemStorageSettings();
        configuration.Invoke(settings);

        services.AddSingleton(settings);
        services.AddScoped<IStorageProvider, FileSystemStorageProvider>();

        return services;
    }
}