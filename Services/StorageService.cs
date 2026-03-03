
using VideoUploadServer.Dtos.Responses;

namespace VideoUploadServer.Services;

public class StorageService
{

    private readonly string _rootPath;

    public StorageService(IConfiguration configuration)
    {
        var storagePath = configuration["UploadSettings:StoragePath"] ??
            throw new InvalidOperationException("StoragePath não configurado no appsettings.json");

        _rootPath = Path.GetPathRoot(storagePath) ??
        throw new InvalidOperationException(
            "Não foi possível determinar o root do caminho configurado.");
    }

    public StorageInfo GetStorageInfo()
    {
        var drive = new DriveInfo(_rootPath);

        if (!drive.IsReady)
        {
            throw new InvalidOperationException("Drive não está disponível.");
        }


        long total = drive.TotalSize;
        long free = drive.AvailableFreeSpace;
        long used = total - free;
        double percentage = total == 0
       ? 0
       : (double)used / total * 100;

        return new StorageInfo
        {
            TotalBytes = total,
            FreeBytes = free,
            UsedBytes = used,
            UsedPercentage = percentage
        };
    }
}