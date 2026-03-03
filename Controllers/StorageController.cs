using Microsoft.AspNetCore.Mvc;
using VideoUploadServer.Services;

namespace VideoUploadServer.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StorageController : ControllerBase
{
    private readonly StorageService _storageService;

    public StorageController(StorageService storageService)
    {
        _storageService = storageService;
    }


    [HttpGet]
    public IActionResult GetStorageInfo()
    {
        var result = _storageService.GetStorageInfo();
        return Ok(result);
    }
}