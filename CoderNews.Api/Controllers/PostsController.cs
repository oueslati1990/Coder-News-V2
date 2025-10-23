using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoderNews.Api.Controllers;

/// <summary>
/// Posts management controller (requires authentication)
/// </summary>
[Route("[Controller]")]
[Authorize]
public class PostsController : ApiController
{
    /// <summary>
    /// Get all posts (placeholder endpoint)
    /// </summary>
    /// <returns>List of posts</returns>
    /// <response code="200">Posts retrieved successfully</response>
    /// <response code="401">Unauthorized - valid JWT token required</response>
    [HttpGet]
    [ProducesResponseType(typeof(string[]), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult ListPosts()
    {
        return Ok(Array.Empty<string>());
    }
}