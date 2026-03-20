namespace EsportsProfileWebApi.Web.Controllers;

using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class ConfigController : ControllerBase
{
    // [HttpPost]
    // public async Task<IActionResult> UploadConfig([FromForm] IFormFile file,int gameId)
    // {
    //     if (file == null || file.Length == 0)
    //         return BadRequest("No file provided.");

    //     var path = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", file.FileName);
    //     Directory.CreateDirectory(Path.GetDirectoryName(path)!);
    //     using (var stream = new FileStream(path, FileMode.Create))
    //     {
    //         await file.CopyToAsync(stream);
    //     }

    //     return Ok(new { message = "File uploaded successfully!"});
    // }

    // [HttpPost]
    // public async Task<IActionResult> GetConfig([FromForm] IFormFile file,int gameId,int userId)
    // {
    //     if (file == null || file.Length == 0)
    //         return BadRequest("No file provided.");

    //     var path = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", file.FileName);
    //     Directory.CreateDirectory(Path.GetDirectoryName(path)!);
    //     using (var stream = new FileStream(path, FileMode.Create))
    //     {
    //         await file.CopyToAsync(stream);
    //     }

    //     return Ok(new { message = "File uploaded successfully!"});
    // }
}