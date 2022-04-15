using Application.Exceptions;
using Application.Extensions;
using Application.Features.CustomFolder.Commands;
using Application.Features.CustomFolder.Queries.GetAllPaged;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Specifications;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Shared.Wrapper;

namespace Infrastructure.Services;
public class CustomFolderService : ICustomFolderService
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;

    public CustomFolderService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
    }

    public async Task<bool> CreateCustomFolderAsync(CreateCustomFolderCommand command, CancellationToken cancellationToken)
    {
        await unitOfWork.RepositoryClassic<CustomFolder>().AddAsync(mapper.Map<CustomFolder>(command), cancellationToken);

        int resultEntitySave = await unitOfWork.CommitAsync(cancellationToken);
        return resultEntitySave != 0;
    }

    public async Task<bool> PatchCustomFolderAsync(PatchCustomFolderCommand command, CancellationToken cancellationToken)
    {
        var customFolder = await unitOfWork.RepositoryClassic<CustomFolder>().GetByIdAsync(command.Id, cancellationToken);
        if (customFolder != null)
        {
            customFolder.Name = command.Name ?? customFolder.Name;
            customFolder.ColorHex = command.ColorHex ?? customFolder.ColorHex;
            customFolder.CategoryId = (command.CategoryId == 0) ? customFolder.CategoryId : command.CategoryId;

            await unitOfWork.RepositoryClassic<CustomFolder>().UpdateAsync(customFolder);
            await unitOfWork.CommitAsync(cancellationToken);
            return true;
        }
        return false;
    }

    public async Task<bool> DeleteCustomFolderByIdAsync(int id, CancellationToken cancellationToken)
    {
        var customFolder = await unitOfWork.RepositoryClassic<CustomFolder>().GetByIdAsync(id, cancellationToken);
        if (customFolder == null)
        {
            return false;
        }
        await unitOfWork.RepositoryClassic<CustomFolder>().DeleteAsync(customFolder);
        return await unitOfWork.CommitAsync(cancellationToken) != 0;
    }

    public async Task<PaginatedResult<GetAllPagedCustomFoldersResponse>> GetAllPagedShopItems(GetAllPagedCustomFoldersQuery request, CancellationToken cancellationToken)
    {
        bool? isSortDescending = null;

        if (request.SortDirection == SortDirection.Ascending)
            isSortDescending = true;
        else if (request.SortDirection == SortDirection.Descending)
            isSortDescending = false;

        CustomFolderFilterSpecification specification = new CustomFolderFilterSpecification(request.SearchString);
        var data = unitOfWork
            .RepositoryClassic<CustomFolder>()
            .Entities
            .Include(element => element.Category)
            .Specify(specification)
            .ProjectTo<GetAllPagedCustomFoldersResponse>(mapper.ConfigurationProvider);

        data = isSortDescending.Value ? data.OrderByDescending(element => element.Name)
              : data.OrderBy(element => element.Name);

        return await data.ToPaginatedListAsync(request.PageNumber, request.PageSize, cancellationToken);
    }

    public async Task<CustomFolder> GetCustomFolderById(int id, CancellationToken cancellationToken)
    {
        var customFolder = await unitOfWork.RepositoryClassic<CustomFolder>().GetByIdAsync(id, cancellationToken);
        if (customFolder == null)
        {
            throw new NotFoundException(nameof(CustomFolder), id);
        }
        return customFolder;
    }
}
