using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Restaurants.Domain.Interfaces;

public interface IBlobStorageService
{
    Task<string> UploadToBlobAsync(string fileName, Stream data);

    string? GetBlobSasUrl(string? blobUrl);
}
