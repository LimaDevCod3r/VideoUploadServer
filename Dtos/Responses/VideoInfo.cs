namespace VideoUploadServer.Dtos.Responses;


public class VideoInfo
{
    public required string Name { get; set; }
    public long Length { get; set; }

    public required string Resolution { get; set; }
    public required string Extension { get; set; }
    public DateTime CreationTimeUtc { get; set; }


}