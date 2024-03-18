namespace CarRentalApplication.StorageProviders;

public interface IStorageProvider
{
    Task DeleteAsync(string path, CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(string path, CancellationToken cancellationToken = default);

    Task<Stream?> ReadAsStreamAsync(string path, CancellationToken cancellationToken = default);

    async Task<string?> ReadAsStringAsync(string path, CancellationToken cancellationToken = default)
    {
        using var stream = await ReadAsStreamAsync(path, cancellationToken);
        if (stream is null)
        {
            return null;
        }

        using var reader = new StreamReader(stream);
        return await reader.ReadToEndAsync(cancellationToken);
    }

    async Task<byte[]?> ReadAsByteArrayAsync(string path, CancellationToken cancellationToken = default)
    {
        using var stream = await ReadAsStreamAsync(path, cancellationToken);
        if (stream is null)
        {
            return null;
        }

        using var memoryStream = new MemoryStream();
        await stream.CopyToAsync(memoryStream, cancellationToken);

        stream.Close();
        return memoryStream.ToArray();
    }

    Task SaveAsync(Stream stream, string path, bool overwrite = false, CancellationToken cancellationToken = default);

    async Task SaveAsync(byte[] content, string path, bool overwrite = false, CancellationToken cancellationToken = default)
    {
        using var stream = new MemoryStream(content);
        await SaveAsync(stream, path, overwrite, cancellationToken);
    }

    async Task UpdateAsync(string oldPath, string newPath, CancellationToken cancellationToken = default)
    {
        var stream = await ReadAsStreamAsync(oldPath, cancellationToken);
        if (stream is null)
        {
            throw new IOException("can't find the stream for the specified path");
        }

        await DeleteAsync(oldPath, cancellationToken);
        await SaveAsync(stream, newPath, true, cancellationToken);
    }
}