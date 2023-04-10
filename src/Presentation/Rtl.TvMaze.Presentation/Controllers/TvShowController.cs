using MediatR;
using Microsoft.AspNetCore.Mvc;
using Rtl.TvMaze.Application.Queries;

namespace Rtl.TvMaze.Presentation.Controllers;

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
    public async Task<IActionResult> GetTvShows([FromQuery] GetTvShowsQuery request,
        CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(request, cancellationToken);
        return Ok(response);
    }
}
