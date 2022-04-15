using Application.Interfaces.Services;
using MediatR;
using Shared.Wrapper;
using System.ComponentModel.DataAnnotations;

namespace Application.Features.CustomFolder.Commands;
public partial class CreateCustomFolderCommand : IRequest<IResult<bool>>
{
    [Required]
    public string Name { get; set; }

    [Required]
    public int CategoryId { get; set; }

    [Required]
    public string ColorHex { get; set; }
}

internal class CreateCustomFolderCommandHandler : IRequestHandler<CreateCustomFolderCommand, IResult<bool>>
{
    private readonly ICustomFolderService customFolderService;

    public CreateCustomFolderCommandHandler(ICustomFolderService customFolderService)
    {
        this.customFolderService = customFolderService;
    }

    public async Task<IResult<bool>> Handle(CreateCustomFolderCommand command, CancellationToken cancellationToken)
    {
        var shopItem = await customFolderService.CreateCustomFolderAsync(command, cancellationToken);
        return await Result<bool>.SuccessAsync(shopItem);
    }
}
