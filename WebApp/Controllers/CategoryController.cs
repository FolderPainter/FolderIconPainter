using Application.Features.Categories.Commands;
using Application.Features.Categories.Queries;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApp.Controllers;

public class CategoryController : BaseApiController
{
    [HttpPost("search")]
    public Task<IEnumerable<CategoryDto>> SearchAsync(SearchCategoriesRequest request)
    {
        return Mediator.Send(request);
    }


    //[HttpPost("search")]
    //public Task<PaginationResponse<CategoryDto>> SearchAsync(SearchCategoriesRequest request)
    //{
    //    return Mediator.Send(request);
    //}

    [HttpGet("{id:int}")]
    public Task<CategoryDto> GetAsync(int id)
    {
        return Mediator.Send(new GetCategoryRequest(id));
    }

    [HttpPost]
    public Task<int> CreateAsync(CreateCategoryRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<int>> UpdateAsync(PatchCategoryRequest request, int id)
    {
        return id != request.Id
            ? BadRequest()
            : Ok(await Mediator.Send(request));
    }

    [HttpDelete("{id:int}")]
    public Task<int> DeleteAsync(int id)
    {
        return Mediator.Send(new DeleteCategoryRequest(id));
    }
}
