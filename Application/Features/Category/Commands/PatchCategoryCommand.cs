using Application.Interfaces.Services;
using FluentValidation;
using MediatR;
using Shared.Wrapper;
using System.ComponentModel.DataAnnotations;

namespace Application.Features.Category.Commands;
public partial class PatchCategoryCommand : IRequest<IResult<bool>>
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }
}

public class PatchCategoryCommandValidator : AbstractValidator<PatchCategoryCommand>
{
    public PatchCategoryCommandValidator()
    {
        RuleFor(v => v.Id).NotNull().WithMessage("Id must be required");
        RuleFor(v => v.Name).Must(NotNullOrWhiteSpace).WithMessage("Category name is empty");
    }

    protected bool NotNullOrWhiteSpace(string name) => !string.IsNullOrWhiteSpace(name);
}

internal class PatchCategoryCommandHandler : IRequestHandler<PatchCategoryCommand, IResult<bool>>
{
    private readonly ICategoryService categoryService;

    public PatchCategoryCommandHandler(ICategoryService categoryService)
    {
        this.categoryService = categoryService;
    }

    public async Task<IResult<bool>> Handle(PatchCategoryCommand command, CancellationToken cancellationToken)
    {
        var result = await categoryService.PatchCategoryAsync(command, cancellationToken);
        return await Result<bool>.SuccessAsync(result);
    }
}
