using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

[Route("api/category")]
[ApiController]
public class CategoryController : BaseApiController<CategoryController>
{
    public CategoryController(IMediator mediator) : base(mediator)
    {
    }
}
