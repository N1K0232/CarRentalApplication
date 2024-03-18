namespace CarRentalApplication.StorageProviders.FileSystem;

public class FileSystemStorageProvider : IStorageProvider
{
    private readonly FileSystemStorageSettings settings;

    public FileSystemStorageProvider(FileSystemStorageSettings settings)
    {
        this.settings = settings;
    }

    public Task DeleteAsync(string path, CancellationToken cancellationToken = default)
    {
        var fullPath = Path.Combine(settings.StorageFolder, path);

        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }

        return Task.CompletedTask;
    }

    public Task<bool> ExistsAsync(string path, CancellationToken cancellationToken = default)
    {
        var fullPath = Path.Combine(settings.StorageFolder, path);
        var exists = File.Exists(fullPath);

        return Task.FromResult(exists);
    }

    public Task<Stream?> ReadAsStreamAsync(string path, CancellationToken cancellationToken = default)
    {
        var fullPath = Path.Combine(settings.StorageFolder, path);
        if (!File.Exists(fullPath))
        {
            return Task.FromResult<Stream?>(null);
        }

        var stream = File.OpenRead(fullPath);
        return Task.FromResult<Stream?>(stream);
    }

    public async Task SaveAsync(Stream stream, string path, bool overwrite = false, CancellationToken cancellationToken = default)
    {
        var fullPath = Path.Combine(settings.StorageFolder, path);
        var directoryName = Path.GetDirectoryName(fullPath);

        if (!string.IsNullOrWhiteSpace(directoryName) && !Directory.Exists(directoryName))
        {
            Directory.CreateDirectory(directoryName);
        }

        if (!overwrite)
        {
            if (File.Exists(fullPath))
            {
                throw new IOException($"The file {path} already exists");
            }
        }

        using var outputStream = new FileStream(fullPath, FileMode.Create, FileAccess.Write);
        stream.Position = 0;

        await stream.CopyToAsync(outputStream, cancellationToken);
        outputStream.Close();
    }
}