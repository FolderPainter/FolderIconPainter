using Application.Interfaces.Services;
using MediatR;
using Shared.Wrapper;

namespace Application.Features.CustomFolder.Commands;
public class DeleteCustomFolderCommand : IRequest<IResult<bool>>
{
    public DeleteCustomFolderCommand(int id)
    {
        Id = id;
    }

    public int Id { get; set; }
}

internal class DeleteCustomFolderCommandHandler : IRequestHandler<DeleteCustomFolderCommand, IResult<bool>>
{
    private readonly ICustomFolderService customFolderService;

    public DeleteCustomFolderCommandHandler(ICustomFolderService customFolderService)
    {
        this.customFolderService = customFolderService;
    }

    public async Task<IResult<bool>> Handle(DeleteCustomFolderCommand command, CancellationToken cancellationToken)
    {
        var result = await customFolderService.DeleteCustomFolderByIdAsync(command.Id, cancellationToken);
        return await Result<bool>.SuccessAsync(result);
    }
}