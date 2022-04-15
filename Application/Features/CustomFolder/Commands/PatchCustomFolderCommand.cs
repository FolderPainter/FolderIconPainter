using Application.Interfaces.Services;
using FluentValidation;
using MediatR;
using Shared.Wrapper;
using System.ComponentModel.DataAnnotations;

namespace Application.Features.CustomFolder.Commands;
public partial class PatchCustomFolderCommand : IRequest<IResult<bool>>
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public int CategoryId { get; set; }

    [Required]
    public string ColorHex { get; set; }
}

public class PatchCustomFolderCommandValidator : AbstractValidator<PatchCustomFolderCommand>
{
    public PatchCustomFolderCommandValidator()
    {
        RuleFor(v => v.Id).NotNull().WithMessage("Id must be required");
        RuleFor(v => v.Name).Must(NotNullOrWhiteSpace).WithMessage("Category name is empty");
        RuleFor(v => v.CategoryId).NotNull().WithMessage("Category must be required");
        RuleFor(v => v.ColorHex).NotNull().WithMessage("Color HEX must be required");
    }

    protected bool NotNullOrWhiteSpace(string name) => !string.IsNullOrWhiteSpace(name);
}

internal class PatchCustomFolderCommandHandler : IRequestHandler<PatchCustomFolderCommand, IResult<bool>>
{
    private readonly ICustomFolderService customFolderService;

    public PatchCustomFolderCommandHandler(ICustomFolderService customFolderService)
    {
        this.customFolderService = customFolderService;
    }

    public async Task<IResult<bool>> Handle(PatchCustomFolderCommand command, CancellationToken cancellationToken)
    {
        var result = await customFolderService.PatchCustomFolderAsync(command, cancellationToken);
        return await Result<bool>.SuccessAsync(result);
    }
}
