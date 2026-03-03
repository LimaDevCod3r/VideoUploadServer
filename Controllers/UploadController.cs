using Microsoft.AspNetCore.Mvc;
using VideoUploadServer.Services;

namespace VideoUploadServer.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UploadController : ControllerBase
{

    private readonly UploadService _uploadService;


    public UploadController(UploadService uploadService)
    {
        _uploadService = uploadService;
    }


    [HttpGet]
    public IActionResult GetAllVideos()
    {
        var result = _uploadService.GetAllVideos();
        return Ok(result);
    }

    [HttpGet("{fileName}")]
    public IActionResult GetVideoByName(string fileName)
    {
        var result = _uploadService.GetVideoByName(fileName);

        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> UploadVideos([FromForm] List<IFormFile> videos)
    {
        var result = await _uploadService.UploadVideos(videos);
        return StatusCode(201, result);
    }

    [HttpDelete("{fileName}")]
    public IActionResult DeleteVideo(string fileName)
    {
        var result = _uploadService.DeleteVideo(fileName);

        if (!result)
        {
            return NotFound();
        }

        return NoContent();

    }
}