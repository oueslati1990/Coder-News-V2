using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoderNews.Api.Controllers;

[Route("[Controller]")]
[Authorize]
public class PostsController : ApiController
{
    [HttpGet]
    public IActionResult ListPosts()
    {
        return Ok(Array.Empty<string>());
    }
}