using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;
/// <summary>
/// Abstract BaseApi Controller Class
/// </summary>
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public abstract class BaseApiController<T> : ControllerBase
{
    public BaseApiController(IMediator mediator)
    {
        this.mediator = mediator;
    }
    protected IMediator mediator { get; set; }
}
