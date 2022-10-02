using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Client.Controllers;
/// <summary>
/// Abstract BaseApi Controller Class
/// </summary>
[ApiController]
[Route("api/[controller]")]
public abstract class BaseApiController : ControllerBase
{
    //private ISender _mediator = null!;

    //protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();
    protected IMediator _mediator { get; set; }

    public BaseApiController(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException();
    }
}
