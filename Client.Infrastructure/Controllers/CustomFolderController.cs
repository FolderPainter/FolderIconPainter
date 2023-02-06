using Application.Features.CustomFolders.Commands;
using Application.Features.CustomFolders.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Wrapper;
using System.Threading.Tasks;

namespace Client.Controllers;
public class CustomFolderController : BaseApiController
{
    public CustomFolderController(IMediator mediator) : base(mediator) { }

    [HttpPost("search")]
    public Task<PaginatedResult<CustomFolderDto>> SearchAsync(SearchCustomFoldersRequest request)
    {
        return _mediator.Send(request);
    }

    [HttpGet("{id:int}")]
    public Task<CustomFolderDto> GetAsync(int id)
    {
        return _mediator.Send(new GetCustomFolderRequest(id));
    }

    [HttpPost]
    public Task<int> CreateAsync(CreateCustomFolderRequest request)
    {
        return _mediator.Send(request);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<int>> UpdateAsync(PatchCustomFolderRequest request, int id)
    {
        return id != request.Id
            ? BadRequest()
            : Ok(await _mediator.Send(request));
    }

    [HttpDelete("{id:int}")]
    public Task<int> DeleteAsync(int id)
    {
        return _mediator.Send(new DeleteCustomFolderRequest(id));
    }
}
