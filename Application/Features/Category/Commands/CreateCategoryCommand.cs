using Application.Interfaces.Services;
using MediatR;
using Shared.Wrapper;
using System.ComponentModel.DataAnnotations;

namespace Application.Features.Category.Commands;
public partial class CreateCategoryCommand : IRequest<IResult<bool>>
{
    [Required]
    public string Name { get; set; }
}

internal class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, IResult<bool>>
{
    private readonly ICategoryService categoryService;

    public CreateCategoryCommandHandler(ICategoryService categoryService)
    {
        this.categoryService = categoryService;
    }

    public async Task<IResult<bool>> Handle(CreateCategoryCommand command, CancellationToken cancellationToken)
    {
        var shopItem = await categoryService.CreateCategoryAsync(command, cancellationToken);
        return await Result<bool>.SuccessAsync(shopItem);
    }
}
