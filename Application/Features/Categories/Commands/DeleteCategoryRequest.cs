using Application.Exceptions;
using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Categories.Commands;
public class DeleteCategoryRequest : IRequest<int>
{
    public int Id { get; set; }

    public DeleteCategoryRequest(int id) => Id = id;
}

public class DeleteCategoryRequestHandler : IRequestHandler<DeleteCategoryRequest, int>
{
    private readonly IUnitOfWork unitOfWork;

    public DeleteCategoryRequestHandler(IUnitOfWork unitOfWork) => this.unitOfWork = unitOfWork;

    public async Task<int> Handle(DeleteCategoryRequest request, CancellationToken cancellationToken)
    {
        var category = await unitOfWork.RepositoryClassic<Category>().GetByIdAsync(request.Id, cancellationToken);

        _ = category ?? throw new NotFoundException("Category not found!");

        if (await unitOfWork.RepositoryClassic<CustomFolder>().Entities.AnyAsync(cf => cf.CategoryId == request.Id, cancellationToken))
        {
            throw new ConflictException("Category cannot be deleted as it's being used.");
        }

        await unitOfWork.RepositoryClassic<Category>().DeleteAsync(category);
        return await unitOfWork.CommitAsync(cancellationToken);
    }
}
