using Application.Features.CustomFolders.Commands;
using Application.Features.CustomFolders.Queries;
using FluentValidation;
using MediatR;

namespace Client.Infrastructure.Services;

public class CustomFolderService : BaseService
{
    private readonly IValidator<CreateCustomFolderRequest> _createValidator;

    public CustomFolderService(IMediator mediator, IValidator<CreateCustomFolderRequest> validator) : base(mediator)
    {
        _createValidator = validator;
    }

    //public Task<PaginatedResult<CustomFolderDto>> SearchAsync(SearchCustomFoldersRequest request)
    //{
    //    return _mediator.Send(request);
    //}

    public Task<CustomFolderDto> GetAsync(int id)
    {
        return _mediator.Send(new GetCustomFolderRequest(id));
    }

    public Task<int> CreateAsync(CreateCustomFolderRequest request)
    {
        return _mediator.Send(request);
    }

    //public async Task<ActionResult<int>> UpdateAsync(PatchCustomFolderRequest request, int id)
    //{
    //    return id != request.Id
    //        ? BadRequest()
    //        : Ok(await _mediator.Send(request));
    //}

    public Task<int> DeleteAsync(int id)
    {
        return _mediator.Send(new DeleteCustomFolderRequest(id));
    }
}
