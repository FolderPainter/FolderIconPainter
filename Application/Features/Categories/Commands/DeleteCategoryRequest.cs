using Application.Exceptions;
using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Wrapper;

namespace Application.Features.Categories.Commands;
public class DeleteCategoryRequest : IRequest<IResult<int>>
{
    public int Id { get; set; }

    public DeleteCategoryRequest(int id) => Id = id;

}

public class DeleteCategoryRequestHandler : IRequestHandler<DeleteCategoryRequest, IResult<int>>
{

    private readonly IUnitOfWork unitOfWork;

    public DeleteCategoryRequestHandler(IUnitOfWork unitOfWork) => this.unitOfWork = unitOfWork;

    public async Task<IResult<int>> Handle(DeleteCategoryRequest request, CancellationToken cancellationToken)
    {
        var category = await unitOfWork.RepositoryClassic<Category>().GetByIdAsync(request.Id, cancellationToken);

        _ = category ?? throw new NotFoundException("Category not found!");

        if (await unitOfWork.RepositoryClassic<CustomFolder>().Entities.AnyAsync(cf => cf.CategoryId == request.Id, cancellationToken))
        {
            throw new ConflictException("Category cannot be deleted as it's being used.");
        }

        await unitOfWork.RepositoryClassic<Category>().DeleteAsync(category);
        await unitOfWork.CommitAsync(cancellationToken);

        return await Result<int>.SuccessAsync(request.Id);
    }
}
