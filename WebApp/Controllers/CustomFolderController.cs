using Application.Features.CustomFolders.Commands;
using Application.Features.CustomFolders.Queries;
using Microsoft.AspNetCore.Mvc;
using Shared.Wrapper;
using System.Threading.Tasks;

namespace WebApp.Controllers;
public class CustomFolderController : BaseApiController
{
    [HttpPost("search")]
    public Task<PaginatedResult<CustomFolderDto>> SearchAsync(SearchCustomFoldersRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpGet("{id:int}")]
    public Task<CustomFolderDto> GetAsync(int id)
    {
        return Mediator.Send(new GetCustomFolderRequest(id));
    }

    [HttpPost]
    public Task<int> CreateAsync(CreateCustomFolderRequest request)
    {
        return Mediator.Send(request);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<int>> UpdateAsync(PatchCustomFolderRequest request, int id)
    {
        return id != request.Id
            ? BadRequest()
            : Ok(await Mediator.Send(request));
    }

    [HttpDelete("{id:int}")]
    public Task<int> DeleteAsync(int id)
    {
        return Mediator.Send(new DeleteCustomFolderRequest(id));
    }
}
