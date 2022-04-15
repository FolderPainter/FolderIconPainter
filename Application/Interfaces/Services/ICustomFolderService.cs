using Application.Features.CustomFolder.Commands;
using Application.Features.CustomFolder.Queries.GetAllPaged;
using Domain.Entities;
using Shared.Wrapper;

namespace Application.Interfaces.Services;
public interface ICustomFolderService
{
    Task<bool> CreateCustomFolderAsync(CreateCustomFolderCommand customFolder, CancellationToken cancellationToken);

    Task<bool> PatchCustomFolderAsync(PatchCustomFolderCommand command, CancellationToken cancellationToken);

    Task<bool> DeleteCustomFolderByIdAsync(int id, CancellationToken cancellationToken);

    Task<CustomFolder> GetCustomFolderById(int id, CancellationToken cancellationToken);

    Task<PaginatedResult<GetAllPagedCustomFoldersResponse>> GetAllPagedShopItems(GetAllPagedCustomFoldersQuery request, CancellationToken cancellationToken);
}
