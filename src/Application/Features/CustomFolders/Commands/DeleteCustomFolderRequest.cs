using Application.Exceptions;
using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;
using Shared.Wrapper;

namespace Application.Features.CustomFolders.Commands;
public class DeleteCustomFolderRequest : IRequest<int>
{
    public DeleteCustomFolderRequest(int id)
    {
        Id = id;
    }

    public int Id { get; set; }
}

internal class DeleteCustomFolderRequestHandler : IRequestHandler<DeleteCustomFolderRequest, int>
{
    private readonly IUnitOfWork unitOfWork;

    public DeleteCustomFolderRequestHandler(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }

    public async Task<int> Handle(DeleteCustomFolderRequest request, CancellationToken cancellationToken)
    {
        var customFolder = await unitOfWork.RepositoryClassic<CustomFolder>().GetByIdAsync(request.Id, cancellationToken);

        _ = customFolder ?? throw new NotFoundException("Custom Folder not found!");

        await unitOfWork.RepositoryClassic<CustomFolder>().DeleteAsync(customFolder);
        return await unitOfWork.CommitAsync(cancellationToken);
    }
}