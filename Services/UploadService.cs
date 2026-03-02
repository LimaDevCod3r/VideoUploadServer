using FFMpegCore;
using VideoUploadServer.Dtos.Responses;

namespace VideoUploadServer.Services;


public class UploadService
{

    private readonly IConfiguration _configuration;

    public UploadService(IConfiguration configuration)
    {
        _configuration = configuration;
    }


    public List<VideoInfo> GetAllVideos()
    {
        var path = _configuration["UploadSettings:StoragePath"];

        if (!Directory.Exists(path))
        {
            return [];
        }

        var directoryInfo = new DirectoryInfo(path);
        var files = directoryInfo.GetFiles();
        var result = new List<VideoInfo>();

        foreach (var file in files)
        {
            var resolution = "N/A";
            try
            {
                var mediaInfo = FFProbe.Analyse(file.FullName);
                var width = mediaInfo.PrimaryVideoStream?.Width;
                var height = mediaInfo.PrimaryVideoStream?.Height;
                resolution = $"{width}x{height}";
            }
            catch
            {
                // arquivo não é um vídeo válido
            }

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
        var path = _configuration["UploadSettings:StoragePath"] ??
            throw new InvalidOperationException("StoragePath não configurado no appsettings.json");

        var filePath = Path.Combine(path, fileName);

        if (!File.Exists(filePath))
        {
            return null;
        }

        var fileInfo = new FileInfo(filePath);
        var resolution = "N/A";
        try
        {
            var mediaInfo = FFProbe.Analyse(filePath);
            var width = mediaInfo.PrimaryVideoStream?.Width;
            var height = mediaInfo.PrimaryVideoStream?.Height;
            resolution = $"{width}x{height}";
        }
        catch
        {
            // arquivo não é um vídeo válido
        }

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
        var path = _configuration["UploadSettings:StoragePath"] ??
            throw new InvalidOperationException("StoragePath não configurado no appsettings.json");

        Directory.CreateDirectory(path);


        var result = new List<UploadResult>();

        foreach (var video in videos)
        {
            var filePath = Path.Combine(path, video.FileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await video.CopyToAsync(stream);

            var resolution = "N/A";
            try
            {
                var mediaInfo = FFProbe.Analyse(filePath);
                var width = mediaInfo.PrimaryVideoStream?.Width;
                var height = mediaInfo.PrimaryVideoStream?.Height;
                resolution = $"{width}x{height}";
            }
            catch
            {
                // arquivo não é um vídeo válido
            }

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
        var path = _configuration["UploadSettings:StoragePath"] ??
            throw new InvalidOperationException("StoragePath não configurado no appsettings.json");

        var filePath = Path.Combine(path, fileName);

        if (!File.Exists(filePath))
        {
            return false;
        }

        File.Delete(filePath);
        return true;

    }

}