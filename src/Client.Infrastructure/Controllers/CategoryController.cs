using Application.Features.Categories.Commands;
using Application.Features.Categories.Queries;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client.Controllers;

public class CategoryController : BaseApiController
{
    public CategoryController(IMediator mediator) : base(mediator) { }

    [HttpPost("search")]
    public Task<IEnumerable<CategoryDto>> SearchAsync(SearchCategoriesRequest request)
    {
        return _mediator.Send(request);
    }

    //[HttpPost("search")]
    //public Task<PaginationResponse<CategoryDto>> SearchAsync(SearchCategoriesRequest request)
    //{
    //    return _mediator.Send(request);
    //}

    [HttpGet("{id:int}")]
    public Task<CategoryDto> GetAsync(int id)
    {
        return _mediator.Send(new GetCategoryRequest(id));
    }

    [HttpPost]
    public Task<int> CreateAsync(CreateCategoryRequest request)
    {
        return _mediator.Send(request);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<int>> UpdateAsync(PatchCategoryRequest request, int id)
    {
        return id != request.Id
            ? BadRequest()
            : Ok(await _mediator.Send(request));
    }

    [HttpDelete("{id:int}")]
    public Task<int> DeleteAsync(int id)
    {
        return _mediator.Send(new DeleteCategoryRequest(id));
    }
}
