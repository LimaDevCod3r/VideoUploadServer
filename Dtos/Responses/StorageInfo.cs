namespace VideoUploadServer.Dtos.Responses;

public class StorageInfo
{
    public long TotalBytes { get; set; }
    public long FreeBytes { get; set; }
    public long UsedBytes { get; set; }
    public double UsedPercentage { get; set; }
}