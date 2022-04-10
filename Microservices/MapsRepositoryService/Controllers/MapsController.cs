using Microsoft.AspNetCore.Mvc;

namespace MapsRepositoryService.Controllers;

[ApiController]
[Route("[controller]")]
public class MapsController : ControllerBase
{
    public record FileToUpload(string FileName, IFormFile File);

    [HttpGet("{id}")]
    public string Get(int id)
    {
        return "value";
    }

    [HttpPost]
    public IActionResult Post([FromForm]FileToUpload file)
    {
        return Ok();
    }
    

    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
}