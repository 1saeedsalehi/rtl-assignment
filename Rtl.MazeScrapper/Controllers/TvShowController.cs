using MediatR;
using Microsoft.AspNetCore.Mvc;
using Rtl.MazeScrapper.Application.Queries;

namespace Rtl.MazeScrapper.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TvShowController : ControllerBase
{
    private readonly IMediator _mediator;

    public TvShowController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Route("shows")]
    public async Task<IActionResult> GetTvShows([FromQuery] GetTvShowsQuery request,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(request, cancellationToken);
        return Ok(response);
    }
}
