
using VideoUploadServer.Dtos.Responses;

namespace VideoUploadServer.Services;

public class StorageService
{

    private readonly DriveInfo _drive;
    public StorageService(IConfiguration configuration)
    {
        var storagePath = configuration["UploadSettings:StoragePath"] ??
            throw new InvalidOperationException("StoragePath não configurado no appsettings.json");

        var rootPath = Path.GetPathRoot(storagePath) ??
        throw new InvalidOperationException(
            "Não foi possível determinar o root do caminho configurado.");

        _drive = new DriveInfo(rootPath);
    }

    public StorageInfo GetStorageInfo()
    {
        if (!_drive.IsReady)
        {
            throw new InvalidOperationException("Drive não está disponível.");
        }

        long total = _drive.TotalSize;
        long free = _drive.AvailableFreeSpace;
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

    public long GetFreeBytes()
    {

        if (!_drive.IsReady)
        {
            throw new InvalidOperationException("Drive não está disponível.");
        }

        return _drive.AvailableFreeSpace;
    }
}