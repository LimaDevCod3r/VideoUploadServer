namespace VideoUploadServer.Dtos.Responses;


public class UploadResult
{
    public required string FileName { get; set; }
    public required string FilePath { get; set; }
    public long FileSize { get; set; }

    public required string Resolution { get; set; }
    public DateTime UploadedAt { get; set; }
}
