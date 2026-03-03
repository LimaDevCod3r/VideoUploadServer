using FFMpegCore;
using VideoUploadServer.Dtos.Responses;

namespace VideoUploadServer.Services;


public class UploadService
{

    private readonly string _storagePath;

    public UploadService(IConfiguration configuration)
    {
        _storagePath = configuration["UploadSettings:StoragePath"] ??
            throw new InvalidOperationException("StoragePath não configurado no appsettings.json");

    }


    public List<VideoInfo> GetAllVideos()
    {

        if (!Directory.Exists(_storagePath))
        {
            return [];
        }

        var directoryInfo = new DirectoryInfo(_storagePath);
        var files = directoryInfo.GetFiles();
        var result = new List<VideoInfo>();

        foreach (var file in files)
        {
            var resolution = GetResolution(file.FullName);

            result.Add(new VideoInfo
            {
                Name = file.Name,
                Length = file.Length,
                Extension = file.Extension,
                Resolution = resolution,
                CreationTimeUtc = file.CreationTimeUtc

            });

        }

        return result;
    }


    public VideoInfo? GetVideoByName(string fileName)
    {

        var filePath = Path.Combine(_storagePath, fileName);

        if (!File.Exists(filePath))
        {
            return null;
        }

        var fileInfo = new FileInfo(filePath);
        var resolution = GetResolution(filePath);

        return new VideoInfo
        {
            Name = fileInfo.Name,
            Length = fileInfo.Length,
            Extension = fileInfo.Extension,
            Resolution = resolution,
            CreationTimeUtc = fileInfo.CreationTimeUtc
        };
    }

    public async Task<List<UploadResult>> UploadVideos(List<IFormFile> videos)
    {
        Directory.CreateDirectory(_storagePath);

        var result = new List<UploadResult>();

        foreach (var video in videos)
        {
            var filePath = Path.Combine(_storagePath, video.FileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await video.CopyToAsync(stream);

            var resolution = GetResolution(filePath);

            result.Add(new UploadResult
            {
                FileName = video.FileName,
                FilePath = filePath,
                FileSize = video.Length,
                Resolution = resolution,
                UploadedAt = DateTime.UtcNow

            });
        }

        return result;

    }


    public bool DeleteVideo(string fileName)
    {

        var filePath = Path.Combine(_storagePath, fileName);

        if (!File.Exists(filePath))
        {
            return false;
        }

        File.Delete(filePath);
        return true;

    }

    private static string GetResolution(string path)
    {
        try
        {
            var mediaInfo = FFProbe.Analyse(path);
            var width = mediaInfo.PrimaryVideoStream?.Width;
            var height = mediaInfo.PrimaryVideoStream?.Height;
            var resolution = $"{width}x{height}";
            return resolution;
        }
        catch
        {
            return "N/A";
        }
    }

}