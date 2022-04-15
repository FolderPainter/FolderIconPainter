using Application.Interfaces.Services;
using MediatR;
using Shared.Wrapper;

namespace Application.Features.Category.Commands;
public class DeleteCategoryCommand : IRequest<IResult<bool>>
{
    public DeleteCategoryCommand(int id)
    {
        Id = id;
    }

    public int Id { get; set; }
}

internal class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, IResult<bool>>
{
    private readonly ICategoryService categoryService;

    public DeleteCategoryCommandHandler(ICategoryService categoryService)
    {
        this.categoryService = categoryService;
    }

    public async Task<IResult<bool>> Handle(DeleteCategoryCommand command, CancellationToken cancellationToken)
    {
        var result = await categoryService.DeleteCategoryByIdAsync(command.Id, cancellationToken);
        return await Result<bool>.SuccessAsync(result);
    }
}
